using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Theme
{
  public class ThemeHandle : RequestHandler
  {
    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      if (client.is_loggedin && is_Tcp)
      {
        
        string[] parts  = message.Split(":");
        int id    = Int32.Parse(parts[1]);
        int theme = Int32.Parse(parts[2]);
        try
        {
          await repository.UpdateTheme(id, theme);
          await TcpHandler.SendMessage("success:theme updated", client);
        }
        catch (Exception ex)
        {
          await TcpHandler.SendMessage("fail:theme couldnt updated", client);
          Console.WriteLine("player couldnt added " + ex);
        }
      }
    }
  }
}