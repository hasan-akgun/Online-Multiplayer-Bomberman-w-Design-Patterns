using src.Game.Bombs.Concretes;
using src.Game.Enemy.Abstractions;
using src.Game.Map.Abstractions;
using src.Game.Map.Concretes;
using src.Game.Map.Concretes.Factories;
using src.Game.Map.Renderer;
using src.Game.Players;
using src.Game.Powerups;

namespace src.Game
{
  public class GameManager
  {
    private Random  random = new Random();
    public ITheme?[,] ?map;
    private ThemeFactory ?theme;
    public List<Bomb> bombs = new List<Bomb>();
    public List<Player> players = new List<Player>();
    public List<Powerup> powerups = new List<Powerup>();
    public List<IEnemy> enemies = new List<IEnemy>();
    private Player player_2 = null!;
    public int theme_choice {get; set;}
    public int player_id {get; set;}
    private static readonly Lazy<GameManager> lazy  = new Lazy<GameManager>(() => new GameManager());
    public static GameManager instance
    {
      get{ return lazy.Value; }
    }
    private NetworkManager networkManager = NetworkManager.instance;
    private GameManager(){}

    public bool isEmpty(int x, int y)
    {
      if(map != null)
      {
        return map[y, x] == null; 
      }
      return false;
    }

    public bool isEmptyEnemy(int x, int y)
    {
      if(map != null)
      {
        return map[y, x] == null || map[y, x] is PlayerCollision;
      }
      return false;
    }

    public void explodeWall(int x, int y)
    {
      if(map != null)
      {
        var wall  = map[y, x];
        if(wall != null && wall.wall_live == 0)
        {
          map[y, x] = null;
          createPowerup(x, y);
        }
      }
    }

    private void createPowerup(int x, int y)
    {
      if(random.Next(8) == 0)
      {
        var powerup = new Powerup(x, y);
        _=networkManager.sendUdpMessage($"{networkManager.connection_id}|UPDATE:POWERUP:{networkManager.game_id}:{x}:{y}");
        powerups.Add(powerup);
      }
    }

    public void start()
    {
      chooseTheme(theme_choice);
      var player    = new Player(networkManager.player1_x, networkManager.player1_y);
      player_2      = new Player(0, 0);
      players.Add(player);
      players.Add(player_2);
      if(map != null)
      {
        var mapRenderer = MapRenderer.create()
                                     .setMap(map)
                                     .setPlayer(players)
                                     .setBombs(bombs)
                                     .setPowerups(powerups)
                                     .setEnemies(enemies)
                                     .build();
        update(mapRenderer, player, player_2);
      }
    }
    private void update(MapRenderer mapRenderer, Player player, Player player_2)
    {
      ITheme collision = new PlayerCollision();
      _=processEvents();
      while (map != null && player.live > 0 && player_2.live > 0)
      {
        if (Console.KeyAvailable)
        {
          var key = Console.ReadKey(true).Key;
          switch (key)
          {
            case ConsoleKey.W:
              map[player.y, player.x] = null;
              player.moveUp();
              map[player.y, player.x] = collision;
              break;
            case ConsoleKey.A:
              map[player.y, player.x] = null;
              player.moveLeft();
              map[player.y, player.x] = collision;
              break;
            case ConsoleKey.S:
              map[player.y, player.x] = null;
              player.moveDown();
              map[player.y, player.x] = collision;
              break;
            case ConsoleKey.D:
              map[player.y, player.x] = null;
              player.moveRight();
              map[player.y, player.x] = collision;
              break;
            case ConsoleKey.Spacebar:
              player.placeBomb();
              break;
          }
        }
        checkPowerup();
        checkEnemies();
        _=networkManager.sendUdpMessage($"{networkManager.connection_id}|UPDATE:MOVE:{networkManager.game_id}:{player.x}:{player.y}:{player.direction.face}:{player.live}:{player.power}");
        Console.SetCursorPosition(1, 17);
        Console.Write($"Power: {player.power}, Live: {player.live}, Bomb: {player.bomb_capacity}");
        Console.SetCursorPosition(0, 1);
        mapRenderer.render();
        Thread.Sleep(10);
      }
      if(player.live <= 0)
      {
        Console.SetCursorPosition(8, 18);
        Console.WriteLine("YOU LOSE");
      }
      if(player_2.live <= 0)
      {
        Console.SetCursorPosition(8, 18);
        Console.WriteLine("YOU WIN");
      }
      Thread.Sleep(3000);
    }

