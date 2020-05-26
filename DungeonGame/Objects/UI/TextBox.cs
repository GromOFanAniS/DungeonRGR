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
        public string Text { get => _text; set => _text = value; }

        public bool isFocused = true;
        string _text;
        Vector2 _position;
        Rectangle _hitbox;
        SpriteFont _font;
        string _label;

        public TextBox(int x, int y, string label, /*int width, int height,*/ string font, ContentManager Content)
        {
            _font = Content.Load<SpriteFont>(font);
            _position = new Vector2(x, y);
            _label = label;
            //_hitbox = new Rectangle(x, y, width, height);
            RegisterFocusedButtonForTextInput(OnInput);
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
                if (((c < 32 && c > 126) || (c < 1040 && c > 1103) || k == Keys.Escape) && c != 8) return;
                if (k == Keys.Back)
                {
                    if (_text.Length > 0)
                        _text = _text.Remove(_text.Length - 1);
                    else return;
                }
                else if (_text.Length < 25) _text += c;
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
            s.DrawString(_font, _label, new Vector2(_position.X - _font.MeasureString(_label).X / 2 + 0.5f, _position.Y - _font.MeasureString(_label).Y), Color.PowderBlue);
            s.DrawString(_font, _text, new Vector2(_position.X - _font.MeasureString(_text).X / 2, _position.Y), Color.Wheat);
        }
    }
}
