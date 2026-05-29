using System;

namespace TrySMT.Utils
{
    public class PriceGenerator
    {
        private Random rnd = new Random();
        
        public int GeneratePrice(int basePrice, int min, int max)
        {
            var variation = rnd.Next(-10, 11);
            var newPrice = basePrice + variation;
            
            if (newPrice < min) newPrice = min;
            if (newPrice > max) newPrice = max;
            
            return newPrice;
        }
    }
}