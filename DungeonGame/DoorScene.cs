using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class DoorScene
    {
        private Texture2D closedDoorTexture;
        private Texture2D openedDoorTexture;

        private List<Door> _doors = new List<Door>();
        public bool DoNewGenerate { get; private set; } = true;

        public void Generate()
        {
            DoNewGenerate = true;
            if (DoNewGenerate)
            {
                DoNewGenerate = !DoNewGenerate;
                _doors[0] = new Door(closedDoorTexture.Width / 2 + 50, closedDoorTexture.Height / 2, closedDoorTexture, openedDoorTexture);
                _doors[1] = new Door(closedDoorTexture.Width * 3 - 50, closedDoorTexture.Height / 2, closedDoorTexture, openedDoorTexture);
            }
        }

        public void Load(ContentManager content)
        {
            closedDoorTexture = content.Load<Texture2D>("Door/DoorClosed");
            openedDoorTexture = content.Load<Texture2D>("Door/DoorOpened");

            _doors.Add(new Door(closedDoorTexture.Width / 2 + 50, closedDoorTexture.Height / 2, closedDoorTexture, openedDoorTexture));
            _doors.Add(new Door(closedDoorTexture.Width * 3 - 50, closedDoorTexture.Height / 2, closedDoorTexture, openedDoorTexture));
        }

        public void Update()
        {
            foreach (var door in _doors)
            {
                door.Update();
            }
        }

        public void Draw(SpriteBatch s)
        {
            foreach (var door in _doors)
                door.Draw(s);
        }
    }
}
