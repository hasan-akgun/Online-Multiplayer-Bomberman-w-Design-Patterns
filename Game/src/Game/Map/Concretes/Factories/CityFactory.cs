using src.Game.Map.Abstractions;

namespace src.Game.Map.Concretes.Factories
{
  public class CityFactory : ThemeFactory
  {
    protected override ITheme createWall()
    {
      return new CityWall();
    }
    protected override ITheme createUnWall()
    {
      return new CityUnWall();
    }
  }
}