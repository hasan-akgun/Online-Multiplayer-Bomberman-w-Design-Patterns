using src.Backend.Entities;
using src.Backend.Network.Requests.Abstractions;

namespace src.Backend.Network.Requests.Leaderboard
{
  public class LeaderboardHandle : RequestHandler
  {
    public override async Task handle(Client client, string message, bool is_Tcp, Server server)
    {
      if (client.is_loggedin && is_Tcp)
      {
        var players = await repository.GetTopPlayers();
        var player_string = players!.Select(p => $"{p.fld_username}-{p.fld_win}-{p.fld_games}");
        var send_ms = string.Join(":", player_string);
        await TcpHandler.SendMessage($"success:{send_ms}", client);
      }
      else if (next != null)
      {
        _= next.handle(client, message, is_Tcp, server);
      }
    }
  }
}
