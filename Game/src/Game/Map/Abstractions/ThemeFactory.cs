using src.Game.Enemy.Abstractions;
using src.Game.Enemy.Concretes;

namespace src.Game.Map.Abstractions
{
  public abstract class ThemeFactory
  {
    private ITheme[,] ?map;
    private Random ?random;
    protected abstract ITheme createWall();
    protected abstract ITheme createUnWall();

    public ITheme[,] createMap()
    {
      var networkManager  = NetworkManager.instance;
      var gameManager     = GameManager.instance;
      var map_info        = networkManager.map_info;
      var map_length_0    = map_info.GetLength(0); 
      var map_length_1    = map_info.GetLength(1); 
      map                 = new ITheme[map_length_0, map_length_1];
      random              = new Random(); 
      ITheme u_wall       = createUnWall();
      for(int i=0; i<map_length_0; i++)
      {
        for(int j=0; j<map_length_1; j++)
        {
          int info = map_info[i, j];
          if(info == -1)
          {
            map[i, j] = u_wall;
          }
          else if (info > 0 && info < 4)
          {
            ITheme wall = createWall();
            wall.wall_live  = info;
            map[i, j] = wall;
          }
          else if (info == 4)
          {
            IEnemy enemy = new Monster(j, i);
            gameManager.enemies.Add(enemy);
          }
          else if (info == 5)
          {
            IEnemy enemy = new Trap(j, i);
            gameManager.enemies.Add(enemy);
          }
        }
      }
      return map;
    }
  }
}