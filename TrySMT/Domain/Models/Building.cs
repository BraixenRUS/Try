namespace TrySMT.Domain.Models
{
    public class Building
    {
        public string name;
        public int cost;
        public int incomePerMinute;

        public Building(string name, int cost, int incomePerMinute)
        {
            this.name = name;
            this.cost = cost;
            this.incomePerMinute = incomePerMinute;
        }
    }
}