using System.Net.Sockets;
using System.Net;
using System.Text;

namespace src.Game
{
  public class NetworkManager
  {
    private static readonly Lazy<NetworkManager> lazy  = new Lazy<NetworkManager>(() => new NetworkManager());
    public static NetworkManager instance
    {
      get{ return lazy.Value; }
    }

    public int[,] map_info {get; set;}  = null!;
    public int connection_id {get; set;}
    public int game_id {get; set;}
    public int player1_x {get; set;}
    public int player1_y {get; set;}
    private NetworkStream stream = null!;
    private UdpClient udpClient = null!;
    private NetworkManager(){}

    public async Task<bool> connectServer()
    {
      try
      {
        TcpClient tcpClient = new TcpClient();
        await tcpClient.ConnectAsync("127.0.0.1", 15000);
        stream  = tcpClient.GetStream();
        udpClient = new UdpClient();
        udpClient.Connect("127.0.0.1", 15000);
        return true;
      }
      catch (Exception)
      {
        Console.Clear();
        return false;
      }
    }

    public async Task<string> sendTcpMessage(string message)
    {
      byte[] message_data = Encoding.UTF8.GetBytes(message);
      await stream.WriteAsync(message_data, 0, message_data.Length);

      byte[] buffer = new byte[4096];
      int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
      string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
      return response;
    }
    public async Task sendUdpMessage(string message)
    {
      byte[] message_data = Encoding.UTF8.GetBytes(message);
      await udpClient.SendAsync(message_data, message_data.Length);
    }

    public async Task<string> getUdpMessage()
    {
      var result = await udpClient.ReceiveAsync();
      string response = Encoding.UTF8.GetString(result.Buffer);
      return response;
    }


  }
}
