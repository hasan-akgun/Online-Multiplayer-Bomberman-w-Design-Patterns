using src.Game.Map.Abstractions;

namespace src.Game.Map.Concretes
{
  public class PlayerCollision : ITheme
  {
    public int wall_live {get; set;} = -2;
    public void printWall()
    {
      Console.Write(" ");
    }
  }
}
