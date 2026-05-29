using TrySMT.Domain.Models;

namespace TrySMT.Domain.Services
{
    public class IncomeService
    {
        public void AddPassiveIncome(Player player)
        {
            var totalIncome = CalculateTotalIncome(player);
            player.money += totalIncome;
        }
        
        public int CalculateTotalIncome(Player player)
        {
            var total = 0;
            foreach (var building in player.ownedBuildings)
            {
                total += building.incomePerMinute;
            }
            
            if (player.incomeUpgrade > 0)
            {
                total = total * (100 + player.incomeUpgrade) / 100;
            }
            
            return total;
        }
    }
}