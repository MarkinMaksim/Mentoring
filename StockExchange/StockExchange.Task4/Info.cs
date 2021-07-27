using System;

namespace StockExchange.Task4
{
    public class Info : EventArgs
    {
        public string Buyer { get; set; }
        public string Seller { get; set; }
        public int NumberOfShares { get; set; }
    }
}
