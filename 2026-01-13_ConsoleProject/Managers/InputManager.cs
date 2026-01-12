public class InputManager
{
    private static ConsoleKey _currentKey;
    
    private static readonly ConsoleKey[] Keys =
    {
        ConsoleKey.UpArrow,
        ConsoleKey.DownArrow,
        ConsoleKey.LeftArrow,
        ConsoleKey.RightArrow,
        ConsoleKey.Enter,
        ConsoleKey.Escape,
        ConsoleKey.Z,
        ConsoleKey.X,
        ConsoleKey.C,
        ConsoleKey.V,
    };
     
    // 현재 키 input 인지 확인
    public static bool IsCurrentKey(ConsoleKey input) => _currentKey == input;
    // 현재 입력 키 반환
    public static ConsoleKey GetCurrentKey() => _currentKey;
    // 현재 키 초기화
    public static void ResetKey() => _currentKey = ConsoleKey.None;
    
    // 게임 매니져에서만 호출
    // 현재 입력 키 읽고 _currentKey 갱신(키 누를 때까지 루프 멈춤)
    public static void GetUserInput()
    {
        ConsoleKey input = Console.ReadKey(true).Key;
        _currentKey = Array.IndexOf(Keys, input) >= 0 ? input : ConsoleKey.None;
        
    }
    

}