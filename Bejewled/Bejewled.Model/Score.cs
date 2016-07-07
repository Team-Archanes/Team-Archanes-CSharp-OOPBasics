namespace Bejewled.Model
{
    using Bejewled.Model.Interfaces;

    public class Score : IScore
    {
        public Score()
        {
            this.PlayerScore = 0;
        }

        public int PlayerScore { get; private set; }

        public void IncreaseScore()
        {
            this.PlayerScore += 10;
        }

        public void Reset()
        {
            this.PlayerScore = 0;
        }
    }
}