public class LogScene : Scene
{

    public override void Update()
    {
        if (InputManager.IsCurrentKey(ConsoleKey.Enter))
        {
            SceneManager.ChangePrevScene();
        }
    }

    public override void Render()
    {
        Debug.Render();
    }

    public override void Enter()
    {
        
    }
    public override void Exit()
    {
        
    }
}