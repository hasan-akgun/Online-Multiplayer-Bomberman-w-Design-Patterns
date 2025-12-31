using src.Game.Menu.Abstractions;
using System;
using System.Threading.Tasks;

namespace src.Game.Menu.Concretes
{
  public class LeaderboardItem : MenuComponent
  {
    public LeaderboardItem() : base("Leaderboard") { }

    public override async Task ExecuteAsync()
    {
      Console.Clear();
      string status = "fail";
      while(status  == "fail")
      {
        string message  = $"LEADER";
        string response = await networkManager.sendTcpMessage(message);
        string[] parts  = response.Split(":");
        status          = parts[0];
        if(status == "success")
        {
          int rank = 1;
          var players  = parts[1..];
          Console.WriteLine($"{"RANK"}. {"PLAYER",-5} {"WIN",2} {"GAMES",2}");
          foreach (var player in players)
          {
            var data = player.Split("-");
            var username = data[0];
            var win      = data[1];
            var games    = data[2];
            Console.WriteLine($"{rank}. {username,-10} {win,2} {games,2}");
          }
          Console.ReadKey();
        }
      }
    }
  }
}
