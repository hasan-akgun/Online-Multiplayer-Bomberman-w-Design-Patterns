using src.Game.Connection.Abstractions;

namespace src.Game.Connection.Concretes
{
  public class ConnectionContext
  {
    private IConnectionState _state;

    public ConnectionContext()
    {
      _state = new NotConnected();
    }

    public void setState(IConnectionState state)
    {
      _state = state;
    }

    public async Task tryConnect()
    {
      await _state.tryConnect(this);
    }
  }
}
