using src.Game.Connection.Abstractions;
using System;

namespace src.Game.Connection.Concretes
{
  public class NotConnected : IConnectionState
  {
    private NetworkManager networkManager = NetworkManager.instance;
    public async Task tryConnect(ConnectionContext connection)
    {
      bool is_connected = false;
      while (!is_connected)
      {
        Console.WriteLine("Attempting to connect to the server...");
        is_connected = await networkManager.connectServer();
        await Task.Delay(100);
      }
      connection.setState(new Connected());
      await connection.tryConnect();
    }
  }
}
