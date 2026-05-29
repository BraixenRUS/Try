using System.Collections.Generic;

namespace TrySMT.Domain.Models
{
    public class Item
    {
        public string name;
        public int price;
        public int quantity;
        public bool isUnlocked;

        public Item(string name, int price)
        {
            this.name = name;
            this.price = price;
            quantity = 0;
            isUnlocked = false;
        }
    }
}