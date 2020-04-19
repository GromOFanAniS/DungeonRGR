using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    class TextBox
    {
        bool isFocused = false;
        StringBuilder _text = new StringBuilder();
        Vector2 _position;
        Rectangle _hitbox;
        SpriteFont _font;

        public TextBox(int x, int y, int width, int height, string font, ContentManager Content)
        {
            _font = Content.Load<SpriteFont>(font);
            _position = new Vector2(x, y);
            _hitbox = new Rectangle(x, y, width, height);
        }

        void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            Game1.gameWindow.TextInput += method;
        }

        void UnRegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            Game1.gameWindow.TextInput -= method;
        }

        void OnInput(object sender, TextInputEventArgs e)
        {
            if (isFocused)
            {
                var k = e.Key;
                var c = e.Character;
                if (((c < 32 && c > 126) || (c < 1040 && c > 1103)) && c != 8) return;
                if (k == Keys.Back)
                {
                    if (_text.Length > 0)
                        _text.Remove(_text.Length - 1, 1);
                    else return;
                }
                else _text.Append(c);
            }
        }

        MouseState lastMouseState = new MouseState();
        public void CheckClick()
        {
            MouseState currentMouseState = Game1.mouseState;
            if (_hitbox.Contains(Game1.mouseState.Position) &&
                currentMouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                isFocused = !isFocused;
                if (isFocused)
                    RegisterFocusedButtonForTextInput(OnInput);
                else
                    UnRegisterFocusedButtonForTextInput(OnInput);
            }
            lastMouseState = currentMouseState;
        }

        public void Draw(SpriteBatch s)
        {
            s.DrawString(_font, "Vvedite Imya Personazha", new Vector2(_position.X, _position.Y-15), Color.Yellow);
            s.DrawString(_font, _text, _position, Color.Black);
        }
    }
}
