using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrySMT.Views
{
    public class Button
    {
        public Rectangle rect;
        public string text;
        public Color color;
        public bool isHovered;
        
        public Button(Rectangle rect, string text, Color color)
        {
            this.rect = rect;
            this.text = text;
            this.color = color;
            isHovered = false;
        }
        
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            var drawColor = isHovered ? Color.Yellow : color;
            spriteBatch.DrawString(font, text, new Vector2(rect.X + 10, rect.Y + 15), drawColor);
        }
        
        public bool Contains(Point point)
        {
            return rect.Contains(point);
        }
    }
}