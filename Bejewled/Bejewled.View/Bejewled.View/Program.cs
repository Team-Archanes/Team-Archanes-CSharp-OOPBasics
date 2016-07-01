namespace Bejewled.View
{
    using Microsoft.Xna.Framework.Content;

    internal static class Program
    {

        private static void Main(string[] args)
        {
            using (var game = new BejeweledView())
            {
                game.Run();
            }
        }
    }
}