namespace src.Backend.Entities
{
  public class Player
  {
    public bool available {get; set;} = false;
    public int fld_id {get; private set;}
    public string fld_username {get; set;}  = null!;
    public string fld_password {get; set;}  = null!;
    public int fld_theme {get; set;}
    public int fld_win {get; set;}
    public int fld_games {get; set;}
    public int x {get; set;}  = 0;
    public int y {get; set;}  = 0;
  }
}