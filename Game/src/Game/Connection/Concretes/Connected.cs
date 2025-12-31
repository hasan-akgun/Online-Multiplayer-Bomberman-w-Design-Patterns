using src.Game.Connection.Abstractions;

namespace src.Game.Connection.Concretes
{
  public class Connected : IConnectionState
  {
    public async Task tryConnect(ConnectionContext connection)
    {
      Console.WriteLine("Connected to the server.");
      await Task.Delay(1000);
    }
  }
}
