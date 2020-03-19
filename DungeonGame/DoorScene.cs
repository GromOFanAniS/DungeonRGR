using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class DoorScene
    {
        Texture2D closedDoorTexture;
        Texture2D openedDoorTexture;

        private List<Door> _doors = new List<Door>();
        public bool DoNewGenerate { get; set; }

        public void Generate()
        {
            if (DoNewGenerate)
            {
                _doors.Add(new Door(100, 100, closedDoorTexture, openedDoorTexture));
                _doors.Add(new Door(200, 100, closedDoorTexture, openedDoorTexture));
            }
        }

        public void Load(ContentManager content)
        {
            closedDoorTexture = content.Load<Texture2D>("Door/DoorClosed");
            openedDoorTexture = content.Load<Texture2D>("Door/DoorOpened");
        }

        public void Update(MouseState mouseState)
        {
            foreach (var door in _doors)
            {
                door.Update(mouseState);
            }
        }

        public void Draw(SpriteBatch s)
        {
            foreach (var door in _doors)
                door.Draw(s);
        }
    }
}
