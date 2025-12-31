using System.Net;
using System.Net.Sockets;

namespace src.Backend.Entities
{
  public class Client
  {
    public int id { get; private set;}
    public string? username { get; set; }
    public TcpClient tcp_socket { get; private set; }
    public IPEndPoint endpoint { get; private set; }
    public IPEndPoint udp_endpoint {get; set;} = null!;
    public bool is_loggedin {get; set;} = false;

    public Client(int client_id, TcpClient socket)
    {
        id          = client_id;
        tcp_socket  = socket;
        endpoint    = (IPEndPoint)socket.Client.RemoteEndPoint!;
    }

    public void Disconnect()
    {
        tcp_socket?.Close();
    }
  }
}