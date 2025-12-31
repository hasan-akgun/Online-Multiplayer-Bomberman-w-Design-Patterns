using src.Game.Menu.Abstractions;
using System;
using System.Threading.Tasks;

namespace src.Game.Menu.Concretes
{
  public class ChooseThemeItem : MenuComponent
  {
    public ChooseThemeItem() : base("Choose Theme") { }

    public override async Task ExecuteAsync()
    {
      Console.Clear();
      string status = "fail";
      while(status  == "fail")
      {
        Console.WriteLine("1. Forest");
        Console.WriteLine("2. Desert");
        Console.WriteLine("3. City");
        string choice = Console.ReadLine()!;
        string message  = $"THEME:{gameManager.player_id}:{choice}";
        string response = await networkManager.sendTcpMessage(message);
        string[] parts  = response.Split(":");
        status          = parts[0];
        if(status == "success")
        {
          gameManager.theme_choice = Int32.Parse(choice);
        }
      }
      
    }
  }
}
