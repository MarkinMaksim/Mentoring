using System;
using System.Collections.Generic;
using System.Text;

namespace StockExchange.Task1
{
    public interface IStockExchange
    {
        bool SellOffer(string player, string stockName, int numberOfShares);

        bool BuyOffer(string player, string stockName, int numberOfShares);
    }
}
