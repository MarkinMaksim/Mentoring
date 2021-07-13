namespace StockExchange.Task1
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            var stockExchange = new StockExchange();

            return new Players
            {
                RedSocks = new RedSocks(stockExchange),
                Blossomers = new Blossomers(stockExchange)
            };
        }
    }
}
