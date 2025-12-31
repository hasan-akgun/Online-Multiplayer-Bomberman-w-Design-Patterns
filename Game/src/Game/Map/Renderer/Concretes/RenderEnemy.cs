using src.Game.Enemy.Abstractions;
using src.Game.Map.Renderer.Abstractions;

namespace src.Game.Map.Renderer.Concretes
{
  public class RenderEnemy : RendererHandler
  {
    public override void handle(Dictionary<string, object> objects, int map_x, int map_y)
    {
      var enemies = (List<IEnemy>)objects["enemies"];
      bool isEnemy = false;
      foreach (var enemy in enemies)
      {
        if (map_x == enemy.x && map_y == enemy.y)
        {
          enemy.printEnemy();
          isEnemy = true;
          break;
        }
      }

      if (!isEnemy)
      {
        if (next != null)
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
}
