using src.Game.Map.Abstractions;

namespace src.Game.Map.Concretes
{
  public class ForestUnWall : ITheme
  {
    public int wall_live {get; set;} = -1;
    public void printWall()
    {
      Console.Write("\x1b[38;2;139;69;19m" + "█" + "\x1b[0m");
    }
  }
}
