using Microsoft.Xna.Framework;

partial class Level : GameObjectList
{

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (quitButton.Pressed)
        {
            this.Reset();
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        }      
    }

    
    //ToDO

    
}
