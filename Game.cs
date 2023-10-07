using static SDL2.SDL;
using static SDL2.SDL_image;
using GlmSharp;

namespace _2dGameEngine;

public class Game : IGame
{
    public nint window;
    public int windowWidth;
    public int windowHeight;
    public nint renderer;
    public bool isRunning;
    public vec2 playerPos;
    public vec2 playerVelocity;
    public const int FPS = 30;
    public const int MillisecondsPerFrame = 1000 / FPS;
    public uint MillisecondsPreviousFrame = 0;

    public Game(string message)
    {
        Console.WriteLine(message);
    }
    public void Initialize()
    {
        if (SDL_Init(SDL_INIT_VIDEO) < 0) Console.WriteLine($"Issue initilizing SDL: {SDL_GetError()}");
        window = SDL_CreateWindow("2d Game Engine", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, windowWidth, windowHeight, SDL_WindowFlags.SDL_WINDOW_BORDERLESS);
        if (window == IntPtr.Zero) Console.WriteLine($"Error creating the window: {SDL_GetError()}");
        renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        if (renderer == IntPtr.Zero) Console.WriteLine($"Error creating SDL renderer: {SDL_GetError()}");
        SDL_SetWindowFullscreen(window, (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        isRunning = true;
    }
    public void Run()
    {
        Setup();
        while(isRunning)
        {
            ProcessInput();
            Update();
            Render();
        }
    }
    public void ProcessInput()
    {
        while (SDL_PollEvent(out SDL_Event e) == 1)
        {
            switch (e.type)
            {
                case SDL_EventType.SDL_KEYDOWN:
                    if (e.key.keysym.sym == SDL_Keycode.SDLK_ESCAPE) isRunning = false;
                    break;
                case SDL_EventType.SDL_QUIT:
                    isRunning = false;
                    break;
            }
        }
    }
    public void Setup()
    {
        playerPos = new vec2 (10, 20);
        playerVelocity = new vec2(1, 0);
    }
    public void Update()
    {
        while (!SDL_TICKS_PASSED(SDL_GetTicks(), MillisecondsPreviousFrame + MillisecondsPerFrame))
        MillisecondsPreviousFrame = SDL_GetTicks();
        playerPos.x += playerVelocity.x;
        playerPos.y += playerVelocity.y;
    }
    public void Render()
    {
        if (SDL_SetRenderDrawColor(renderer, 21, 21, 21, 255) < 0) Console.WriteLine($"There was an issue with setting the render draw color. {SDL_GetError()}");
        if (SDL_RenderClear(renderer) < 0) Console.WriteLine($"There was an issue with clearing the render surface. {SDL_GetError()}");

        // Render player rect
        SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
        SDL_Rect player = new() { x = 10, y = 10, w = 20, h = 20 };
        SDL_RenderFillRect(renderer, ref player);

        // Draw a PNG texture
        IntPtr surface = IMG_Load(@"C:\Users\Jason\source\repos\2dGameEngine\Assets\Profile360x360.png");
        IntPtr texture = SDL_CreateTextureFromSurface(renderer, surface);
        SDL_FreeSurface(surface);
        SDL_Rect dstRect = new() { x = (int)playerPos.x, y = (int)playerPos.y, w = 360, h = 360 };
        SDL_RenderCopy(renderer, texture, (nint)null, ref dstRect);
        SDL_DestroyTexture(texture);

        SDL_RenderPresent(renderer);
    }
    public void Destroy()
    {
        // Clean up the resources that were created.
        SDL_DestroyRenderer(renderer);
        SDL_DestroyWindow(window);
        SDL_Quit();
    }
}
