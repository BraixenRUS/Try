using System.Collections.Generic;

namespace TrySMT.Domain.Models
{
    public class Player
    {
        public int money;
        public List<Item> inventory;
        public List<Building> ownedBuildings;
        public int storageUpgrade;
        public int incomeUpgrade;
        public int sellPriceUpgrade;

        public Player()
        {
            money = 500;
            inventory = new List<Item>();
            ownedBuildings = new List<Building>();
            storageUpgrade = 0;
            incomeUpgrade = 0;
            sellPriceUpgrade = 0;
        }
    }
}