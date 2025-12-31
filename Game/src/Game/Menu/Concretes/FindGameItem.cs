using src.Game.Menu.Abstractions;
using System;
using System.Threading.Tasks;

namespace src.Game.Menu.Concretes
{
  public class FindGameItem : MenuComponent
  {
    public FindGameItem() : base("Start Game") { }

    public override async Task ExecuteAsync()
    {
      Console.Clear();
      string status = "fail";
      while(status == "fail")
      {
        string message  = $"MATCH";
        string response = await networkManager.sendTcpMessage(message);
        string[] parts  = response.Split(":");
        status   = parts[0];
        if(status == "success")
        {
          string map_info = parts[1];
          string map_length_0 = parts[2];
          string map_length_1 = parts[3];
          int position_x   = Int32.Parse(parts[4]);
          int position_y   = Int32.Parse(parts[5]);
          int game_id      = Int32.Parse(parts[6]);
          networkManager.player1_x = position_x;
          networkManager.player1_y = position_y;
          networkManager.game_id   = game_id;
          networkManager.map_info  = gameManager.parseMapString(map_info, map_length_0, map_length_1);
          gameManager.start();
        } 
      }
    }
  }
}
