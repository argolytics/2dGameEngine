namespace _2dGameEngine;

public interface IGame
{
    public void Initialize();
    public void Run();
    public void ProcessInput();
    public void Update();
    public void Render();
    public void Destroy();
}