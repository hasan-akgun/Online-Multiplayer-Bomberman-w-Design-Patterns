using src.Game.Bombs.Abstractions;
using src.Game.Enemy.Abstractions;
using src.Game.Map.Abstractions;
using src.Game.Map.Concretes;
using src.Game.Players;

namespace src.Game.Bombs.Concretes
{
 public class Bomb : ISubject
  {
    private List<IObserver> observers = new List<IObserver>();
    public List<(int x, int y)> range {get; private set;}
    private bool is_exploded;
    private ITheme collision;
    private ITheme?[,] ?map;
    private int base_x, base_y;
    private int left_x, right_x;
    private int lower_y, upper_y;
    private GameManager gameManager;
    public Bomb(int power, int x, int y)
    {
      gameManager = GameManager.instance;
      map         = gameManager.map;
      is_exploded = false;
      base_x      = x;
      base_y      = y;
      range       = new List<(int x, int y)>{(x, y)};
      upper_y     = y - power < 0 ? 0 : y - power;
      lower_y     = y + power >= map?.GetLength(0) ? map.GetLength(0) - 1 : y + power;
      right_x     = x + power >= map?.GetLength(1) ? map.GetLength(1) - 1 : x + power;
      left_x      = x - power < 0 ? 0 : x - power;
      collision   = new ObjectCollision();
      map?[y, x]  = collision;
      _=processExplode();
    }

    public void attach(IObserver observer)
    {
      observers.Add(observer);
    }
    public void detachAll()
    {
      observers.Clear();
    }
    public void giveDamage()
    {
      foreach (var observer in observers)
        {
          observer.takeDamage();
        }
    }
    private async Task processExplode()
    {
      await Task.Delay(3000);
      scanRange();
      explode();
      detachAll();
      await Task.Delay(100);
      gameManager.bombs.Remove(this);
    }
    private void explode()
    {
      for(int y = upper_y; y <= lower_y; y++)
      {
        var wall  = map?[y, base_x];
        if(wall != null && wall.wall_live == -1)
        {
          continue;
        }
        range.Add((base_x, y));
      }
      for(int x = left_x; x <= right_x; x++)
      {
        var wall  = map?[base_y, x];
        if(wall != null && wall.wall_live == -1)
        {
          continue;
        }
        range.Add((x, base_y));
      }
      giveDamage();
      map?[base_y, base_x] = null;
      is_exploded          = true;
    }
    private void scanRange()
    {
      var players = gameManager.players;
      var enemies = gameManager.enemies;
      for(int y = upper_y; y <= lower_y; y++)
      {
        var wall  = map?[y, base_x];
        if(wall != null && wall is IObserver)
        {
          attach((IObserver)wall);
        }
        foreach(Player player in players)
        {
          if(y == player.y && base_x == player.x)
          {
            attach(player);
          }
        }
        foreach(IEnemy enemy in enemies)
        {
          if(y == enemy.y && base_x == enemy.x && enemy is IObserver)
          {
            attach((IObserver)enemy);
          }
        }
      }
      for(int x = left_x; x <= right_x; x++)
      {
        var wall  = map?[base_y, x];
        if(wall != null && wall is IObserver)
        {
          attach((IObserver)wall);
        }
        foreach(Player player in players)
        {
          if(x == player.x && base_y == player.y)
          {
            attach(player);
          }
        }
        foreach(IEnemy enemy in enemies)
        {
          if(x == enemy.x && base_y == enemy.y && enemy is IObserver)
          {
            attach((IObserver)enemy);
          }
        }
      }
    }
    public void printBomb()
    {
      if (is_exploded)
      {
        Console.Write("\x1b[38;2;255;69;0m" + "@" + "\x1b[0m");
      }
      else
      {
        Console.Write("o");
      }
    }
  } 
}