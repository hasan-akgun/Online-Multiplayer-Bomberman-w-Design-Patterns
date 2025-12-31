using System.Data;
using Dapper;
using Npgsql;
using src.Backend.Database.Abstractions;
using src.Backend.Entities;

namespace src.Backend.Database.Concretes
{
  public class PlayerRepository : IPlayerRepository
  {
    private readonly string connection_string;

    public PlayerRepository(string connection_string)
    {
      this.connection_string  = connection_string;
    }

    private IDbConnection createConnection()
    {
      return new NpgsqlConnection(connection_string);
    }
    public async Task AddPlayer(Player player)
    {
      var sql = @"INSERT INTO tbl_players (fld_username, fld_password) 
                  VALUES (@fld_username, @fld_password)";

      using (var connection = createConnection())
      {
          await connection.ExecuteAsync(sql, player);
      }
    }

    public async Task<Player?> GetPlayerByUsername(string username)
    {
      var sql = "SELECT * FROM tbl_players WHERE fld_username = @fld_username";

      using (var connection = createConnection())
      {
        return await connection.QuerySingleOrDefaultAsync<Player>(sql, new { fld_username = username });
      }
    }

    public async Task<Player?> GetPlayerById(int id)
    {
      var sql = "SELECT * FROM tbl_players WHERE fld_id = @fld_id";

      using (var connection = createConnection())
      {
          return await connection.QuerySingleOrDefaultAsync<Player>(sql, new { fld_id = id });
      }
    }
    
    public async Task UpdateTheme(int id, int theme)
    {
      var sql = @"UPDATE tbl_players
                  SET fld_theme = @fld_theme
                  WHERE fld_id = @fld_id";

      using (var connection = createConnection())
      {
          await connection.ExecuteAsync(sql, new {fld_theme = theme, fld_id = id});
      }
    }

    public async Task UpdateWin(int id)
    {
      var sql = @"UPDATE tbl_players
                  SET fld_win = fld_win + 1
                  WHERE fld_id = @fld_id";

      using (var connection = createConnection())
      {
          await connection.ExecuteAsync(sql, new {fld_id = id});
      }
    }
    public async Task UpdateGames(int id)
    {
      var sql = @"UPDATE tbl_players
                  SET fld_games = fld_games + 1
                  WHERE fld_id = @fld_id";

      using (var connection = createConnection())
      {
          await connection.ExecuteAsync(sql, new {fld_id = id});
      }
    }

    public async Task<IEnumerable<Player>?> GetTopPlayers()
    {
      var sql = @"SELECT p.fld_username, p.fld_win, p.fld_games
                  FROM tbl_players p
                  ORDER BY fld_win DESC, fld_games DESC
                  LIMIT 10";

      using (var connection = createConnection())
      {
        return await connection.QueryAsync<Player>(sql);
      }
    }

  }
}