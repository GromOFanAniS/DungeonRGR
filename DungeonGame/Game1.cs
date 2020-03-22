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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static GameState _gameState;
        public static Camera _camera;

        MainMenu mainMenu;
        DoorScene doorScene;

        public static Player player;

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
            _camera = new Camera(GraphicsDevice.Viewport);
            mainMenu = new MainMenu();
            doorScene = new DoorScene();
            doorScene.DoNewGenerate = true;
            player = new Player(new Vector2(0, 0));
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
            player.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Escape)) Exit();
            MouseState mouseState = Mouse.GetState();

            if (kstate.IsKeyDown(Keys.W)) _camera.Zoom += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.S)) _camera.Zoom -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (_gameState == GameState.MenuScene)
            {
                mainMenu.Update(mouseState);
            }
            else if (_gameState == GameState.DoorScene)
            {
                doorScene.Generate();
                doorScene.Update(mouseState, gameTime);
                if (doorScene.DoNewGenerate)
                    doorScene.DoNewGenerate = false;
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
                player.Draw(gameTime, spriteBatch);
            }
            else if (_gameState == GameState.GoldScene)
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
