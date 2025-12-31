using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Login
{
  public class LoginHandle : RequestHandler
  {
    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      if (!client.is_loggedin)
      {
        if(is_Tcp)
        {
          string[] parts  = message.Split(":");
          string username = parts[1];
          string password = parts[2];
          try
          {
            var player      = await repository.GetPlayerByUsername(username);
            if(player == null)
            {
              await TcpHandler.SendMessage("fail:Player Not Found!", client);
              return;
            }

            if(player.fld_password  == password)
            {
              client.is_loggedin  = true;
              await TcpHandler.SendMessage($"success:{client.id}:{player.fld_theme}:{player.fld_id}", client);
              server.AddNewPlayer(client, player);
            }
            else
            {
              await TcpHandler.SendMessage("fail:Incorrect Password", client);
            }
          }
          catch (Exception ex)
          {
            Console.WriteLine("error occured at login " + ex);
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