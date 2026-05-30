using TrySMT.Domain.Models;
using Microsoft.Xna.Framework;
using System; 

namespace TrySMT.Domain.Services
{
    public class GameManager
    {
        public Player player;
        public MarketService market;
        public IncomeService incomeService;
        public EventService eventService;

        public string menuMessage;
        public double menuMessageTimer;
        
        public GameManager()
        {
            player = new Player();
            market = new MarketService();
            incomeService = new IncomeService();
            eventService = new EventService();
            
            player.inventory.Add(new Item("Зерно", market.wheatPrice));
            player.inventory.Add(new Item("Древесина", market.woodPrice));
            player.inventory.Add(new Item("Шёлк", market.silkPrice));
            player.inventory.Add(new Item("Золото", market.goldPrice));
            
            player.inventory[0].isUnlocked = true;
            player.inventory[1].isUnlocked = true;
            player.inventory[2].isUnlocked = false;
            player.inventory[3].isUnlocked = false;
            
            menuMessage = "Нажмите 1-4 для покупки, 5-8 для продажи";
            menuMessageTimer = 0;
        }
        
        public void Update(double deltaTime, GameTime gameTime)
        {
            if (menuMessageTimer > 0)
            {
                menuMessageTimer -= deltaTime;
            }
            
            eventService.Update(gameTime, market);
        }
        
        public void UnlockItem(int itemIndex, int cost, string itemName)
        {
            var item = player.inventory[itemIndex];
    
            if (item.isUnlocked)
            {
                menuMessage = $"{itemName} уже открыт!";
                menuMessageTimer = 2;
                return;
            }
    
            if (player.money >= cost)
            {
                player.money -= cost;
                item.isUnlocked = true;
                menuMessage = $"Открыт {itemName} за {cost} золота! Теперь вы можете его покупать и продавать.";
                menuMessageTimer = 2;
            }
            else
            {
                menuMessage = $"Недостаточно золота для открытия {itemName}! Нужно: {cost}";
                menuMessageTimer = 2;
            }
        }
        
        public void BuyItem(int itemIndex)
        {
            var item = player.inventory[itemIndex];
            
            if (!item.isUnlocked)
            {
                if (itemIndex == 2 && player.money >= 500)
                {
                    player.money -= 500;
                    item.isUnlocked = true;
                    menuMessage = "Открыт Шёлк за 500 золота!";
                    menuMessageTimer = 2;
                }
                else if (itemIndex == 3 && player.money >= 1500)
                {
                    player.money -= 1500;
                    item.isUnlocked = true;
                    menuMessage = "Открыто Золото за 1500 золота!";
                    menuMessageTimer = 2;
                }
                else
                {
                    var needMoney = itemIndex == 2 ? 500 : 1500;
                    menuMessage = $"Недостаточно золота для открытия {item.name}! Нужно: {needMoney}";
                    menuMessageTimer = 2;
                }
                return;
            }
            
            var price = market.GetItemPrice(item.name);
            
            if (market.BuyItem(player, item, 1, price))
            {
                menuMessage = $"Куплен {item.name} за {price} золота";
            }
            else
            {
                menuMessage = $"Недостаточно золота для покупки {item.name}! Нужно: {price}";
            }
            menuMessageTimer = 2;
        }
        
        public void SellItem(int itemIndex)
        {
            var item = player.inventory[itemIndex];
            
            if (!item.isUnlocked)
            {
                menuMessage = $"{item.name} ещё не открыт!";
                menuMessageTimer = 2;
                return;
            }
            
            if (item.quantity <= 0)
            {
                menuMessage = $"У вас нет {item.name} для продажи!";
                menuMessageTimer = 2;
                return;
            }
            
            var price = market.GetItemPrice(item.name);
            
            if (market.SellItem(player, item, 1, price))
            {
                menuMessage = $"Продан {item.name} за {price} золота";
            }
            else
            {
                menuMessage = $"Ошибка продажи {item.name}!";
            }
            menuMessageTimer = 2;
        }
        
