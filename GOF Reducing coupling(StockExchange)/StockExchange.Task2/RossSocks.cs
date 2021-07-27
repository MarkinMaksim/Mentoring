namespace StockExchange.Task2
{
    public class RossSocks
    {
        private IStockExchange _stockExchange;

        public RossSocks(IStockExchange stockExchange)
        {
            _stockExchange = stockExchange;
        }

        public bool SellOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.SellOffer("RossSocks", stockName, numberOfShares);
        }

        public bool BuyOffer(string stockName, int numberOfShares)
        {
            return _stockExchange.BuyOffer("RossSocks", stockName, numberOfShares);
        }
    }
}
