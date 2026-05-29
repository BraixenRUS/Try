using System;

namespace TrySMT.Domain.Services
{
    public class EventService
    {
        private Random rnd = new Random();
        private string lastEventMessage = "";
        
        public void TriggerRandomEvent(MarketService market)
        {
            var eventType = rnd.Next(0, 5);
            
            switch (eventType)
            {
                case 0:
                    market.wheatPrice = market.wheatPrice * 2;
                    lastEventMessage = "Мэр объелся блинов! Цена на зерно взлетела вдвое!";
                    break;
                case 1:
                    market.woodPrice = market.woodPrice + 30;
                    lastEventMessage = "Лесной пожар! Древесина стала золотой!";
                    break;
                case 2:
                    market.silkPrice = market.silkPrice * 3;
                    lastEventMessage = "Мода на шёлк! Даже собаки в шёлковых попонах!";
                    break;
                case 3:
                    market.goldPrice = market.goldPrice / 2;
                    lastEventMessage = "Фальшивое золото! Рынок в панике, цены рухнули!";
                    break;
                case 4:
                    market.wheatPrice = market.wheatPrice / 2;
                    lastEventMessage = "Гигантский урожай! Зерно дешевле грязи!";
                    break;
            }
            
            LimitPrices(market);
        }
        
        private void LimitPrices(MarketService market)
        {
            if (market.wheatPrice < 5) market.wheatPrice = 5;
            if (market.wheatPrice > 50) market.wheatPrice = 50;
            if (market.woodPrice < 15) market.woodPrice = 15;
            if (market.woodPrice > 70) market.woodPrice = 70;
            if (market.silkPrice < 40) market.silkPrice = 40;
            if (market.silkPrice > 180) market.silkPrice = 180;
            if (market.goldPrice < 100) market.goldPrice = 100;
            if (market.goldPrice > 400) market.goldPrice = 400;
        }
        
        public string GetLastEventMessage()
        {
            return lastEventMessage;
        }
    }
}