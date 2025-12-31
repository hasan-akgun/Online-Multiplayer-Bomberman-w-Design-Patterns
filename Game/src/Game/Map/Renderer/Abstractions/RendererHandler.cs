namespace src.Game.Map.Renderer.Abstractions
{
  public abstract class RendererHandler
  {
    protected RendererHandler ?next;
    protected GameManager gameManager = GameManager.instance;

    public void setNext(RendererHandler next)
    {
      this.next = next;
    }

    public abstract void handle(Dictionary<string, object> objects, int map_x, int map_y);
  }
}