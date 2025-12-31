namespace src.Game.Bombs.Abstractions
{
  public interface ISubject
  {
    void attach(IObserver observer);
    void detachAll();
    void giveDamage();
  }
}