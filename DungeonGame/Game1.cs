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
        public static Label actions;
        Label gold;
        MainMenu mainMenu;
        Scene scene;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
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
            actions = new Label(10, Window.ClientBounds.Height - 100);
            gold = new Label(Window.ClientBounds.Width / 2, 15);
            mainMenu = new MainMenu();
            player = new Player();
            player.Position((Window.ClientBounds.Width + player.Width) / 2, 180);
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
            Door.Load(Content);
            Gold.Load(Content);
            HealthBar.Load(Content);
            Label.Load(Content);
            Character.LoadCharacters(Content);
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
            if (keyboardState.IsKeyDown(Keys.T)) Console.WriteLine(player.Name);
            if (keyboardState.IsKeyDown(Keys.Up)) _camera.Zoom += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Down)) _camera.Zoom -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;


            switch (_gameState)
            {
                case GameState.MenuScene:
                    mainMenu.Update();
                    break;
                case GameState.DoorScene:
                    if (Scene.DoNewGenerate)
                    {
                        player.Position((Window.ClientBounds.Width - player.Width) / 2, 180);
                        scene = new DoorScene();
                    }
                    
                    scene.Update(gameTime);
                    player.Update(gameTime);
                    break;
                case GameState.GoldScene:
                    if (Scene.DoNewGenerate)
                        scene = new GoldScene();

                    scene.Update(gameTime);
                    player.Update(gameTime);
                    break;
                case GameState.EnemyScene:
                    if (Scene.DoNewGenerate)
                        scene = new EnemyScene();

                    scene.Update(gameTime);
                    player.Update(gameTime);
                    break;
                case GameState.LeaderboardScene:
                    break;
            }
            actions.Update(gameTime);

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
            switch (_gameState)
            {
                case GameState.MenuScene:
                    mainMenu.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    break;
                case GameState.DoorScene:
                    scene?.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    actions.Draw(spriteBatch);
                    gold.Draw(spriteBatch);
                    break;
                case GameState.GoldScene:
                    scene?.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    actions.Draw(spriteBatch);
                    gold.Draw(spriteBatch);
                    break;
                case GameState.EnemyScene:
                    scene?.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                    actions.Draw(spriteBatch);
                    gold.Draw(spriteBatch);
                    break;
                case GameState.LeaderboardScene:
                    break;
                case GameState.Exit:
                    Exit();
                    break;
            }
            gold.Text = "У вас " + player.gold + " золота";
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
