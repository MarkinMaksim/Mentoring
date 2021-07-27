using System;

namespace StockExchange.Task2
{
    public class RedSocks
    {
        private IStockExchange _stockExchange;

        public RedSocks(IStockExchange stockExchange)
        {
            _stockExchange = stockExchange;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.SellOffer("RedSocks", stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.BuyOffer("RedSocks", stockName, numberOfShares);
        }
    }
}
