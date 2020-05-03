using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Player
    {
        public StringBuilder Name { get; set; }
        public int gold = 0;

        Animation idle;
        Animation walk;
        Animation currentAnimation;
        static Texture2D playerSheetTexture;
        SpriteEffects flip = SpriteEffects.None;

        public int Width { get { return idle.CurrentRectangle.Width; } }
        public int Height { get { return idle.CurrentRectangle.Height; } }

        public float X { get; set; } = -100;
        public float Y { get; set; }

        public Player()
        {
            Name = new StringBuilder();

            idle = new Animation();
                idle.AddFrame(new Rectangle(0, 0, 95, 184), TimeSpan.FromSeconds(1));
            currentAnimation = idle;

            walk = new Animation();
            for (int i = 1; i < 8; i++)
                walk.AddFrame(new Rectangle(96*i, 0, 95, 184), TimeSpan.FromSeconds(.25));
            
        }

        public void Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static void Load(GraphicsDevice graphicsDevice)
        {
            using (var stream = TitleContainer.OpenStream("Content/Player/PlayerSheet.png"))
            {
                playerSheetTexture = Texture2D.FromStream(graphicsDevice, stream);
            }
        }

        private float velocity = 250f;
        public void Update(GameTime gameTime)
        {
            this.currentAnimation = idle;
            //if (keyboardState.IsKeyDown(Keys.W)) 
            //{
            //    this.Y -= 250f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            if (Game1.keyboardState.IsKeyDown(Keys.A))
            {
                this.currentAnimation = walk;
                flip = SpriteEffects.FlipHorizontally;
                if ((this.X - velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) <= 0) return;
                this.X -= velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            //if (keyboardState.IsKeyDown(Keys.S)) 
            //{
            //    this.Y += 250f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            if (Game1.keyboardState.IsKeyDown(Keys.D)) 
            {
                this.currentAnimation = walk;
                flip = SpriteEffects.None;
                if ((this.X + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) >= (Game1.gameWindow.ClientBounds.Width - Width)) return;
                this.X += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color color = Color.White;
            var sourceRectangle = currentAnimation.CurrentRectangle;
            spriteBatch.Draw(playerSheetTexture, topLeftOfSprite, null, sourceRectangle, null, 0, null, color, flip);
        }
    }
}
