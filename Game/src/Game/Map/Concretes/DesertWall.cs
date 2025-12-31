using src.Game.Bombs.Abstractions;
using src.Game.Map.Abstractions;

namespace src.Game.Map.Concretes
{
  public class DesertWall : ITheme, IObserver
  {
    public int wall_live {get; set;} = 3;
    public void printWall()
    {
      switch (wall_live)
      {
        case 3:
          Console.Write("\x1b[38;2;237;201;108m" + "▓" + "\x1b[0m");
          break;
        case 2:
          Console.Write("\x1b[38;2;237;201;108m" + "▒" + "\x1b[0m");
          break;
        case 1:
          Console.Write("\x1b[38;2;237;201;108m" + "░" + "\x1b[0m");
          break;
      }
    }
    public void takeDamage()
    {
      wall_live--;
    }
  }
}
