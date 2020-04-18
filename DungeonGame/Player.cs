using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DungeonGame
{
    public class Player
    {
        Animation idle;
        Animation walk;
        Animation currentAnimation;
        static Texture2D playerSheetTexture;
        GraphicsDevice graphicsDevice;
        SpriteEffects flip = SpriteEffects.None;

        public int Width { get { return idle.CurrentRectangle.Width; } }
        public int Height { get { return idle.CurrentRectangle.Height; } }

        public float X { get; set; }
        public float Y { get; set; }

        public Player(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            if (playerSheetTexture == null)
            {
                using (var stream = TitleContainer.OpenStream("Content/Player/PlayerSheet.png"))
                {
                    playerSheetTexture = Texture2D.FromStream(this.graphicsDevice, stream);
                }
            }

            idle = new Animation();
                idle.AddFrame(new Rectangle(0, 0, 96, 184), TimeSpan.FromSeconds(1));
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

        public void Load()
        {
            using (var stream = TitleContainer.OpenStream("Content/Player/PlayerSheet.png"))
            {
                playerSheetTexture = Texture2D.FromStream(this.graphicsDevice, stream);
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            this.currentAnimation = idle;
            //if (keyboardState.IsKeyDown(Keys.W)) 
            //{
            //    this.Y -= 250f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            if (keyboardState.IsKeyDown(Keys.A))
            {
                this.X -= 250f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.currentAnimation = walk;
                flip = SpriteEffects.FlipHorizontally;
            }
            //if (keyboardState.IsKeyDown(Keys.S)) 
            //{
            //    this.Y += 250f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            if (keyboardState.IsKeyDown(Keys.D)) 
            {
                this.X += 250f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.currentAnimation = walk;
                flip = SpriteEffects.None;
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
