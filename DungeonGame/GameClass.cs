using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

    public class GameClass : Game
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

        public GameClass()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
        }


        protected override void Initialize()
        {
            IsMouseVisible = true;
            gameWindow = Window;
            difficulty = 1;
            actions = new Label(10, Window.ClientBounds.Height / 7 * 6);
            _camera = new Camera(GraphicsDevice.Viewport);
            player = Player.GetPlayer();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            leaderBoard = LeaderBoard.GetLeaderBoard();
            Background.Init(Content, spriteBatch);
            MainMenu.Load(Content);
            Door.Load(Content);
            Gold.Load(Content);
            ShortSword.Load(Content);
            Sword.Load(Content);
            Bow.Load(Content);
            SkyFracture.Load(Content);
            HealthBar.Load(Content);
            Label.Load(Content);
            Character.LoadCharacters(Content);
            MusicPlayer.Load(Content);
        }


        protected override void UnloadContent()
        {
            leaderBoard.SaveBoard();
        }

        public static MouseState mouseState;

        protected override void Update(GameTime gameTime)
        {
            KeyboardHandler.Update();
            mouseState = Mouse.GetState();
            if (KeyboardHandler.IsPressed(Keys.Up)) _camera.Zoom += 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (KeyboardHandler.IsPressed(Keys.Down)) _camera.Zoom -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            player = Player.GetPlayer();

            switch (gameState)
            {
                case GameState.MenuScene:
                    if(Scene.DoNewGenerate)
                    {
                        scene = new MainMenu();
                    }
                    player.Update(gameTime);
                    player.Position((WindowWidth - player.Width) / 2, (WindowHeight - player.Height) / 2 + 24);
                    break;
                case GameState.DoorScene:
                    if (Scene.DoNewGenerate)
                    {
                        player.Position((WindowWidth - player.Width) / 2, (WindowHeight - player.Height) / 2);
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
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
