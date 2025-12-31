using src.Game.Bombs.Concretes;
using src.Game.Enemy.Abstractions;
using src.Game.Map.Abstractions;
using src.Game.Map.Renderer.Abstractions;
using src.Game.Map.Renderer.Concretes;
using src.Game.Players;
using src.Game.Powerups;

namespace src.Game.Map.Renderer
{
  public class MapRenderer
  {
    public Dictionary<string, object> objects {get; private set;}
    private RendererHandler renderWall;
    private RendererHandler renderPlayer;
    private RendererHandler renderBomb;
    private RendererHandler renderPowerup;
    private RendererHandler renderEnemy;

    private MapRenderer()
    {
      objects       = new Dictionary<string, object>();
      renderWall    = new RenderWall();
      renderPlayer  = new RenderPlayer();
      renderBomb    = new RenderBomb();
      renderPowerup = new RenderPowerup();
      renderEnemy   = new RenderEnemy();
      renderBomb.setNext(renderEnemy);
      renderEnemy.setNext(renderPlayer);
      renderPlayer.setNext(renderPowerup);
      renderPowerup.setNext(renderWall);
    }

    public static MapRendererBuilder create()
    {
      return new MapRendererBuilder();
    }
    public void render()
    {
      var map = (ITheme?[,])objects["map"];
      for(int y=0; y<map.GetLength(0); y++)
      {
        Console.WriteLine();
        for(int x=0; x<map.GetLength(1); x++)
        {
          renderBomb.handle(objects, x, y);
        }
      }
    }

    public class MapRendererBuilder
    {
      private MapRenderer mapRenderer;

      public MapRendererBuilder()
      {
        mapRenderer = new MapRenderer();
      }

      public MapRendererBuilder setMap(ITheme?[,] map)
      {
        mapRenderer.objects.Add("map", map);
        return this;
      }
      public MapRendererBuilder setPlayer(List<Player> players)
      {
        mapRenderer.objects.Add("players", players);
        return this;
      }

      public MapRendererBuilder setBombs(List<Bomb> bombs)
      {
        mapRenderer.objects.Add("bombs", bombs);
        return this;
      }
      public MapRendererBuilder setPowerups(List<Powerup> powerups)
      {
        mapRenderer.objects.Add("powerups", powerups);
        return this;
      }
      public MapRendererBuilder setEnemies(List<IEnemy> enemies)
      {
        mapRenderer.objects.Add("enemies", enemies);
        return this;
      }

      public MapRenderer build()
      {
        return mapRenderer;
      }
    }
  }
}