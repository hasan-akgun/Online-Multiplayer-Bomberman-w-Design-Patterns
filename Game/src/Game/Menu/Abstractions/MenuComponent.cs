namespace src.Game.Menu.Abstractions
{
  public abstract class MenuComponent
  {
    public string Title { get; protected set; }
    protected GameManager gameManager = GameManager.instance;
    protected NetworkManager networkManager = NetworkManager.instance;
    protected MenuManager menuManager = MenuManager.instance;

    public MenuComponent(string title)
    {
      Title = title;
    }

    public abstract Task ExecuteAsync();
  }
}