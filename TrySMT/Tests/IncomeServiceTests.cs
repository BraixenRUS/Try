using Xunit;
using TrySMT.Domain.Services;
using TrySMT.Domain.Models;
using System.Collections.Generic;

namespace TrySMT.Tests
{
    public class IncomeServiceTests
    {
        [Fact]
        public void CalculateTotalIncome_ShouldReturnZeroWhenNoBuildings()
        {
            var service = new IncomeService();
            var player = new Player();
            player.ownedBuildings = new List<Building>();
            
            var income = service.CalculateTotalIncome(player);
            
            Assert.Equal(0, income);
        }
        
        [Fact]
        public void CalculateTotalIncome_ShouldSumBuildingIncomes()
        {
            var service = new IncomeService();
            var player = new Player();
            player.ownedBuildings = new List<Building>
            {
                new Building("Поле", 300, 10),
                new Building("Лесопилка", 1000, 35),
                new Building("Банк", 20000, 700)
            };
            
            var income = service.CalculateTotalIncome(player);
            
            Assert.Equal(745, income);
        }
        
        [Fact]
        public void AddPassiveIncome_ShouldIncreasePlayerMoney()
        {
            var service = new IncomeService();
            var player = new Player();
            player.money = 500;
            player.ownedBuildings = new List<Building>
            {
                new Building("Поле", 300, 10)
            };
            
            service.AddPassiveIncome(player);
            
            Assert.Equal(510, player.money);
        }
        
        [Fact]
        public void CalculateTotalIncome_ShouldApplyUpgradeBonus()
        {
            var service = new IncomeService();
            var player = new Player();
            player.ownedBuildings = new List<Building>
            {
                new Building("Поле", 300, 100)
            };
            player.incomeUpgrade = 20;
            
            var income = service.CalculateTotalIncome(player);
            
            Assert.Equal(120, income);
        }
    }
}