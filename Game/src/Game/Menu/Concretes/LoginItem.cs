using src.Game.Menu.Abstractions;


namespace src.Game.Menu.Concretes
{
  public class LoginItem : MenuComponent
  {
    public LoginItem() : base("LOGIN") { }

    public override async Task ExecuteAsync()
    {
      Console.Clear();
      string status = "fail";
      while(status  == "fail")
      {
        Console.Write("Username: ");
        string username = Console.ReadLine()!;
        Console.Write("Password: ");
        string password = Console.ReadLine()!;
        string message  = $"LOGIN:{username}:{password}";
        string response = await networkManager.sendTcpMessage(message);
        string[] parts  = response.Split(":");
        status          = parts[0];
        if(status == "success")
        {
          int connection_id  = Int32.Parse(parts[1]);
          int theme_choice   = Int32.Parse(parts[2]);
          int player_id      = Int32.Parse(parts[3]);
          networkManager.connection_id = connection_id;
          gameManager.theme_choice     = theme_choice;
          gameManager.player_id        = player_id;
          Console.Clear();
          var start_menu = menuManager.buildStartMenu();
          await start_menu.ExecuteAsync();
        }
        else
        {
          string error  = parts[1];
          Console.Clear();
          Console.WriteLine(error);
        }
      }
    }
  }
}
