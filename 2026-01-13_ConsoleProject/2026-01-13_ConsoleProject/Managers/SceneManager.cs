public static class SceneManager
{
    public static Action OnChangeScene;
    public static Scene CurrentScene { get; private set; }

    private static Scene _previousScene;

    // SceneType으로 Scene 관리
    private static Dictionary<SceneType, Scene> _scenes = new Dictionary<SceneType, Scene>();

    // Scene 추가
    public static void AddScene(SceneType type, Scene scene)
    {
        if (_scenes.ContainsKey(type)) return;
        _scenes.Add(type, scene);
    }

    // 이전에 저장한 SceneType 로 돌아감
    public static void ChangePrevScene() => Change(_previousScene);

    // SceneType으로 장면으로 전환
    public static void Change(SceneType type)
    {
        if (!_scenes.ContainsKey(type)) return;
        Change(_scenes[type]);
    }

    // 전달 받은 Scene 장면 전환
    public static void Change(Scene scene)
    {
        Scene next = scene;                 // 다음 장면 저장
        if (CurrentScene == next) return;   // 동일한 장면이면 리턴
        CurrentScene?.Exit();               // 현재 장면 정리
        next.Enter();                       // 다음 장면 진입 

        _previousScene = CurrentScene;      // 이전 장면 저장
        CurrentScene = next;                // 현재 장면 저장
        OnChangeScene?.Invoke();            // 장면 변경 이벤트 발생
    }
    public static Scene GetScene(SceneType type)
    {
        if (!_scenes.ContainsKey(type)) return null;
        return _scenes[type];
    }
    
    
    public static void Update() => CurrentScene?.Update();
    public static void Render() => CurrentScene?.Render();
    
}