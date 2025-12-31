using System.Net;
using System.Net.Sockets;
using System.Text;
using src.Backend.Entities;

namespace src.Backend.Network
{
  public class TcpHandler
  {
    private TcpListener tcp_listener;
    private Server server;
    public TcpHandler(Server server)
    {
      this.server   = server;
      tcp_listener  = new TcpListener(IPAddress.Any, Constants.PORT);
    }

    public void Start()
    {
      Console.WriteLine($"[TCP] Listening: {Constants.PORT}");
      tcp_listener.Start();
      _= AcceptLoop();
    }

    private async Task AcceptLoop()
    {
      while (true)
      {
        var tcp_client  = await tcp_listener.AcceptTcpClientAsync();
        server.ConnectNewClient(tcp_client);
      }
    }

    public async Task HandleClientComm(Client client)
    {
      NetworkStream stream = client.tcp_socket.GetStream();
      byte[] buffer = new byte[Constants.BUFFER_SIZE];

      try
      {
        while (client.tcp_socket.Connected)
        {
          int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
          if (byteCount == 0) break;

          string msg = Encoding.UTF8.GetString(buffer, 0, byteCount);
          _= server.OnMessageReceived(client.id, msg, is_Tcp: true);
        }
      }
      catch(Exception ex)
      {
        Console.WriteLine($"[TCP Error] {ex.Message}");
      }
      finally
      {
        server.DisconnectClient(client.id);
      }
    }

    public static async Task SendMessage(string message, Client client)
    {
      try
      {
        NetworkStream stream = client.tcp_socket.GetStream();
        byte[] data = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(data, 0, data.Length);
      }
      catch (Exception ex)
      {        
        Console.WriteLine($"[ERROR] TCP message couldnt send: {ex.Message}");
        return; 
      }
    }
  }
}