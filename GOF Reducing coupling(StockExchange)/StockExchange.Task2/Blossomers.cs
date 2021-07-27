using System;

namespace StockExchange.Task2
{
    public class Blossomers
    {
        private IStockExchange _stockExchange;

        public Blossomers(IStockExchange stockExchange)
        {
            _stockExchange = stockExchange;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.SellOffer("Blossomers", stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.BuyOffer("Blossomers", stockName, numberOfShares);
        }
    }
}
