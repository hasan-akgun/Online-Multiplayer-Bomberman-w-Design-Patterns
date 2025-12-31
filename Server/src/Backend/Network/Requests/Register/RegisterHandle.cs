using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Login
{
  public class RegisterHandle : RequestHandler
  {
    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      if (!client.is_loggedin)
      {
        var stream  = client.tcp_socket.GetStream();
        if(is_Tcp)
        {
          string[] parts  = message.Split(":");
          string username = parts[1];
          string password = parts[2];
          
          var player = new Player
          {
            fld_username  = username,
            fld_password  = password
          };
          try
          {
            await repository.AddPlayer(player);
            await TcpHandler.SendMessage("success:player added", client);
          }
          catch (Exception ex)
          {
            await TcpHandler.SendMessage("fail:player couldnt added", client);
            Console.WriteLine("player couldnt added " + ex);
          }
        }
      }
      else if (next != null)
      {
        _= next.handle(client, message, is_Tcp, server);
      }
    }
  }
}