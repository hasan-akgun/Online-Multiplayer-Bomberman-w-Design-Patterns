using src.Game.Bombs.Concretes;
using src.Game.Map.Renderer.Abstractions;

namespace src.Game.Map.Renderer.Concretes
{
  public class RenderBomb : RendererHandler
  {
    public override void handle(Dictionary<string, object> objects, int map_x, int map_y)
    {
      var bombs       = (List<Bomb>)objects["bombs"];
      bool isInRange  = false;
      int bomb_index  = 0;
      foreach(Bomb bomb in bombs)
      {
        isInRange = bomb.range.Contains((map_x, map_y));
        if (isInRange)
        {
          break;
        }
        bomb_index++;
      }
      if (isInRange)
      {
        bombs[bomb_index].printBomb();
      }
      else if(next != null)
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
