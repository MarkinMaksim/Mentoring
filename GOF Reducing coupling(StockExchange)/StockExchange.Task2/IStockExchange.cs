namespace StockExchange.Task2
{
    public interface IStockExchange
    {
        bool SellOffer(string player, string stockName, int numberOfShares);

        bool BuyOffer(string player, string stockName, int numberOfShares);
    }
}
