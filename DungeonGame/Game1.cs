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
        PlayerMenuScene,
        SkillMenuScene,
        GoldScene,
        GameOverScene,
        Exit
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static Random random = new Random();
        public static GameWindow gameWindow;
        public static GameState gameState;
        public static Camera _camera;
        public static Label actions;
        public static int difficulty;
        public static int WindowWidth => gameWindow.ClientBounds.Width;
        public static int WindowHeight => gameWindow.ClientBounds.Height;
        
        private static Player player;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Scene scene;
        private LeaderBoard leaderBoard;

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
            IsMouseVisible = true;
            gameWindow = Window;
            difficulty = 1;
            actions = new Label(10, Window.ClientBounds.Height / 7 * 6);
            _camera = new Camera(GraphicsDevice.Viewport);
            player = Player.GetPlayer();
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

            leaderBoard = LeaderBoard.GetLeaderBoard();
            MainMenu.Load(Content);
            Door.Load(Content);
            Gold.Load(Content);
            ShortSword.Load(Content);
            Sword.Load(Content);
            Bow.Load(Content);
            HealthBar.Load(Content);
            Label.Load(Content);
            Character.LoadCharacters(Content);
            MusicPlayer.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            leaderBoard.SaveBoard();
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
            if (keyboardState.IsKeyDown(Keys.T)) Console.WriteLine(player.Name);
            if (keyboardState.IsKeyDown(Keys.Up)) _camera.Zoom += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Down)) _camera.Zoom -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            player = Player.GetPlayer();

            switch (gameState)
            {
                case GameState.MenuScene:
                    if(Scene.DoNewGenerate)
                    {
                        scene = new MainMenu();
                    }
                    break;
                case GameState.DoorScene:
                    if (Scene.DoNewGenerate)
                    {
                        player.Position((Window.ClientBounds.Width - player.Width) / 2, (Window.ClientBounds.Height - player.Height) / 2);
                        scene = new DoorScene();
                    }
                    player.Update(gameTime);
                    break;
                case GameState.GoldScene:
                    if (Scene.DoNewGenerate)
                        scene = new GoldScene();
                    player.Update(gameTime);
                    break;
                case GameState.EnemyScene:
                    if (Scene.DoNewGenerate)
                    {
                        player.canWalk = false;
                        scene = new EnemyScene();
                    }
                    player.Update(gameTime);
                    break;
                case GameState.PlayerMenuScene:
                    if (Scene.DoNewGenerate)
                    {
                        player.canWalk = false;
                        player.Position((Window.ClientBounds.Width - player.Width) / 2, (Window.ClientBounds.Height - player.Height) / 2);
                        scene = new PlayerMenuScene();
                    }
                    player.Update(gameTime);
                    break;
                case GameState.SkillMenuScene:
                    if (Scene.DoNewGenerate)
                    {
                        player.canWalk = false;
                        scene = new SkillMenuScene();
                    }
                    player.Update(gameTime);
                    break;
                case GameState.GameOverScene:
                    if (Scene.DoNewGenerate)
                        scene = new GameOverScene();
                    break;
                case GameState.LeaderboardScene:
                    if (Scene.DoNewGenerate)
                        scene = new LeaderBoardScene(leaderBoard.BoardLabel);
                    break;
                case GameState.Exit:
                    Exit();
                    break;
            }
            scene?.Update(gameTime);
            actions.Update(gameTime);
            MusicPlayer.Update();
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
            scene?.Draw(spriteBatch);
            switch (gameState)
            {
                case GameState.MenuScene:
                    player.Draw(spriteBatch);
                    break;
                case GameState.DoorScene:
                    player.Draw(spriteBatch);
                    actions.Draw(spriteBatch);
                    break;
                case GameState.GoldScene:
                    player.Draw(spriteBatch);
                    actions.Draw(spriteBatch);
                    break;
                case GameState.EnemyScene:
                    player.Draw(spriteBatch);
                    actions.Draw(spriteBatch);
                    break;
                case GameState.PlayerMenuScene:
                    player.Draw(spriteBatch);
                    break;
                case GameState.SkillMenuScene:
                    break;
            }
            
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
        
    }
}