        public void BuyBuilding(int buildingIndex)
        {
            Building building = null;
    
            switch (buildingIndex)
            {
                case 0:
                    building = new Building("Поле", 300, 10);
                    break;
                case 1:
                    building = new Building("Лесопилка", 1000, 35);
                    break;
                case 2:
                    building = new Building("Банк", 20000, 700);
                    break;
            }
    
            if (building == null) return;
    
            if (player.money >= building.cost)
            {
                player.money -= building.cost;
                player.ownedBuildings.Add(building);
                menuMessage = $"Куплено {building.name} за {building.cost} золота! Доход +{building.incomePerMinute}/мин";
            }
            else
            {
                menuMessage = $"Недостаточно золота для покупки {building.name}! Нужно: {building.cost}";
            }
            menuMessageTimer = 2;
        }
        public void BuyItem(int itemIndex, int quantity)
        {
            var item = player.inventory[itemIndex];
            
            if (!item.isUnlocked)
            {
                if (itemIndex == 2 && player.money >= 500)
                {
                    player.money -= 500;
                    item.isUnlocked = true;
                    menuMessage = "Открыт Шёлк за 500 золота!";
                    menuMessageTimer = 2;
                }
                else if (itemIndex == 3 && player.money >= 1500)
                {
                    player.money -= 1500;
                    item.isUnlocked = true;
                    menuMessage = "Открыто Золото за 1500 золота!";
                    menuMessageTimer = 2;
                }
                else
                {
                    var needMoney = itemIndex == 2 ? 500 : 1500;
                    menuMessage = $"Недостаточно золота для открытия {item.name}! Нужно: {needMoney}";
                    menuMessageTimer = 2;
                }
                return;
            }
            
            var price = market.GetItemPrice(item.name);
            
            if (market.BuyItem(player, item, quantity, price))
            {
                menuMessage = $"Куплен {item.name} x{quantity} за {price * quantity} золота";
            }
            else
            {
                var maxCanBuy = player.money / price;
                menuMessage = $"Недостаточно золота! Можно купить максимум {maxCanBuy} шт.";
            }
            menuMessageTimer = 2;
        }

        public void SellItem(int itemIndex, int quantity)
        {
            var item = player.inventory[itemIndex];
            
            if (!item.isUnlocked)
            {
                menuMessage = $"{item.name} ещё не открыт!";
                menuMessageTimer = 2;
                return;
            }
            
            if (item.quantity <= 0)
            {
                menuMessage = $"У вас нет {item.name} для продажи!";
                menuMessageTimer = 2;
                return;
            }
            
            var price = market.GetItemPrice(item.name);
            var actualQuantity = Math.Min(quantity, item.quantity);
            
            if (market.SellItem(player, item, actualQuantity, price))
            {
                menuMessage = $"Продан {item.name} x{actualQuantity} за {price * actualQuantity} золота";
            }
            else
            {
                menuMessage = $"Ошибка продажи {item.name}!";
            }
            menuMessageTimer = 2;
        }

        public void BuyMaxItem(int itemIndex)
        {
            var item = player.inventory[itemIndex];
            
            if (!item.isUnlocked)
            {
                var needMoney = itemIndex == 2 ? 500 : 1500;
                if (player.money >= needMoney)
                {
                    player.money -= needMoney;
                    item.isUnlocked = true;
                    menuMessage = $"Открыт {item.name} за {needMoney} золота!";
                    menuMessageTimer = 2;
                }
                else
                {
                    menuMessage = $"Недостаточно золота для открытия {item.name}! Нужно: {needMoney}";
                    menuMessageTimer = 2;
                }
                return;
            }
            
            var price = market.GetItemPrice(item.name);
            var maxCanBuy = player.money / price;
            
            if (maxCanBuy > 0)
            {
                market.BuyItem(player, item, maxCanBuy, price);
                menuMessage = $"Куплен {item.name} x{maxCanBuy} за {price * maxCanBuy} золота";
            }
            else
            {
                menuMessage = $"Недостаточно золота для покупки {item.name}! Нужно хотя бы {price}";
            }
            menuMessageTimer = 2;
        }

        public void SellAllItem(int itemIndex)
        {
            var item = player.inventory[itemIndex];
            
            if (!item.isUnlocked)
            {
                menuMessage = $"{item.name} ещё не открыт!";
                menuMessageTimer = 2;
                return;
            }
            
            if (item.quantity <= 0)
            {
                menuMessage = $"У вас нет {item.name} для продажи!";
                menuMessageTimer = 2;
                return;
            }
            
            var price = market.GetItemPrice(item.name);
            var totalIncome = price * item.quantity;
            
            market.SellItem(player, item, item.quantity, price);
            menuMessage = $"Продан весь {item.name} x{item.quantity} за {totalIncome} золота";
            menuMessageTimer = 2;
        }
    }
}