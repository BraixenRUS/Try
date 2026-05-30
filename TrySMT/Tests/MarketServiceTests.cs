using Xunit;
using TrySMT.Domain.Services;
using TrySMT.Domain.Models;

namespace TrySMT.Tests
{
    public class MarketServiceTests
    {
        [Fact]
        public void BuyItem_ShouldReduceMoneyAndIncreaseQuantity()
        {
            var market = new MarketService();
            var player = new Player();
            var item = new Item("Зерно", 10);
            player.money = 100;
            
            var result = market.BuyItem(player, item, 2, 10);
            
            Assert.True(result);
            Assert.Equal(80, player.money);
            Assert.Equal(2, item.quantity);
        }
        
        [Fact]
        public void BuyItem_ShouldFailWhenNotEnoughMoney()
        {
            var market = new MarketService();
            var player = new Player();
            var item = new Item("Золото", 150);
            player.money = 100;
            
            var result = market.BuyItem(player, item, 1, 150);
            
            Assert.False(result);
            Assert.Equal(100, player.money);
            Assert.Equal(0, item.quantity);
        }
        
        [Fact]
        public void SellItem_ShouldIncreaseMoneyAndDecreaseQuantity()
        {
            var market = new MarketService();
            var player = new Player();
            var item = new Item("Древесина", 25);
            item.quantity = 5;
            player.money = 100;
            
            var result = market.SellItem(player, item, 2, 25);
            
            Assert.True(result);
            Assert.Equal(150, player.money);
            Assert.Equal(3, item.quantity);
        }
        
        [Fact]
        public void SellItem_ShouldFailWhenNotEnoughQuantity()
        {
            var market = new MarketService();
            var player = new Player();
            var item = new Item("Шёлк", 60);
            item.quantity = 1;
            
            var result = market.SellItem(player, item, 5, 60);
            
            Assert.False(result);
            Assert.Equal(1, item.quantity);
        }
        
        [Fact]
        public void UpdatePrices_ShouldChangePricesWithinBounds()
        {
            var market = new MarketService();
            
            for (var i = 0; i < 100; i++)
            {
                market.UpdatePrices();
                
                Assert.InRange(market.wheatPrice, 5, 30);
                Assert.InRange(market.woodPrice, 15, 45);
                Assert.InRange(market.silkPrice, 40, 100);
                Assert.InRange(market.goldPrice, 100, 250);
            }
        }
        
        [Fact]
        public void GetItemPrice_ShouldReturnCorrectPrice()
        {
            var market = new MarketService();
            market.wheatPrice = 15;
            market.woodPrice = 30;
            market.silkPrice = 70;
            market.goldPrice = 180;
            
            Assert.Equal(15, market.GetItemPrice("Зерно"));
            Assert.Equal(30, market.GetItemPrice("Древесина"));
            Assert.Equal(70, market.GetItemPrice("Шёлк"));
            Assert.Equal(180, market.GetItemPrice("Золото"));
            Assert.Equal(0, market.GetItemPrice("Неизвестно"));
        }
    }
}