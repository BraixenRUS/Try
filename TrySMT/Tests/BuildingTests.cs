using Xunit;
using TrySMT.Domain.Models;

namespace TrySMT.Tests
{
    public class BuildingTests
    {
        [Fact]
        public void Building_ShouldHaveCorrectProperties()
        {
            var building = new Building("Тест", 500, 20);
            
            Assert.Equal("Тест", building.name);
            Assert.Equal(500, building.cost);
            Assert.Equal(20, building.incomePerMinute);
        }
        
        [Fact]
        public void FieldBuilding_ShouldCost300AndGive10Income()
        {
            var field = new Building("Поле", 300, 10);
            
            Assert.Equal(300, field.cost);
            Assert.Equal(10, field.incomePerMinute);
        }
        
        [Fact]
        public void SawmillBuilding_ShouldCost1000AndGive35Income()
        {
            var sawmill = new Building("Лесопилка", 1000, 35);
            
            Assert.Equal(1000, sawmill.cost);
            Assert.Equal(35, sawmill.incomePerMinute);
        }
        
        [Fact]
        public void BankBuilding_ShouldCost20000AndGive700Income()
        {
            var bank = new Building("Банк", 20000, 700);
            
            Assert.Equal(20000, bank.cost);
            Assert.Equal(700, bank.incomePerMinute);
        }
    }
}