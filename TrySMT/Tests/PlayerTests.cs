using Xunit;
using TrySMT.Domain.Models;
using System.Linq;

namespace TrySMT.Tests
{
    public class PlayerTests
    {
        [Fact]
        public void Player_ShouldStartWith500Money()
        {
            var player = new Player();
            Assert.Equal(700, player.money);
        }
        
        [Fact]
        public void Player_ShouldStartWithEmptyInventory()
        {
            var player = new Player();
            Assert.NotNull(player.inventory);
            Assert.Empty(player.inventory);
        }
        
        [Fact]
        public void Player_ShouldStartWithNoBuildings()
        {
            var player = new Player();
            Assert.NotNull(player.ownedBuildings);
            Assert.Empty(player.ownedBuildings);
        }
        
        [Fact]
        public void Player_ShouldStartWithZeroUpgrades()
        {
            var player = new Player();
            Assert.Equal(0, player.storageUpgrade);
            Assert.Equal(0, player.incomeUpgrade);
            Assert.Equal(0, player.sellPriceUpgrade);
        }
        
        [Fact]
        public void Player_CanAddItemToInventory()
        {
            var player = new Player();
            var item = new Item("Зерно", 10);
            player.inventory.Add(item);
            
            Assert.Single(player.inventory);
            Assert.Equal("Зерно", player.inventory.First().name);
        }
        
        [Fact]
        public void Player_CanAddBuilding()
        {
            var player = new Player();
            var building = new Building("Поле", 300, 10);
            player.ownedBuildings.Add(building);
            
            Assert.Single(player.ownedBuildings);
            Assert.Equal("Поле", player.ownedBuildings.First().name);
        }
    }
}