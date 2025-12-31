using src.Game.Map.Abstractions;
using src.Game.Map.Renderer.Abstractions;

namespace src.Game.Map.Renderer.Concretes
{
  public class RenderWall : RendererHandler
  {
    public override void handle(Dictionary<string, object> objects, int map_x, int map_y)
    {
      gameManager.explodeWall(map_x, map_y);
      var map   = (ITheme[,])objects["map"];
      var wall  = map[map_y, map_x];
      if (wall != null)
      {
        wall.printWall();
      }
      else if (next != null)
      {
        next.handle(objects, map_x, map_y);
      }
      else
      {
        Console.Write(" ");
      }
    }
  }
}
