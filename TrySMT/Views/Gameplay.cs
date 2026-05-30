using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using TrySMT.Domain.Models;
using TrySMT.Domain.Services;

namespace TrySMT.Views
{
    public class Gameplay
    {
        private GameManager gameManager;
        private UIPanel uiPanel;
        private double passiveIncomeTimer;
        private double eventTimer;
        
        private Rectangle[,] mapCells;
        private string[,] mapBuildings;
        
        private Texture2D emptyTexture;
        private Texture2D fieldTexture;
        private Texture2D sawmillTexture;
        private Texture2D bankTexture;
        private Texture2D buttonTexture;
        private Texture2D buttonHoverTexture;
        private Texture2D panelBgTexture;
        
        private Rectangle devZone;
        private Rectangle buildMenuRect;
        private Rectangle fieldButtonRect;
        private Rectangle sawmillButtonRect;
        private Rectangle bankButtonRect;
        private bool showBuildMenu;
        private int selectedCellX;
        private int selectedCellY;
        
        private MouseState previousMouse;
        private KeyboardState previousKeyboard;
        
        public Gameplay(GameManager gameManager)
        {
            this.gameManager = gameManager;
            uiPanel = new UIPanel(gameManager);
            passiveIncomeTimer = 0;
            eventTimer = 0;
            previousMouse = Mouse.GetState();
            previousKeyboard = Keyboard.GetState();
            
            mapCells = new Rectangle[5, 5];
            mapBuildings = new string[5, 5];
            
            var cellSize = 70;
            var startX = 500;
            var startY = 150;
            
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    mapCells[i, j] = new Rectangle(startX + i * cellSize, startY + j * cellSize, cellSize, cellSize);
                    mapBuildings[i, j] = "empty";
                }
            }
            
            devZone = new Rectangle(0, 0, 50, 50);
            
