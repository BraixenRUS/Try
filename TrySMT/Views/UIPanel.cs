using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrySMT.Domain.Services;

namespace TrySMT.Views
{
    public class UIPanel
    {
        private GameManager gameManager;
        private Rectangle[] buyButtons;
        private Rectangle[] sellButtons;
        private Rectangle[] buyMaxButtons;
        private Rectangle[] sellAllButtons;
        private Rectangle[] unlockButtons;
        private bool[] buyHovered;
        private bool[] sellHovered;
        private bool[] buyMaxHovered;
        private bool[] sellAllHovered;
        private bool[] unlockHovered;
        private Texture2D buyButtonTexture;
        private Texture2D buyHoverTexture;
        private Texture2D sellButtonTexture;
        private Texture2D sellHoverTexture;
        private Texture2D buyMaxButtonTexture;
        private Texture2D buyMaxHoverTexture;
        private Texture2D sellAllButtonTexture;
        private Texture2D sellAllHoverTexture;
        private Texture2D unlockButtonTexture;
        private Texture2D unlockHoverTexture;
        private Texture2D panelBgTexture;
        
        public UIPanel(GameManager gameManager)
        {
            this.gameManager = gameManager;
            buyButtons = new Rectangle[4];
            sellButtons = new Rectangle[4];
            buyMaxButtons = new Rectangle[4];
            sellAllButtons = new Rectangle[4];
            unlockButtons = new Rectangle[4];
            buyHovered = new bool[4];
            sellHovered = new bool[4];
            buyMaxHovered = new bool[4];
            sellAllHovered = new bool[4];
            unlockHovered = new bool[4];
            
            for (var i = 0; i < 4; i++)
            {
                buyButtons[i] = new Rectangle(220, 180 + i * 35, 50, 28);
                sellButtons[i] = new Rectangle(275, 180 + i * 35, 50, 28);
                buyMaxButtons[i] = new Rectangle(330, 180 + i * 35, 55, 28);
                sellAllButtons[i] = new Rectangle(390, 180 + i * 35, 55, 28);
                unlockButtons[i] = new Rectangle(220, 180 + i * 35, 130, 28);
            }
        }
        
        public void LoadTextures(GraphicsDevice graphicsDevice)
        {
            buyButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            buyButtonTexture.SetData(new[] { Color.DarkGreen });
            
            buyHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            buyHoverTexture.SetData(new[] { Color.LimeGreen });
            
            sellButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            sellButtonTexture.SetData(new[] { Color.DarkRed });
            
            sellHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            sellHoverTexture.SetData(new[] { Color.IndianRed });
            
            buyMaxButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            buyMaxButtonTexture.SetData(new[] { Color.DarkGreen });
            
            buyMaxHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            buyMaxHoverTexture.SetData(new[] { Color.LimeGreen });
            
            sellAllButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            sellAllButtonTexture.SetData(new[] { Color.DarkRed });
            
            sellAllHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            sellAllHoverTexture.SetData(new[] { Color.IndianRed });
            
            unlockButtonTexture = new Texture2D(graphicsDevice, 1, 1);
            unlockButtonTexture.SetData(new[] { Color.DarkOrange });
            
            unlockHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            unlockHoverTexture.SetData(new[] { Color.Orange });
            
            panelBgTexture = new Texture2D(graphicsDevice, 1, 1);
            panelBgTexture.SetData(new[] { new Color(0, 0, 0, 200) });
        }
        
        public void Update(MouseState mouse)
        {
            var mousePos = new Point(mouse.X, mouse.Y);
            
            for (var i = 0; i < 4; i++)
            {
                buyHovered[i] = buyButtons[i].Contains(mousePos);
                sellHovered[i] = sellButtons[i].Contains(mousePos);
                buyMaxHovered[i] = buyMaxButtons[i].Contains(mousePos);
                sellAllHovered[i] = sellAllButtons[i].Contains(mousePos);
                unlockHovered[i] = unlockButtons[i].Contains(mousePos);
            }
        }
        
