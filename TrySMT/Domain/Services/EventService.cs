using System;
using Microsoft.Xna.Framework;

namespace TrySMT.Domain.Services
{
    public class EventService
    {
        private Random rnd = new Random();
        private string lastEventMessage = "";
        private string currentEventDescription = "";
        
        private bool isEventActive = false;
        private double eventEndTime = 0;
        
        private int originalWheatPrice;
        private int originalWoodPrice;
        private int originalSilkPrice;
        private int originalGoldPrice;
        
        public void TriggerRandomEvent(MarketService market)
        {
            if (isEventActive)
            {
                RevertPrices(market);
            }
            
            originalWheatPrice = market.wheatPrice;
            originalWoodPrice = market.woodPrice;
            originalSilkPrice = market.silkPrice;
            originalGoldPrice = market.goldPrice;
            
            var eventType = rnd.Next(0, 25);
            var duration = 20.0;
            var message = "";
            
            switch (eventType)
            {
                case 0:
                    market.wheatPrice = market.wheatPrice * 2;
                    message = "Мэр объелся блинов! Цена на зерно удвоена на 30 секунд! Говорят, он съел 40 штук.";
                    break;
                case 1:
                    market.woodPrice = market.woodPrice + 30;
                    message = "Лесной пожар! Древесина взлетела. Кто-то решил пожарить шашлык посреди леса.";
                    break;
                case 2:
                    market.silkPrice = market.silkPrice * 3;
                    message = "Мода на шёлк! Даже собаки бегают в шёлковых попонах. Город сошёл с ума.";
                    break;
                case 3:
                    market.goldPrice = market.goldPrice / 2;
                    message = "Фальшивое золото! Оказалось, что это покрашенные кирпичи. Рынок в панике!";
                    break;
                case 4:
                    market.wheatPrice = market.wheatPrice / 2;
                    message = "Гигантский урожай! Зерно везде, даже в туалетах. Цены упали.";
                    break;
                case 5:
                    market.woodPrice = market.woodPrice / 2;
                    market.silkPrice = market.silkPrice / 2;
                    message = "Китайский демпинг! Лес и шёлк летят в пропасть. Панды грустят.";
                    break;
                case 6:
                    market.wheatPrice = market.wheatPrice * 3;
                    message = "Кузнечик-мутант съел половину полей! Зерно взлетело. Эколог сказал: 'А я предупреждал'.";
                    break;
                case 7:
                    market.goldPrice = market.goldPrice * 2;
                    message = "Война в Африке! Все скупают золото. Кровь, пот и благородные металлы.";
                    break;
                case 8:
                    market.wheatPrice = market.wheatPrice + 20;
                    market.woodPrice = market.woodPrice + 20;
                    message = "Нашествие бобров! Они сожрали половину леса и... почему-то пшеницу. Цены растут.";
                    break;
                case 9:
                    market.silkPrice = market.silkPrice / 3;
                    message = "Шёлковый пузырь лопнул! Вчера все носили шёлк, сегодня — мешковину.";
                    break;
                case 10:
                    market.goldPrice = market.goldPrice / 3;
                    message = "Пьяный старатель нашёл клад и пропишил его в кабаке! Золото подешевело, бармен радуется.";
                    break;
                case 11:
                    market.wheatPrice = market.wheatPrice + 50;
                    message = "Дракон прилетел и требует зерно! Иначе сожжёт город. Цены взлетели.";
                    break;
                case 12:
                    market.woodPrice = market.woodPrice * 4;
                    message = "Ёжики в тумане заблудились и съели все ёлки! Лес подорожал.";
                    break;
                case 13:
                    market.silkPrice = market.silkPrice / 2;
                    message = "Паук-мутант сплёл гигантскую паутину! Шёлка стало больше, цены рухнули.";
                    break;
                case 14:
                    market.wheatPrice = market.wheatPrice * 2;
                    message = "В городе эпидемия! Врачи прописали каши. Спрос на зерно взлетел, цены выросли!";
                    break;
                case 15:
                    market.goldPrice = market.goldPrice + 100;
                    message = "Банк напечатал слишком много денег! Инфляция, золото дорожает.";
                    break;
                case 16:
                    market.wheatPrice = market.wheatPrice + 10;
                    market.woodPrice = market.woodPrice + 10;
                    market.silkPrice = market.silkPrice + 10;
                    market.goldPrice = market.goldPrice + 10;
                    message = "Лёгкая инфляция. Всё подорожало. Даже воздух теперь платный.";
                    break;
                case 17:
                    market.wheatPrice = market.wheatPrice - 10;
                    market.woodPrice = market.woodPrice - 10;
                    market.silkPrice = market.silkPrice - 10;
                    market.goldPrice = market.goldPrice - 10;
                    message = "Дефляция! Всё дешевеет. Даже зарплаты... Ой, а это плохо.";
                    break;
                case 18:
                    market.wheatPrice = 5;
                    message = "Пшеница почти бесплатна! Мельник сжёг свою мельницу ради страховки, теперь распродаёт запасы.";
                    break;
                case 19:
                    market.goldPrice = 500;
                    message = "Гномы устроили распродажу! 'Надоело сидеть под горой' — заявил их вожак.";
                    break;
                case 20:
                    market.woodPrice = 100;
                    message = "Древесина стала золотой! Буквально. Деревья покрылись позолотой.";
                    break;
                case 21:
                    market.silkPrice = market.silkPrice * 3;
                    message = "Гусеницы устроили забастовку! Требуют лучшие листья. Шёлка стало мало, цены взлетели!";
                    break;
                case 22:
                    market.wheatPrice = 100;
                    market.woodPrice = 100;
                    market.silkPrice = 100;
                    market.goldPrice = 100;
                    message = "Коммунизм победил в одном отдельно взятом городе! Все цены сравнялись. Товарищи, это успех? Ленин недоволен.";
                    break;
                case 23:
                    market.goldPrice = 1000;
                    message = "Золотая лихорадка! Все побежали в шахты. Даже мэр бросил дела.";
                    break;
                case 24:
                    market.wheatPrice = market.wheatPrice * 2;
                    market.woodPrice = market.woodPrice * 2;
                    market.silkPrice = market.silkPrice * 2;
                    market.goldPrice = market.goldPrice * 2;
                    message = "Конец света! Нет, это просто налоги подняли. Но всё подорожало вдвое.";
                    break;
            }
            
            LimitPrices(market);
            
            isEventActive = true;
            eventEndTime = duration;
            currentEventDescription = message;
            lastEventMessage = message;
        }
        
        public void Update(GameTime gameTime, MarketService market)
        {
            if (isEventActive)
            {
                eventEndTime -= gameTime.ElapsedGameTime.TotalSeconds;
                if (eventEndTime <= 0)
                {
                    RevertPrices(market);
                    isEventActive = false;
                    currentEventDescription = "";
                    lastEventMessage = "Эффект события закончился. Цены вернулись к норме.";
                }
            }
        }
        
        private void RevertPrices(MarketService market)
        {
            market.wheatPrice = originalWheatPrice;
            market.woodPrice = originalWoodPrice;
            market.silkPrice = originalSilkPrice;
            market.goldPrice = originalGoldPrice;
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
        
        public string GetCurrentEventDescription()
        {
            return currentEventDescription;
        }
        
        public bool IsEventActive()
        {
            return isEventActive;
        }
        
        public double GetEventTimeRemaining()
        {
            return eventEndTime;
        }
    }
}