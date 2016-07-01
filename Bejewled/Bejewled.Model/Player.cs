namespace Bejewled.Model
{
    using Bejewled.Model.Interfaces;

    public class Player : IPlayer
    {
        private IScore playerScore;

        public Player(IScore playerScore)
        {
            this.PlayerScore = playerScore;
        }

        public IScore PlayerScore { get; private set; }
    }
}