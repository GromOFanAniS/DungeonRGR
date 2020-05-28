using System;

namespace DungeonGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameClass())
                game.Run();
        }
    }
}
