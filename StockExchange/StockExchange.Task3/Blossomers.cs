using System;

namespace StockExchange.Task3
{
    public class Blossomers
    {
        public int SoldShares { get; set; }

        public int BoughtShares { get; set; }

        private IStockExchange _stockExchange;

        public Blossomers(IStockExchange stockExchange)
        {
            _stockExchange = stockExchange;
            _stockExchange.StockChangeEvent += Info;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.SellOffer("Blossomers", stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.BuyOffer("Blossomers", stockName, numberOfShares);
        }

        public void Info(object sender, Info e)
        {
            if (e.Buyer == "Blossomers")
            {
                BoughtShares += e.NumberOfShares;
            }

            if (e.Seller == "Blossomers")
            {
                SoldShares += e.NumberOfShares;
            }
        }
    }
}
