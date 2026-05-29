namespace TrySMT.Domain.Models
{
    public class Upgrade
    {
        public string name;
        public int cost;
        public string effectType;
        public int effectValue;

        public Upgrade(string name, int cost, string effectType, int effectValue)
        {
            this.name = name;
            this.cost = cost;
            this.effectType = effectType;
            this.effectValue = effectValue;
        }
    }
}