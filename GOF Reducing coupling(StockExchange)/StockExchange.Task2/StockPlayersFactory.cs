namespace StockExchange.Task2
{
    public class StockPlayersFactory
    {
        public Players CreatePlayers()
        {
            var stockExchange = new StockExchange();

            return new Players
            {
                RedSocks = new RedSocks(stockExchange),
                Blossomers = new Blossomers(stockExchange),
                RossSocks = new RossSocks(stockExchange)
            };
        }
    }
}
