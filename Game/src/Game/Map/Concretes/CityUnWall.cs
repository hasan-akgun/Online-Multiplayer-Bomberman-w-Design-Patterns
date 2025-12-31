using src.Game.Map.Abstractions;

namespace src.Game.Map.Concretes
{
  public class CityUnWall : ITheme
  {
    public int wall_live {get; set;} = -1;
    public void printWall()
    {
      Console.Write("\x1b[38;2;169;169;169m" + "█" + "\x1b[0m");
    }
  }
}