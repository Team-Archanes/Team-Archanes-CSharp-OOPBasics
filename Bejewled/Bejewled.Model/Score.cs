namespace Bejewled.Model
{
    using Bejewled.Model.Interfaces;

    public class Score : IScore
    {
        private int playerScore;

        public Score()
        {
        }

        public void Reset()
        {
            this.Reset();
            playerScore = 0;
        }

        public int PlayerScore
        {
            get { return playerScore; }
            set { playerScore = value; }
        }
    }
}