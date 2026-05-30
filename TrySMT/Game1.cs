using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrySMT.Domain.Services;
using TrySMT.Views;

namespace TrySMT
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private Texture2D backgroundTexture;
        
        private GameManager gameManager;
        private MainMenu mainMenu;
        private Gameplay gameplay;
        
        private MouseState currentMouse;
        private MouseState previousMouse;
        private KeyboardState currentKeyboard;
        private KeyboardState previousKeyboard;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();
            
            gameManager = new GameManager();
            mainMenu = new MainMenu();
            gameplay = new Gameplay(gameManager);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("SpriteFont1");
            gameplay.LoadTextures(Content, GraphicsDevice);
            mainMenu.LoadTextures(GraphicsDevice);
            
            using (var stream = TitleContainer.OpenStream("Content/background.jpg"))
            {
                backgroundTexture = Texture2D.FromStream(GraphicsDevice, stream);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GameState.ExitGame)
                Exit();
            
            currentMouse = Mouse.GetState();
            currentKeyboard = Keyboard.GetState();
            
            if (GameState.CurrentScreen == ScreenType.MainMenu)
            {
                mainMenu.Update(currentMouse, previousMouse);
            }
            else if (GameState.CurrentScreen == ScreenType.Gameplay)
            {
                gameplay.Update(gameTime, currentMouse, previousMouse, currentKeyboard, previousKeyboard);
            }
            
            previousMouse = currentMouse;
            previousKeyboard = currentKeyboard;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            
            _spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            
            if (GameState.CurrentScreen == ScreenType.MainMenu)
            {
                mainMenu.Draw(_spriteBatch, _font);
            }
            else if (GameState.CurrentScreen == ScreenType.Gameplay)
            {
                gameplay.Draw(_spriteBatch, _font);
            }
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}