using System.Net.Sockets;
using System.Text;
using src.Backend.Entities;

namespace src.Backend.Network
{
  public class UdpHandler
  {
    private UdpClient udp_listener;
    private Server server;

    public UdpHandler(Server server)
    {
        this.server = server;
        udp_listener = new UdpClient(Constants.PORT);
    }

    public void Start()
    {
        Console.WriteLine($"[UDP] Listening: {Constants.PORT}");
        _ = ReceiveLoop();
    }

    private async Task ReceiveLoop()
    {
      while (true)
      {
        try
        {
          var result = await udp_listener.ReceiveAsync();
          string msg = Encoding.UTF8.GetString(result.Buffer);
          string[] parts = msg.Split('|');
          
          if (parts.Length >= 2 && int.TryParse(parts[0], out int client_id))
          {
            server.UpdateClientUdpEndPoint(client_id, result.RemoteEndPoint);
            _= server.OnMessageReceived(client_id, parts[1], is_Tcp: false);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"[UDP Error] {ex.Message}");
        }
      }
    }

    public async Task SendMessage(string message, Client client)
    {
      try
      {
        byte[] data = Encoding.UTF8.GetBytes(message);
        if (client.endpoint != null) 
        {
          await udp_listener.SendAsync(data, data.Length, client.udp_endpoint);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"[UDP Send Error] Message could not be sent to {client.id}: {ex.Message}");
      }
    }
  }
}