using src.Game.Enemy.Abstractions;
using src.Game.Players;

namespace src.Game.Enemy.Concretes
{
  public class Trap : IEnemy
  {
    public int x { get; set; }
    public int y { get; set; }
    public int live { get; set; }
    private GameManager gameManager = GameManager.instance;
    private List<Player> players;

    public Trap(int x, int y)
    {
      this.x = x;
      this.y = y;
      this.live = 1; 
      players = gameManager.players;
    }

    public void move(){}

    public void checkPlayer()
    {
      foreach (var player in players)
      {
        if(x == player.x && y == player.y)
        {
          player.takeDamage();
          live--;
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
      Console.Write("M");
    }
  }
}
