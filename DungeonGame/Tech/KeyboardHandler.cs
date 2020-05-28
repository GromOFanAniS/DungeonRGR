using Microsoft.Xna.Framework.Input;

namespace DungeonGame
{
    public class KeyboardHandler
    {
        private static KeyboardState oldState;
        private static KeyboardState newState;
        
        public static void Update()
        {
            oldState = newState;
            newState = Keyboard.GetState();
        }

        public static bool IsPressed(Keys key)
        {
            return newState.IsKeyDown(key);
        }

        public static bool HasBeenPressed(Keys key)
        {
            return newState.IsKeyDown(key) && !oldState.IsKeyDown(key);
        }
    }
}
