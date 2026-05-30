using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TrySMT.Views
{
    public class MainMenu
    {
        private Rectangle startRect;
        private Rectangle exitRect;
        private bool startHovered;
        private bool exitHovered;
        private Texture2D startButtonTexture;
        private Texture2D startHoverTexture;
        private Texture2D exitButtonTexture;
        private Texture2D exitHoverTexture;
        
        public MainMenu()
        {
            startRect = new Rectangle(400, 350, 200, 50);
            exitRect = new Rectangle(400, 430, 200, 50);
            startHovered = false;
            exitHovered = false;
        }
        
        public void LoadTextures(GraphicsDevice graphicsDevice)
        {
            startButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            startButtonTexture.SetData(new[] { Color.DarkGreen });
            
            startHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            startHoverTexture.SetData(new[] { Color.LimeGreen });
            
            exitButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            exitButtonTexture.SetData(new[] { Color.DarkRed });
            
            exitHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            exitHoverTexture.SetData(new[] { Color.IndianRed });
        }
        
        public void Update(MouseState mouse, MouseState prevMouse)
        {
            var mousePos = new Point(mouse.X, mouse.Y);
            
            startHovered = startRect.Contains(mousePos);
            exitHovered = exitRect.Contains(mousePos);
            
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                if (startRect.Contains(mousePos))
                {
                    GameState.CurrentScreen = ScreenType.Gameplay;
                }
                else if (exitRect.Contains(mousePos))
                {
                    GameState.ExitGame = true;
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            var scale = 2.5f;
            var titleSize = font.MeasureString("Bablo Simulator");
            var scaledWidth = titleSize.X * scale;
            var scaledHeight = titleSize.Y * scale;
            
            spriteBatch.DrawString(font, "Bablo Simulator", new Vector2(500 - scaledWidth / 2, 150), Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            
            var startTexture = startHovered ? startHoverTexture : startButtonTexture;
            var exitTexture = exitHovered ? exitHoverTexture : exitButtonTexture;
            
            spriteBatch.Draw(startTexture, startRect, Color.White);
            spriteBatch.Draw(exitTexture, exitRect, Color.White);
            
            var startTextSize = font.MeasureString("START");
            var exitTextSize = font.MeasureString("EXIT");
            
            spriteBatch.DrawString(font, "START", new Vector2(startRect.X + startRect.Width / 2 - startTextSize.X / 2, startRect.Y + startRect.Height / 2 - startTextSize.Y / 2), Color.White);
            spriteBatch.DrawString(font, "EXIT", new Vector2(exitRect.X + exitRect.Width / 2 - exitTextSize.X / 2, exitRect.Y + exitRect.Height / 2 - exitTextSize.Y / 2), Color.White);
        }
    }
}