            buildMenuRect = new Rectangle(300, 350, 250, 150);
            fieldButtonRect = new Rectangle(310, 370, 230, 35);
            sawmillButtonRect = new Rectangle(310, 415, 230, 35);
            bankButtonRect = new Rectangle(310, 460, 230, 35);
            showBuildMenu = false;
        }
        
        public void LoadTextures(ContentManager content, GraphicsDevice graphicsDevice)
        {
            using (var stream = TitleContainer.OpenStream("Content/Sprites/empty.png"))
            {
                emptyTexture = Texture2D.FromStream(graphicsDevice, stream);
            }
            using (var stream = TitleContainer.OpenStream("Content/Sprites/field.png"))
            {
                fieldTexture = Texture2D.FromStream(graphicsDevice, stream);
            }
            using (var stream = TitleContainer.OpenStream("Content/Sprites/sawmill.png"))
            {
                sawmillTexture = Texture2D.FromStream(graphicsDevice, stream);
            }
            using (var stream = TitleContainer.OpenStream("Content/Sprites/bank.png"))
            {
                bankTexture = Texture2D.FromStream(graphicsDevice, stream);
            }
            
            buttonTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.DarkGreen });
            
            buttonHoverTexture = new Texture2D(graphicsDevice, 1, 1);
            buttonHoverTexture.SetData(new[] { Color.LimeGreen });
            
            panelBgTexture = new Texture2D(graphicsDevice, 1, 1);
            panelBgTexture.SetData(new[] { new Color(0, 0, 0, 180) });
            
            uiPanel.LoadTextures(graphicsDevice);
        }
        
        public void Update(GameTime gameTime, MouseState mouse, MouseState prevMouse, KeyboardState keyboard, KeyboardState prevKeyboard)
        {
            var deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            gameManager.Update(deltaTime, gameTime);
            
            uiPanel.Update(mouse);
            
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                var mousePos = new Point(mouse.X, mouse.Y);
                
                if (devZone.Contains(mousePos))
                {
                    gameManager.eventService.TriggerRandomEvent(gameManager.market);
                    gameManager.menuMessage = gameManager.eventService.GetLastEventMessage();
                    gameManager.menuMessageTimer = 3;
                }
                else if (showBuildMenu)
                {
                    if (fieldButtonRect.Contains(mousePos))
                    {
                        TryBuildBuilding("field", 300, 10, "Поле");
                    }
                    else if (sawmillButtonRect.Contains(mousePos))
                    {
                        TryBuildBuilding("sawmill", 1000, 35, "Лесопилка");
                    }
                    else if (bankButtonRect.Contains(mousePos))
                    {
                        TryBuildBuilding("bank", 20000, 700, "Банк");
                    }
                    else if (!buildMenuRect.Contains(mousePos))
                    {
                        showBuildMenu = false;
                    }
                }
                else
                {
                    var clickedOnButton = false;
                    
                    for (var i = 0; i < 4; i++)
                    {
                        var buyButton = new Rectangle(220, 180 + i * 35, 50, 28);
                        var sellButton = new Rectangle(275, 180 + i * 35, 50, 28);
                        var buyMaxButton = new Rectangle(330, 180 + i * 35, 55, 28);
                        var sellAllButton = new Rectangle(390, 180 + i * 35, 55, 28);
                        
                        if (buyButton.Contains(mousePos) || sellButton.Contains(mousePos) || buyMaxButton.Contains(mousePos) || sellAllButton.Contains(mousePos))
                        {
                            clickedOnButton = true;
                            break;
                        }
                    }
                    
                    if (!clickedOnButton)
                    {
                        for (var i = 0; i < 5; i++)
                        {
                            for (var j = 0; j < 5; j++)
                            {
                                if (mapCells[i, j].Contains(mousePos))
                                {
                                    HandleCellClick(i, j);
                                }
                            }
                        }
                    }
                }
            }
            
            uiPanel.HandleClick(mouse, prevMouse);
            
            if (keyboard.IsKeyDown(Keys.Escape) && prevKeyboard.IsKeyUp(Keys.Escape))
            {
                if (showBuildMenu)
                    showBuildMenu = false;
                else
                    GameState.CurrentScreen = ScreenType.MainMenu;
            }
            
            passiveIncomeTimer += deltaTime;
            if (passiveIncomeTimer >= 60.0)
            {
                gameManager.incomeService.AddPassiveIncome(gameManager.player);
                passiveIncomeTimer = 0;
                gameManager.menuMessage = $"Пассивный доход +{gameManager.incomeService.CalculateTotalIncome(gameManager.player)}!";
                gameManager.menuMessageTimer = 2;
            }
            
            eventTimer += deltaTime;
            if (eventTimer >= 45.0)
            {
                gameManager.eventService.TriggerRandomEvent(gameManager.market);
                eventTimer = 0;
                gameManager.menuMessage = gameManager.eventService.GetLastEventMessage();
                gameManager.menuMessageTimer = 3;
            }
            
            previousMouse = mouse;
            previousKeyboard = keyboard;
        }
        
        private void HandleCellClick(int x, int y)
        {
            var building = mapBuildings[x, y];
            
            if (building == "empty")
            {
                selectedCellX = x;
                selectedCellY = y;
                showBuildMenu = true;
                gameManager.menuMessage = "Выберите здание для постройки";
                gameManager.menuMessageTimer = 3;
            }
            else
            {
                gameManager.menuMessage = "Здесь уже есть здание";
                gameManager.menuMessageTimer = 2;
            }
        }
        
        private void TryBuildBuilding(string buildingType, int cost, int income, string displayName)
        {
            if (mapBuildings[selectedCellX, selectedCellY] != "empty")
            {
                gameManager.menuMessage = "Здесь уже есть здание!";
                gameManager.menuMessageTimer = 2;
                showBuildMenu = false;
                return;
            }
            
            if (gameManager.player.money >= cost)
            {
                mapBuildings[selectedCellX, selectedCellY] = buildingType;
                gameManager.player.money -= cost;
                
                var building = new Building(displayName, cost, income);
                gameManager.player.ownedBuildings.Add(building);
                
                gameManager.menuMessage = $"Построено {displayName}! +{income}/мин";
                gameManager.menuMessageTimer = 2;
                showBuildMenu = false;
            }
            else
            {
                gameManager.menuMessage = $"Недостаточно золота! Нужно {cost} для {displayName}";
                gameManager.menuMessageTimer = 2;
            }
        }
        
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            var mapStartX = 500;
            var mapStartY = 150;
            var mapWidth = 5 * 70;
            var mapHeight = 5 * 70;
            var borderThickness = 3;
            
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    var rect = mapCells[i, j];
                    
                    spriteBatch.Draw(emptyTexture, rect, Color.White);
                    
                    Texture2D texture = null;
                    
                    switch (mapBuildings[i, j])
                    {
                        case "field":
                            texture = fieldTexture;
                            break;
                        case "sawmill":
                            texture = sawmillTexture;
                            break;
                        case "bank":
                            texture = bankTexture;
                            break;
                    }
                    
                    if (texture != null)
                    {
                        spriteBatch.Draw(texture, rect, Color.White);
                    }
                    
                    if (mapBuildings[i, j] == "empty")
                    {
                        var priceText = "300";
                        var textSize = font.MeasureString(priceText);
                        
                        var bgRect = new Rectangle(rect.X + rect.Width / 2 - (int)textSize.X / 2 - 2, rect.Y + rect.Height - 22, (int)textSize.X + 4, 20);
                        spriteBatch.Draw(panelBgTexture, bgRect, Color.White);
                        spriteBatch.DrawString(font, priceText, new Vector2(rect.X + rect.Width / 2 - textSize.X / 2, rect.Y + rect.Height - 20), Color.White);
                    }
                }
            }
            
            var borderTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            borderTexture.SetData(new[] { Color.Black });
            
            spriteBatch.Draw(borderTexture, new Rectangle(mapStartX - borderThickness, mapStartY - borderThickness, mapWidth + borderThickness * 2, borderThickness), Color.White);
            spriteBatch.Draw(borderTexture, new Rectangle(mapStartX - borderThickness, mapStartY + mapHeight, mapWidth + borderThickness * 2, borderThickness), Color.White);
            spriteBatch.Draw(borderTexture, new Rectangle(mapStartX - borderThickness, mapStartY - borderThickness, borderThickness, mapHeight + borderThickness * 2), Color.White);
            spriteBatch.Draw(borderTexture, new Rectangle(mapStartX + mapWidth, mapStartY - borderThickness, borderThickness, mapHeight + borderThickness * 2), Color.White);
            
            if (showBuildMenu)
            {
                var bgTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                bgTexture.SetData(new[] { new Color(40, 40, 40, 230) });
                spriteBatch.Draw(bgTexture, buildMenuRect, Color.White);
                
                spriteBatch.DrawString(font, "Выберите здание:", new Vector2(buildMenuRect.X + 10, buildMenuRect.Y + 5), Color.White);
                
                var isFieldHovered = fieldButtonRect.Contains(previousMouse.Position);
                var isSawmillHovered = sawmillButtonRect.Contains(previousMouse.Position);
                var isBankHovered = bankButtonRect.Contains(previousMouse.Position);
                
                var fieldBtnTex = isFieldHovered ? buttonHoverTexture : buttonTexture;
                var sawmillBtnTex = isSawmillHovered ? buttonHoverTexture : buttonTexture;
                var bankBtnTex = isBankHovered ? buttonHoverTexture : buttonTexture;
                
                spriteBatch.Draw(fieldBtnTex, fieldButtonRect, Color.White);
                spriteBatch.DrawString(font, "Поле - 300 золота (+10/мин)", new Vector2(fieldButtonRect.X + 10, fieldButtonRect.Y + 8), Color.Black);
                
                spriteBatch.Draw(sawmillBtnTex, sawmillButtonRect, Color.White);
                spriteBatch.DrawString(font, "Лесопилка - 1000 золота (+35/мин)", new Vector2(sawmillButtonRect.X + 10, sawmillButtonRect.Y + 8), Color.Black);
                
                spriteBatch.Draw(bankBtnTex, bankButtonRect, Color.White);
                spriteBatch.DrawString(font, "Банк - 20000 золота (+700/мин)", new Vector2(bankButtonRect.X + 10, bankButtonRect.Y + 8), Color.Black);
            }
            
            uiPanel.Draw(spriteBatch, font);
            
            spriteBatch.DrawString(font, "DEV", new Vector2(10, 10), Color.Red);
        }
    }
}