        public void HandleClick(MouseState mouse, MouseState prevMouse)
        {
            if (mouse.LeftButton != ButtonState.Pressed || prevMouse.LeftButton != ButtonState.Released)
                return;
                
            var mousePos = new Point(mouse.X, mouse.Y);
            
            for (var i = 0; i < 4; i++)
            {
                var item = gameManager.player.inventory[i];
                
                if (!item.isUnlocked)
                {
                    if (unlockButtons[i].Contains(mousePos))
                    {
                        if (i == 2)
                            gameManager.UnlockItem(2, 500, "Шёлк");
                        else if (i == 3)
                            gameManager.UnlockItem(3, 1500, "Золото");
                    }
                }
                else
                {
                    if (buyButtons[i].Contains(mousePos))
                    {
                        gameManager.BuyItem(i, 1);
                    }
                    else if (sellButtons[i].Contains(mousePos))
                    {
                        gameManager.SellItem(i, 1);
                    }
                    else if (buyMaxButtons[i].Contains(mousePos))
                    {
                        gameManager.BuyMaxItem(i);
                    }
                    else if (sellAllButtons[i].Contains(mousePos))
                    {
                        gameManager.SellAllItem(i);
                    }
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            var y = 22;
            
            var moneyText = $"Денег: {gameManager.player.money} золота";
            var moneySize = font.MeasureString(moneyText);
            spriteBatch.Draw(panelBgTexture, new Rectangle(18, (int)y - 2, (int)moneySize.X + 8, (int)moneySize.Y + 4), Color.White);
            spriteBatch.DrawString(font, moneyText, new Vector2(20, y), Color.White);
            
            y += 30;
            var incomeText = $"Пассивный доход: +{gameManager.incomeService.CalculateTotalIncome(gameManager.player)}/мин";
            var incomeSize = font.MeasureString(incomeText);
            spriteBatch.Draw(panelBgTexture, new Rectangle(18, (int)y - 2, (int)incomeSize.X + 8, (int)incomeSize.Y + 4), Color.White);
            spriteBatch.DrawString(font, incomeText, new Vector2(20, y), Color.White);
            
            if (gameManager.eventService.IsEventActive())
            {
                y += 28;
                var timeLeft = gameManager.eventService.GetEventTimeRemaining();
                var eventActiveText = $"⚠️ СОБЫТИЕ АКТИВНО! Осталось: {timeLeft:F1} сек.";
                var eventActiveSize = font.MeasureString(eventActiveText);
                spriteBatch.Draw(panelBgTexture, new Rectangle(18, (int)y - 2, (int)eventActiveSize.X + 8, (int)eventActiveSize.Y + 4), Color.White);
                spriteBatch.DrawString(font, eventActiveText, new Vector2(20, y), Color.Orange);
                
                y += 28;
                var eventDesc = gameManager.eventService.GetCurrentEventDescription();
                var scale = 1.2f;
                var descSize = font.MeasureString(eventDesc) * scale;
                spriteBatch.Draw(panelBgTexture, new Rectangle(18, (int)y - 2, (int)descSize.X + 8, (int)descSize.Y + 4), Color.White);
                spriteBatch.DrawString(font, eventDesc, new Vector2(20, y), Color.Yellow, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            
            var itemY = 180;
            for (var i = 0; i < gameManager.player.inventory.Count; i++)
            {
                var item = gameManager.player.inventory[i];
                var price = gameManager.market.GetItemPrice(item.name);
                
                if (!item.isUnlocked)
                {
                    var text = $"{item.name}: ЗАКРЫТ";
                    var textSize = font.MeasureString(text);
                    spriteBatch.Draw(panelBgTexture, new Rectangle(18, itemY - 2, (int)textSize.X + 8, (int)textSize.Y + 4), Color.White);
                    spriteBatch.DrawString(font, text, new Vector2(20, itemY), Color.Gray);
                    
                    var unlockTexture = unlockHovered[i] ? unlockHoverTexture : unlockButtonTexture;
                    spriteBatch.Draw(unlockTexture, unlockButtons[i], Color.White);
                    
                    var needMoney = i == 2 ? "500" : "1500";
                    var buttonText = $"Открыть за {needMoney}";
                    spriteBatch.DrawString(font, buttonText, new Vector2(unlockButtons[i].X + 10, unlockButtons[i].Y + 6), Color.White);
                }
                else
                {
                    var text = $"{item.name}: {price} золота | {item.quantity} шт.";
                    var textSize = font.MeasureString(text);
                    spriteBatch.Draw(panelBgTexture, new Rectangle(18, itemY - 2, (int)textSize.X + 8, (int)textSize.Y + 4), Color.White);
                    spriteBatch.DrawString(font, text, new Vector2(20, itemY), Color.White);
                    
                    var buyTexture = buyHovered[i] ? buyHoverTexture : buyButtonTexture;
                    var sellTexture = sellHovered[i] ? sellHoverTexture : sellButtonTexture;
                    var buyMaxTexture = buyMaxHovered[i] ? buyMaxHoverTexture : buyMaxButtonTexture;
                    var sellAllTexture = sellAllHovered[i] ? sellAllHoverTexture : sellAllButtonTexture;
                    
                    spriteBatch.Draw(buyTexture, buyButtons[i], Color.White);
                    spriteBatch.Draw(sellTexture, sellButtons[i], Color.White);
                    spriteBatch.Draw(buyMaxTexture, buyMaxButtons[i], Color.White);
                    spriteBatch.Draw(sellAllTexture, sellAllButtons[i], Color.White);
                    
                    spriteBatch.DrawString(font, "1", new Vector2(buyButtons[i].X + 20, buyButtons[i].Y + 6), Color.White);
                    spriteBatch.DrawString(font, "1", new Vector2(sellButtons[i].X + 20, sellButtons[i].Y + 6), Color.White);
                    spriteBatch.DrawString(font, "MAX", new Vector2(buyMaxButtons[i].X + 12, buyMaxButtons[i].Y + 6), Color.White);
                    spriteBatch.DrawString(font, "MAX", new Vector2(sellAllButtons[i].X + 12, sellAllButtons[i].Y + 6), Color.White);
                }
                itemY += 35;
            }
            
            if (gameManager.menuMessageTimer > 0)
            {
                var msgY = 500;
                var msgText = gameManager.menuMessage;
                var msgSize = font.MeasureString(msgText);
                spriteBatch.Draw(panelBgTexture, new Rectangle(18, msgY - 2, (int)msgSize.X + 8, (int)msgSize.Y + 4), Color.White);
                spriteBatch.DrawString(font, msgText, new Vector2(20, msgY), Color.Yellow);
            }
        }
    }
}