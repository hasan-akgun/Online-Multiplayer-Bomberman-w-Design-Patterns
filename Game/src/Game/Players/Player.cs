using src.Game.Bombs.Abstractions;
using src.Game.Bombs.Concretes;

namespace src.Game.Players
{
  public class Player : IObserver
  {
    public int x { get; set; }
    public int y { get; set; }
    public int live {get; set;}
    public int power {get; set;}
    public int bomb_count {get; set;}
    public int bomb_capacity {get; set;}
    public (string face, string display) direction {get; set;}
    private GameManager gameManager;
    private NetworkManager networkManager; 
    public Player(int x, int y)
    {
      gameManager = GameManager.instance;
      networkManager = NetworkManager.instance;
      this.x        = x;
      this.y        = y;
      live          = 3;
      power         = 1;
      bomb_count    = 3;
      bomb_capacity = 3;
      direction     = ("up", "↑");
    }
    
    public void moveUp()
    {
      if(gameManager.isEmpty(x, y - 1) && direction.face == "up")
      {
        y = y - 1;
      }
      direction = ("up", "↑");    
    }

    public void moveDown()
    {
      if(gameManager.isEmpty(x, y + 1) && direction.face == "down")
      {
        y = y + 1;
      }
      direction = ("down", "↓");
    }

    public void moveLeft()
    {
      if(gameManager.isEmpty(x - 1, y) && direction.face == "left")
      {
        x = x - 1;
      }
      direction = ("left", "←");
    }

    public void moveRight()
    {
      if(gameManager.isEmpty(x + 1, y) && direction.face == "right")
      {
        x = x + 1;
      }
      direction = ("right", "→");
    }

    public void placeBomb()
    {
      if(bomb_count > 0)
      {
        Bomb bomb;
        var bombList = gameManager.bombs;
        switch (direction.face)
        {
          case "up":
            if(gameManager.isEmpty(x, y - 1))
            {
              bomb = new Bomb(power, x, y-1);
              bombList.Add(bomb);
              _=networkManager.sendUdpMessage($"{networkManager.connection_id}|UPDATE:BOMB:{networkManager.game_id}:{x}:{y-1}:{power}");
              bomb_count--;
              _=reloadBomb();
            }
            break;
          case "down":
            if(gameManager.isEmpty(x, y + 1))
            {
              bomb = new Bomb(power, x, y+1);
              bombList.Add(bomb);
              _=networkManager.sendUdpMessage($"{networkManager.connection_id}|UPDATE:BOMB:{networkManager.game_id}:{x}:{y+1}:{power}");
              bomb_count--;
              _=reloadBomb();
            }
            break;
          case "left":
            if(gameManager.isEmpty(x - 1, y))
            {
              bomb = new Bomb(power, x-1, y);
              bombList.Add(bomb);
              _=networkManager.sendUdpMessage($"{networkManager.connection_id}|UPDATE:BOMB:{networkManager.game_id}:{x-1}:{y}:{power}");
              bomb_count--;
              _=reloadBomb();
            }
            break;
          case "right":
            if(gameManager.isEmpty(x + 1, y))
            {
              bomb = new Bomb(power, x+1, y);
              bombList.Add(bomb);
              _=networkManager.sendUdpMessage($"{networkManager.connection_id}|UPDATE:BOMB:{networkManager.game_id}:{x+1}:{y}:{power}");
              bomb_count--;
              _=reloadBomb();
            }
            break;
        }
      }
    }

    public async Task reloadBomb()
    {
      if(bomb_count < bomb_capacity)
      {
        int delay = 2000 * (bomb_capacity - bomb_count);
        await Task.Delay(delay);
        bomb_count++;
      }
    }

    public void printPlayer()
    {
      Console.Write(direction.display);
    }

    public void takeDamage()
    {
      live--;
    }
  }
}