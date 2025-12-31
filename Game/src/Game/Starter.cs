using src.Game.Connection.Concretes;

namespace src.Game
{
  public class Starter
  {
    private ConnectionContext connection  = new ConnectionContext();
    private MenuManager menuManager       = MenuManager.instance;
    public async Task start()
    {
      await connection.tryConnect();
      await menuManager.start();
    }
  }
}
