using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockExchange.Task2
{
    public class StockExchange : IStockExchange
    {
        public List<Offer> _sellOffers = new List<Offer>();
        public List<Offer> _buyOffers = new List<Offer>();

        public bool SellOffer(string player, string stockName, int numberOfShares)
        {
            var offer = _buyOffers.FirstOrDefault(x => x.Player != player && x.StockName == stockName && x.NumberOfShares == numberOfShares);

            if (offer != null)
            {
                _buyOffers.Remove(offer);
                return true;
            }
            else
            {
                _sellOffers.Add(new Offer { Player = player, StockName = stockName, NumberOfShares = numberOfShares });
                return false;
            }
        }

        public bool BuyOffer(string player, string stockName, int numberOfShares)
        {
            var offer = _sellOffers.FirstOrDefault(x => x.Player != player && x.StockName == stockName && x.NumberOfShares == numberOfShares);

            if (offer != null)
            {
                _sellOffers.Remove(offer);
                return true;
            }
            else
            {
                _buyOffers.Add(new Offer { Player = player, StockName = stockName, NumberOfShares = numberOfShares });
                return false;
            }
        }
    }
}
