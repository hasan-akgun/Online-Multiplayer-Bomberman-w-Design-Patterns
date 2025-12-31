using src.Backend.Entities;

namespace src.Backend.Database.Abstractions
{
  public interface IPlayerRepository
  {
    Task AddPlayer(Player player);
    Task<Player?> GetPlayerByUsername(string username);
    Task<Player?> GetPlayerById(int id);
    Task UpdateTheme(int id, int theme);
    Task UpdateWin(int id);
    Task UpdateGames(int id);
    Task<IEnumerable<Player>?> GetTopPlayers();
  }
}
