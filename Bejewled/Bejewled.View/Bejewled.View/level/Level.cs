using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{
    protected bool locked, solved;
    protected Button quitButton;

    public Level(int levelIndex)
    {
        // load the backgrounds
        GameObjectList backgrounds = new GameObjectList(0, "backgrounds");
        SpriteGameObject background_main = new SpriteGameObject("Backgrounds/spr_sky");
        background_main.Position = new Vector2(0, GameEnvironment.Screen.Y - background_main.Height);
        backgrounds.Add(background_main);

        // add a few random mountains
        for (int i = 0; i < 5; i++)
        {
            SpriteGameObject mountain = new SpriteGameObject("Backgrounds/spr_mountain_" + (GameEnvironment.Random.Next(2) + 1), 1);
            mountain.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * GameEnvironment.Screen.X - mountain.Width / 2, GameEnvironment.Screen.Y - mountain.Height);
            backgrounds.Add(mountain);
        }

        
    }

    
    //ToDo
    

    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    public bool Solved
    {
        get { return solved; }
        set { solved = value; }
    }
}

