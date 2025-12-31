using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Update
{
  public class BombHandle : RequestHandler
  {
    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      string[] parts = message.Split(":");
      string req_type = parts[1];
      if (client.is_loggedin && !is_Tcp)
      {
        if(req_type == "BOMB")
        {
          int game_id       = Int32.Parse(parts[2]);
          string x          = parts[3];
          string y          = parts[4];
          int power         = Int32.Parse(parts[5]);
          var match         = server.games[game_id];
          foreach (var (id, player) in match)
          {
            if(id != client.id)
            {
              var client_p  = server.clients[id];
              await server.SendUdpMessage(client_p, $"BOMB:{x}:{y}:{power}");
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
}