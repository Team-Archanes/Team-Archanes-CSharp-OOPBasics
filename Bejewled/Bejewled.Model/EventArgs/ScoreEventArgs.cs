namespace Bejewled.Model.EventArgs
{
    using Bejewled.Model.Interfaces;

    public class ScoreEventArgs : System.EventArgs
    {
        private readonly IScore score;

        public ScoreEventArgs(IScore score)
        {
            this.score = score;
        }

        public string GainedScore => this.score.PlayerScore.ToString();
    }
}