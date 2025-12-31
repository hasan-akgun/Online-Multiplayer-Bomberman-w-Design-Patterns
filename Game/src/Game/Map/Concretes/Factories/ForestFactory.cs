using src.Game.Map.Abstractions;

namespace src.Game.Map.Concretes.Factories
{
  public class ForestFactory : ThemeFactory
  {
    protected override ITheme createWall()
    {
      return new ForestWall();
    }
    protected override ITheme createUnWall()
    {
      return new ForestUnWall();
    }
  }
}
