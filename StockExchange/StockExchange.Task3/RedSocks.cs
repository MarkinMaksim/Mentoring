using System;

namespace StockExchange.Task3
{
    public class RedSocks
    {
        public int SoldShares { get; set; }

        public int BoughtShares { get; set; }

        private IStockExchange _stockExchange;

        public RedSocks(IStockExchange stockExchange)
        {
            _stockExchange = stockExchange;
            _stockExchange.StockChangeEvent += Info;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.SellOffer("RedSocks", stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.BuyOffer("RedSocks", stockName, numberOfShares);
        }

        public void Info(object sender, Info e)
        {
            if (e.Buyer == "RedSocks")
            {
                BoughtShares += e.NumberOfShares;
            }

            if (e.Seller == "RedSocks")
            {
                SoldShares += e.NumberOfShares;
            }
        }
    }
}
