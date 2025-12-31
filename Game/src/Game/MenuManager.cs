using src.Game.Menu.Concretes;

namespace src.Game
{
  public class MenuManager
  {
    private static readonly Lazy<MenuManager> lazy  = new Lazy<MenuManager>(() => new MenuManager());
    public static MenuManager instance
    {
      get{ return lazy.Value; }
    }
    private MenuManager(){}

    public async Task start()
    {
      var general_menu  = new MenuPage("BOMBERMAN");
      general_menu.AddComponent(new LoginItem());
      general_menu.AddComponent(new RegisterItem());
      await general_menu.ExecuteAsync();
    }

    public MenuPage buildStartMenu()
    {
      var start_menu = new MenuPage("START MENU");
      start_menu.AddComponent(new FindGameItem());
      start_menu.AddComponent(new ChooseThemeItem());
      start_menu.AddComponent(new LeaderboardItem());

      return start_menu;
    } 
  }
}