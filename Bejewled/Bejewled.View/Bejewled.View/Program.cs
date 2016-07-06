using Microsoft.Xna.Framework;

namespace Bejewled.View
{
    using Microsoft.Xna.Framework.Content;

    class Program : GameEnvironment
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