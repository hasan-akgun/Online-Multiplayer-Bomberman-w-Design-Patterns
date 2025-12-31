using src.Game.Map.Renderer.Abstractions;
using src.Game.Players;

namespace src.Game.Map.Renderer.Concretes
{
  public class RenderPlayer : RendererHandler
  {
    public override void handle(Dictionary<string, object> objects, int map_x, int map_y)
    {
      var players = (List<Player>)objects["players"];
      bool isPlayer = false;
      foreach (var player in players)
      {
        if (map_x == player.x && map_y == player.y)
        {
          player.printPlayer();
          isPlayer = true;
          break;
        }
      }

      if (!isPlayer)
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