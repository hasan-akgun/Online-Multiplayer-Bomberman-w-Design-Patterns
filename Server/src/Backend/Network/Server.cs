using System.Collections.Concurrent;
using System.Net.Sockets;
using src.Backend.Entities;
using src.Backend.Network.Requests.Login;
using src.Backend.Network.Requests.Abstractions;
using src.Backend.Network.Requests.Match;
using System.Net;
using src.Backend.Network.Requests.Update;
using src.Backend.Network.Requests.Theme;
using src.Backend.Network.Requests.Leaderboard;

namespace src.Backend.Network
{
  public class Server
  {
    public ConcurrentDictionary<int, Client> clients = new ConcurrentDictionary<int, Client>();
    public ConcurrentDictionary<int, Player> players = new ConcurrentDictionary<int, Player>();
    public ConcurrentDictionary<int, ConcurrentDictionary<int, Player>> games = new ConcurrentDictionary<int, ConcurrentDictionary<int, Player>>();
    public ConcurrentDictionary<int, string> maps = new ConcurrentDictionary<int, string>();
    private TcpHandler tcp_handler;
    private UdpHandler udp_handler;
    private int last_client_id = 0;

    public Server()
    {
      tcp_handler = new TcpHandler(this);
      udp_handler = new UdpHandler(this);
    }

    public void Start()
    {
      tcp_handler.Start();
      udp_handler.Start();
    }

    public void ConnectNewClient(TcpClient tcp_socket)
    {
      int new_id = Interlocked.Increment(ref last_client_id);
      Client new_client = new Client(new_id, tcp_socket);
      if (clients.TryAdd(new_id, new_client))
      {
          Console.WriteLine($"Player {new_id} connected. IP: {new_client.endpoint}");
          _ = tcp_handler.HandleClientComm(new_client);
      }
    }

    public void DisconnectClient(int id)
    {
      if (clients.TryRemove(id, out Client? client))
      {
        client.Disconnect();
      }
      players.TryRemove(id, out Player? player);
    }

    public void AddNewPlayer(Client client, Player player)
    {
      int id  = client.id;
      players.TryAdd(id, player);
    }

    public void UpdateClientUdpEndPoint(int clientId, IPEndPoint udpEndPoint)
    {
      if (clients.TryGetValue(clientId, out Client? client))
      {
        if (client.endpoint == null || !client.endpoint.Equals(udpEndPoint))
        {
          client.udp_endpoint = udpEndPoint; 
        }
      }
    }
    public async Task SendUdpMessage(Client client, string message)
    {
      await udp_handler.SendMessage(message, client); 
    }
    public async Task OnMessageReceived(int client_id, string message, bool is_Tcp)
    {
      if(!clients.TryGetValue(client_id, out Client? client)) return;
      string message_type = message.Split(":")[0];
      RequestHandler login_request = new LoginHandle();
      RequestHandler register_request = new RegisterHandle();
      RequestHandler match_request = new MatchHandle();
      RequestHandler update_request = new MoveHandle();
      RequestHandler bomb_handler = new BombHandle();
      RequestHandler powerup_handler = new PowerupHandle();
      RequestHandler map_handler  = new MapHandle();
      RequestHandler theme_request = new ThemeHandle();
      RequestHandler leader_request = new LeaderboardHandle();
      match_request.setNext(map_handler);
      update_request.setNext(bomb_handler);
      bomb_handler.setNext(powerup_handler);

      switch (message_type)
      {
        case "LOGIN":
          _=login_request.handle(client, message, is_Tcp, this);
          break;
        case "REGISTER":
          _=register_request.handle(client, message, is_Tcp, this);
          break;
        case "MATCH":
          _=match_request.handle(client, message, is_Tcp, this);
          break;
        case "UPDATE":
          _=update_request.handle(client, message, is_Tcp, this);
          break;
        case "THEME":
          _=theme_request.handle(client, message, is_Tcp, this);
          break;
        case "LEADER":
          _=leader_request.handle(client, message, is_Tcp, this);
          break;
      }
    }
  }
}