    private async Task processEvents()
    {
      while (true)
      {
        try
        {
          var response   = await networkManager.getUdpMessage();
          string[] parts = response.Split(":");
          string process_type = parts[0];
          switch (process_type)
          {
            case "MOVE":
              movePlayer2(parts[1..]);
              break;
            case "BOMB":
              placeBomb2(parts[1..]);
              break;
            case "POWERUP":
              createPowerup2(parts[1..]);
              break;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("UDP ERROR: " + ex);
        }
      }
    }
    
    private void movePlayer2(string[] parts)
    {
      int x          = Int32.Parse(parts[0]);
      int y          = Int32.Parse(parts[1]);
      string face    = parts[2];
      int live       = Int32.Parse(parts[3]);
      int power      = Int32.Parse(parts[4]);
      string display = "";
      switch (face)
      {
        case "up":
          display = "\x1b[38;2;0;0;255m" + "↑" + "\x1b[0m";
          break;
        case "down":
          display = "\x1b[38;2;0;0;255m" + "↓" + "\x1b[0m";
          break;
        case "right":
          display = "\x1b[38;2;0;0;255m" + "→" + "\x1b[0m";
          break;
        case "left":
          display = "\x1b[38;2;0;0;255m" + "←" + "\x1b[0m";
          break;
      }
      player_2.x          = x;
      player_2.y          = y;
      player_2.live       = live;
      player_2.direction  = (face, display);
      player_2.power      = power;
    }
    
    private void placeBomb2(string[] parts)
    {
      int x          = Int32.Parse(parts[0]);
      int y          = Int32.Parse(parts[1]);
      int power      = Int32.Parse(parts[2]);
      var bomb       = new Bomb(power, x, y);
      bombs.Add(bomb);
    }

    private void createPowerup2(string[] parts)
    {
      int x          = Int32.Parse(parts[0]);
      int y          = Int32.Parse(parts[1]);
      var powerup    = new Powerup(x, y);
      powerups.Add(powerup);
    }

    private void checkPowerup()
    {
      foreach (var powerup in powerups)
        {
          if (powerup.isPickedUp())
          {
            powerup.activatePower();
            powerups.Remove(powerup);
            break;
          }
        }
    }

    private void checkEnemies()
    {
      foreach (var enemy in enemies)
      {
        enemy.checkPlayer();
        if (!enemy.isAlive())
        {
          enemies.Remove(enemy);
          break;
        }
      }
    }
    
    public int[,] parseMapString(string map_info, string length_0, string length_1)
    {
      var map_length_0  = Int32.Parse(length_0);
      var map_length_1  = Int32.Parse(length_1);
      var map_arr       = map_info.Split(",");
      int[,] map        = new int[map_length_0, map_length_1];
      int index         = 0;

      for (int i = 0; i < map_length_0; i++)
      {
        for (int j = 0; j < map_length_1; j++)
        {
            if (index < map_info.Length)
            {
              map[i, j] = int.Parse(map_arr[index]);
              index++;
            }
        }
      }
      return map;
    }
    private void chooseTheme(int choice)
    {
      if(theme == null)
      {
        switch (choice)
        {
          case 1:
            theme = new ForestFactory();
            break;
          case 2:
            theme = new DesertFactory();
            break;
          case 3:
            theme = new CityFactory();
            break;
          default:
            theme = new ForestFactory();
            break;
        }
        map = theme.createMap();
      }
    }
  }
}