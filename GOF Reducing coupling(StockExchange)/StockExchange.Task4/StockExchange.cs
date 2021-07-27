using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockExchange.Task4
{
    public class StockExchange : IStockExchange
    {
        public List<Offer> _sellOffers = new List<Offer>();
        public List<Offer> _buyOffers = new List<Offer>();
        public event EventHandler<Info> StockChangeEvent = delegate { };

        public bool SellOffer(string player, string stockName, int numberOfShares)
        {
            var offer = _buyOffers.FirstOrDefault(x => x.Player != player && x.StockName == stockName && x.NumberOfShares == numberOfShares);

            if (offer != null)
            {
                _buyOffers.Remove(offer);
                StockChangeEvent(this, new Info { Buyer = offer.Player, Seller = player, NumberOfShares = numberOfShares });
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
                StockChangeEvent(this, new Info { Buyer = player, Seller = offer.Player, NumberOfShares = numberOfShares });
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
