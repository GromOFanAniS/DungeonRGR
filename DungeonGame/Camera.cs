using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Camera
    {
        private readonly Viewport _viewport;
        private float _zoom;
        private Vector2 _position;
        private Vector2 _origin;

        public Vector2 Origin
        {
            get { return _origin; }
        }
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < 0.1f)
                    _zoom = 0.1f;
            }
        }
        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public Camera(Viewport viewport)
        {
            _viewport = viewport;

            _zoom = 1f;
            _origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            _position = Vector2.Zero;
        }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-_position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }
    }
}
