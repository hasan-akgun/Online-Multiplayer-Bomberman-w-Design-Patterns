using src.Game.Map.Abstractions;

namespace src.Game.Map.Concretes.Factories
{
  public class DesertFactory : ThemeFactory
  {
    protected override ITheme createWall()
    {
      return new DesertWall();
    }
    protected override ITheme createUnWall()
    {
      return new DesertUnWall();
    }
  }
}
