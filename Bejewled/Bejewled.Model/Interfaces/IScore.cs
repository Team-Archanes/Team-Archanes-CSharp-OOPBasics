namespace Bejewled.Model.Interfaces
{
    public interface IScore
    {
        void IncreaseScore();

        int PlayerScore { get;}
    }
}