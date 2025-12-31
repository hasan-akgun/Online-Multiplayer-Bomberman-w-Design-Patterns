using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Update
{
  public class MoveHandle : RequestHandler
  {
    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      string[] parts = message.Split(":");
      string req_type = parts[1];
      if (client.is_loggedin && !is_Tcp)
      {
        if(req_type == "MOVE")
        {
          int game_id       = Int32.Parse(parts[2]);
          string x          = parts[3];
          string y          = parts[4];
          string face       = parts[5];
          int live          = Int32.Parse(parts[6]);
          int power         = Int32.Parse(parts[7]);
          var match         = server.games[game_id];
          string send_ms    = $"MOVE:{x}:{y}:{face}:{live}:{power}";
          bool is_alive     = live <= 0 ? false : true;
          foreach (var (id, player) in match)
          {
            if(id != client.id)
            {
              var client_p  = server.clients[id];
              await server.SendUdpMessage(client_p, send_ms);
              if (!is_alive)
              {
                await repository.UpdateWin(player.fld_id);
              }
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