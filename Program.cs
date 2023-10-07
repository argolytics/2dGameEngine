namespace _2dGameEngine;

internal class Program
{
    static void Main()
    {
        Game game = new("Begin game");
        game.Initialize();
        game.Run();
    }
}