using src.Game.Menu.Abstractions;

namespace src.Game.Menu.Concretes
{
  public class MenuPage : MenuComponent
  {
    private readonly List<MenuComponent> _components = new List<MenuComponent>();

    public MenuPage(string title) : base(title) { }

    public void AddComponent(MenuComponent component)
    {
      _components.Add(component);
    }

    public override async Task ExecuteAsync()
    {
      while (true)
      {
        Console.Clear();
        Console.WriteLine($"--- {Title} ---");
        for (int i = 0; i < _components.Count; i++)
        {
          Console.WriteLine($"{i + 1}. {_components[i].Title}");
        }
        Console.WriteLine("-----------------");
        Console.Write("Enter your choice: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _components.Count)
        {
          await _components[choice - 1].ExecuteAsync();
        }
        else
        {
          Console.WriteLine("Invalid choice. Press any key to try again.");
          Console.ReadKey();
        }
      }
    }
  }
}
