using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Match
{
  public class MapHandle : RequestHandler
  {
    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      if (client.is_loggedin)
      {
        if (is_Tcp)
        {
          var game_id = Int32.Parse(message);
          var match = server.games[game_id];
          var maps  = server.maps;
          string map_string;
          int map_length_0 = Constants.MAP_LENGTH_0;
          int map_length_1 = Constants.MAP_LENGTH_1;
          if (!maps.TryGetValue(game_id, out string? map_s))
          {
            var map = createMap();
            List<int> flat_list = new List<int>();
            for (int i = 0; i < map_length_0; i++)
            {
              for (int j = 0; j < map_length_1; j++)
              {
                flat_list.Add(map[i, j]);
              }
            }
            map_string = string.Join(",", flat_list);
            maps.TryAdd(game_id, map_string);
          }
          else
          {
            map_string = map_s;
          }
          
          foreach (var (id, player) in match)
          {
            var client_p  = server.clients[id];
            await TcpHandler.SendMessage($"success:{map_string}:{map_length_0}:{map_length_1}:{player.x}:{player.y}:{game_id}", client_p);
            await repository.UpdateGames(player.fld_id);
          }
        }
      }
      else
      {
        server.DisconnectClient(client.id);
      }
    }

    private int[,] createMap()
    {
      int[,] map  = new int[Constants.MAP_LENGTH_0, Constants.MAP_LENGTH_1];
      var map_length_0  = Constants.MAP_LENGTH_0; 
      var map_length_1  = Constants.MAP_LENGTH_1;
      Random random = new Random();
      for(int i=0; i<map_length_0; i++)
      {
        for(int j=0; j<map_length_1; j++)
        {
          if(i == 0 || i == map_length_0 - 1 )
          {
            map[i,j]  = -1;
          }
          else if(j == 0 || j == 1 || j == map_length_1 - 2 || j == map_length_1 - 1)
          {
            map[i,j]  = -1;
          }
          else if(i == 10 && j == 5 || i == 10 && j == 25)
          {
            continue;
          }
          else
          {
            int posibility  = random.Next(7);
            int enemy_psb   = random.Next(150);
            int wall_live   = random.Next(1,4);        
            if (posibility == 0)
            {
              map[i,j]      = wall_live;
            }
            else if(enemy_psb == 3)
            {
              map[i, j]     = 4;
            }
            else if(enemy_psb == 5)
            {
              map[i, j]     = 5;
            }
          }
        }
      }
      return map;
    }
  }
}
