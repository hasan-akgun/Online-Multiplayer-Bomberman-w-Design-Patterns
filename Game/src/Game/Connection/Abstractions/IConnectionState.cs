using src.Game.Connection.Concretes;

namespace src.Game.Connection.Abstractions
{
  public interface IConnectionState
  {
    Task tryConnect(ConnectionContext connection);
  }
}