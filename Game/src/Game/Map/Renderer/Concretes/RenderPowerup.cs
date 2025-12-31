using src.Game.Map.Renderer.Abstractions;
using src.Game.Powerups;

namespace src.Game.Map.Renderer.Concretes
{
  public class RenderPowerup : RendererHandler
  {
    public override void handle(Dictionary<string, object> objects, int map_x, int map_y)
    {
      var powerups = (List<Powerup>)objects["powerups"];
      bool isPowerup = false;
      foreach (var powerup in powerups)
      {
        if (map_x == powerup.x && map_y == powerup.y)
        {
          powerup.printPowerup();
          isPowerup = true;
          break;
        }
      }

      if (!isPowerup)
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
