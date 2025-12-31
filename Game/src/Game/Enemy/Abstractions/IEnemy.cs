namespace src.Game.Enemy.Abstractions
{
  public interface IEnemy
  {
    int x { get; set; }
    int y { get; set; }
    int live {get; set;}
    void move();
    void checkPlayer();
    bool isAlive();
    void printEnemy();
  }
}
