using src.Game.Players;

namespace src.Game.Powerups
{
  public class Powerup
  {
    private Random random;
    private int type;
    public int x {get; private set;}
    public int y {get; private set;}
    private GameManager gameManager;
    private List<Player> players;
    private Player powered_player = null!;
    public Powerup(int x, int y)
    {
      gameManager = GameManager.instance;
      players     = gameManager.players;
      random      = new Random();
      this.x      = x;
      this.y      = y;
      type        = random.Next(3);
    }

    public void activatePower()
    {
      switch (type)
      {
        case 0:
          powered_player.live++;
          break;
        case 1:
          powered_player.bomb_capacity++;
          powered_player.bomb_count = powered_player.bomb_capacity;
          break;
        case 2:
          powered_player.power++;
          break;
      }
    }

    public bool isPickedUp()
    {
      foreach (var player in players)
      {
        if(x == player.x && y == player.y)
        {
          powered_player = player;
          return true;
        }
      }
      return false;
    }
    public void printPowerup()
    {
      Console.Write("?");
    }
  }
}