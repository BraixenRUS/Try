using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrySMT.Domain.Models;
using TrySMT.Domain.Services;

namespace TrySMT
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        
        private Player player;
        private MarketService market;
        private IncomeService incomeService;
        private EventService eventService;
        
        private double passiveIncomeTimer;
        private double eventTimer;
        
        private MouseState previousMouse;

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
            
            player = new Player();
            market = new MarketService();
            incomeService = new IncomeService();
            eventService = new EventService();
            
            player.inventory.Add(new Item("Зерно", market.wheatPrice));
            player.inventory.Add(new Item("Древесина", market.woodPrice));
            
            passiveIncomeTimer = 0;
            eventTimer = 0;
            previousMouse = Mouse.GetState();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("DefaultFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            var currentMouse = Mouse.GetState();
            
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                var devZone = new Rectangle(0, 0, 50, 50);
                if (devZone.Contains(currentMouse.Position))
                {
                    eventService.TriggerRandomEvent(market);
                }
            }
            
            passiveIncomeTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (passiveIncomeTimer >= 60.0)
            {
                incomeService.AddPassiveIncome(player);
                passiveIncomeTimer = 0;
            }
            
            eventTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (eventTimer >= 45.0)
            {
                eventService.TriggerRandomEvent(market);
                eventTimer = 0;
            }
            
            previousMouse = currentMouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();
            
            _spriteBatch.DrawString(_font, "Bablo Simulator", new Vector2(400, 20), Color.Black);
            _spriteBatch.DrawString(_font, $"Money: {player.money} gold", new Vector2(20, 100), Color.Black);
            _spriteBatch.DrawString(_font, $"Passive income: +{incomeService.CalculateTotalIncome(player)}/min", new Vector2(20, 140), Color.Black);
            
            _spriteBatch.DrawString(_font, $"Wheat: {market.wheatPrice} gold", new Vector2(20, 200), Color.Black);
            _spriteBatch.DrawString(_font, $"Wood: {market.woodPrice} gold", new Vector2(20, 230), Color.Black);
            _spriteBatch.DrawString(_font, $"Silk: {market.silkPrice} gold", new Vector2(20, 260), Color.Black);
            _spriteBatch.DrawString(_font, $"Gold: {market.goldPrice} gold", new Vector2(20, 290), Color.Black);
            
            var lastEvent = eventService.GetLastEventMessage();
            if (!string.IsNullOrEmpty(lastEvent))
            {
                _spriteBatch.DrawString(_font, $"Событие: {lastEvent}", new Vector2(20, 340), Color.DarkRed);
            }
            
            _spriteBatch.DrawString(_font, "[DEV зона в левом верхнем углу]", new Vector2(20, 400), Color.Red);
            
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}