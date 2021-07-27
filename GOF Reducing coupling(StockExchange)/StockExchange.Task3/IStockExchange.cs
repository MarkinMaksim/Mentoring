using System;

namespace StockExchange.Task3
{
    public interface IStockExchange
    {
        bool SellOffer(string player, string stockName, int numberOfShares);

        bool BuyOffer(string player, string stockName, int numberOfShares);

        event EventHandler<Info> StockChangeEvent;
    }
}
