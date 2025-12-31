using src.Game.Menu.Abstractions;
using src.Game;
using System.Threading.Tasks;
using System;

namespace src.Game.Menu.Concretes
{
  public class RegisterItem : MenuComponent
  {
    public RegisterItem() : base("REGISTER") { }

    public override async Task ExecuteAsync()
    {
      Console.Clear();
      string status = "fail";
      while(status  == "fail")
      {
        Console.Write("   Username: ");
        string username = Console.ReadLine()!;
        Console.Write("   Password: ");
        string password = Console.ReadLine()!;
        string message  = $"REGISTER:{username}:{password}";
        string response = await networkManager.sendTcpMessage(message);
        string[] parts  = response.Split(":");
        status          = parts[0];
        string server_message = parts[1];
        Console.Clear();
        Console.WriteLine(server_message);
      }
    }
  }
}
