using System.Collections.Concurrent;
using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Match
{
  public class MatchHandle : RequestHandler
  {
    private static readonly object _lock = new object();

    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      if (client.is_loggedin)
      {
        if (is_Tcp)
        {
          var games   = server.games;
          var players = server.players;
          var player  = players[client.id];
          bool is_found = false;
          player.available  = true;
          while (!is_found)
          {
            if (!player.available) return;

            foreach (var (key, value) in players)
            {
              if(key != client.id && value.available)
              {
                lock (_lock)
                {
                  if (player.available && value.available)
                  {
                    ConcurrentDictionary<int, Player> match  = new ConcurrentDictionary<int, Player>();
                    
                    value.x = 25;
                    value.y = 10;
                    value.available = false;
                    match.TryAdd(key, value);

                    player.x = 5;
                    player.y = 10;
                    player.available = false;
                    match.TryAdd(client.id, player);

                    int game_id = games.Count + 1;
                    while (games.ContainsKey(game_id))
                    {
                      game_id++;
                    }
                    is_found  = true;
                    games.TryAdd(game_id, match);
                    if (next != null)
                    {
                      string game_id_message  = game_id.ToString();
                      _ = next.handle(client, game_id_message, is_Tcp, server);
                    }
                  }
                }
              }
              if (is_found) break;
            }
            if (!is_found) await Task.Delay(100);
          }
        }
      }
      else
      {
        server.DisconnectClient(client.id);
      }
    }
  }
}
