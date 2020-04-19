using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonGame
{
    public enum GameState
    {
        MenuScene,
        LeaderboardScene,
        EnemyScene,
        FightingScene,
        DoorScene,
        GoldScene,
        Exit
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Random random = new Random();

        public static GameWindow gameWindow;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static GameState _gameState;
        public static Camera _camera;

        public static Player player;
        MainMenu mainMenu;
        DoorScene doorScene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            gameWindow = Window;
            _camera = new Camera(GraphicsDevice.Viewport);
            mainMenu = new MainMenu();
            doorScene = new DoorScene();
            player = new Player(GraphicsDevice);
            doorScene.DoNewGenerate = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            mainMenu.Load(Content);
            doorScene.Load(Content);
            player.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public static KeyboardState keyboardState;
        public static MouseState mouseState;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            //if (keyboardState.IsKeyDown(Keys.Escape)) Exit();
            
            if (keyboardState.IsKeyDown(Keys.Up)) _camera.Zoom += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Down)) _camera.Zoom -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (_gameState == GameState.MenuScene)
            {
                mainMenu.Update();
            }
            else if (_gameState == GameState.DoorScene)
            {
                doorScene.Generate();
                doorScene.Update();
                if (doorScene.DoNewGenerate)
                {
                    player.Position((Window.ClientBounds.Width - player.Width) / 2, 180);
                    doorScene.DoNewGenerate = false;
                }
                player.Update(gameTime);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            var viewMatrix = _camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);
            if (_gameState == GameState.MenuScene)
            {
                mainMenu.Draw(spriteBatch);
            }
            else if (_gameState == GameState.DoorScene)
            {
                doorScene.Draw(spriteBatch);
                player.Draw(spriteBatch);
            }
            else if (_gameState == GameState.GoldScene)
            {

            }
            else if (_gameState == GameState.EnemyScene)
            {

            }
            else if (_gameState == GameState.FightingScene)
            {

            }
            else if (_gameState == GameState.LeaderboardScene)
            {

            }
            else if (_gameState == GameState.Exit)
            {
                Exit();
            }
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
