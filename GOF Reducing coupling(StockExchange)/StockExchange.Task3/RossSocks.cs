using System;

namespace StockExchange.Task3
{
    public class RossSocks
    {
        public int SoldShares { get; set; }

        public int BoughtShares { get; set; }

        private IStockExchange _stockExchange;

        public RossSocks(IStockExchange stockExchange)
        {
            _stockExchange = stockExchange;
            _stockExchange.StockChangeEvent += Info;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.SellOffer("RossSocks", stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.BuyOffer("RossSocks", stockName, numberOfShares);
        }

        public void Info(object sender, Info e)
        {
            if (e.Buyer == "RossSocks")
            {
                BoughtShares += e.NumberOfShares;
            }

            if (e.Seller == "RossSocks")
            {
                SoldShares += e.NumberOfShares;
            }
        }
    }
}
