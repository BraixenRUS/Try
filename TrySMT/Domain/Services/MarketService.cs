using System;
using TrySMT.Domain.Models;

namespace TrySMT.Domain.Services
{
    public class MarketService
    {
        private Random rnd = new Random();
        
        public int wheatPrice = 10;
        public int woodPrice = 25;
        public int silkPrice = 60;
        public int goldPrice = 150;

        public void UpdatePrices()
        {
            wheatPrice += rnd.Next(-3, 5);
            if (wheatPrice < 5) wheatPrice = 5;
            if (wheatPrice > 30) wheatPrice = 30;

            woodPrice += rnd.Next(-5, 7);
            if (woodPrice < 15) woodPrice = 15;
            if (woodPrice > 45) woodPrice = 45;

            silkPrice += rnd.Next(-10, 12);
            if (silkPrice < 40) silkPrice = 40;
            if (silkPrice > 100) silkPrice = 100;

            goldPrice += rnd.Next(-20, 25);
            if (goldPrice < 100) goldPrice = 100;
            if (goldPrice > 250) goldPrice = 250;
        }

        public bool BuyItem(Player player, Item item, int quantity, int currentPrice)
        {
            var totalCost = currentPrice * quantity;
            if (player.money >= totalCost)
            {
                player.money -= totalCost;
                item.quantity += quantity;
                return true;
            }
            return false;
        }

        public bool SellItem(Player player, Item item, int quantity, int currentPrice)
        {
            if (item.quantity >= quantity)
            {
                var totalIncome = currentPrice * quantity;
                
                if (player.sellPriceUpgrade > 0)
                {
                    totalIncome = totalIncome * (100 + player.sellPriceUpgrade) / 100;
                }
                
                player.money += totalIncome;
                item.quantity -= quantity;
                return true;
            }
            return false;
        }
    }
}