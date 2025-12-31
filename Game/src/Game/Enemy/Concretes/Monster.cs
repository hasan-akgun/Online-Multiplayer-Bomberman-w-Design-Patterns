using src.Game.Bombs.Abstractions;
using src.Game.Enemy.Abstractions;
using src.Game.Map.Abstractions;
using src.Game.Map.Concretes;
using src.Game.Players;

namespace src.Game.Enemy.Concretes
{
  public class Monster : IEnemy, IObserver
  {
    public int x { get; set; }
    public int y { get; set; }
    public int live { get; set; }
    private GameManager gameManager = GameManager.instance;
    private ITheme collision;
    private ITheme?[,] ?map;
    private List<Player> players;
    private Player locked_player  = null!;
    
    public Monster(int x, int y)
    {
      map = gameManager.map;
      this.x = x;
      this.y = y;
      this.live = 2;
      players = gameManager.players;
      collision   = new ObjectCollision();
      map?[y, x]  = collision;
      _= processMove();
    }

    public async Task processMove()
    {
      while (isAlive())
      {
        await Task.Delay(1500);
        lockPlayer();
        map?[y, x]  = null;
        move();
        map?[y, x]  = collision;
      }
      map?[y, x]  = null;
    }
    public void move()
    {
      int target_x  = locked_player.x - x;
      int target_y  = locked_player.y - y;
      if(target_x > 0 && x != locked_player.x && gameManager.isEmptyEnemy(x+1, y))
      { 
        x++;
        return;
      }
      else if(target_x < 0 && x != locked_player.x && gameManager.isEmptyEnemy(x-1, y))
      {
        x--;
        return;
      }

      if(target_y > 0 && y != locked_player.y && gameManager.isEmptyEnemy(x, y+1))
      {
        y++;
        return;
      }
      else if(target_y < 0 && y != locked_player.y && gameManager.isEmptyEnemy(x, y-1))
      {
        y--;
        return;
      }
    }

    public void takeDamage()
    {
      live--;
    }
    private void lockPlayer()
    {
      int total_distance = 99;
      foreach (var player in players)
      {
        int distance_x     = x - player.x < 0 ? (x - player.x) * -1 : x - player.x;
        int distance_y     = y - player.y < 0 ? (y - player.y) * -1 : y - player.y;   
        int temp_distance  = distance_x + distance_y;
        if(temp_distance < total_distance)
        {
          total_distance = temp_distance;
          locked_player  = player;
        }
      }
    }
    public void checkPlayer()
    {
      foreach (var player in players)
      {
        if(x == player.x && y == player.y)
        {
          player.live = 0;
          break;
        }
      }
    }
    public bool isAlive()
    {
      return live > 0 ? true : false;
    }
    public void printEnemy()
    {
      Console.Write("E");
    }
  }
}
