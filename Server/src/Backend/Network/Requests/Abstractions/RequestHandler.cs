using src.Backend.Database.Abstractions;
using src.Backend.Database.Concretes;
using src.Backend.Entities;

namespace src.Backend.Network.Requests.Abstractions
{
  public abstract class RequestHandler
  {
    protected RequestHandler ?next;
    protected IPlayerRepository repository  = new PlayerRepository("Host=localhost;Port=5432;Database=bomberman;Username=postgres;Password=1234");
    public void setNext(RequestHandler next)
    {
      this.next = next;
    }

    public abstract Task handle(Client client, string message, bool is_Tcp, Server server);
  }
}