public class MenuList
{
    private List<(string Text, Action OnSelect)> _menuList;
    private int _selectedIndex;
    public int SelectedIndex
    {
        get => _selectedIndex;
    }

    private Rectangle _outline;
    private int _maxLength;
    
    

    // 메뉴 항목 (텍스트 + 실행할 Action)
    public MenuList(params (string Text, Action OnSelect)[] menuList)
    {
        if (menuList.Length == 0) _menuList = new List<(string Text, Action OnSelect)>();
        else _menuList = menuList.ToList();

        for (int i = 0; i < _menuList.Count; i++)
        {
            int textwidth = _menuList[i].Text.GetTextWidth();
            if (textwidth > _maxLength) _maxLength = textwidth;
        }

        _outline = new Rectangle(width: _maxLength + 4, height: _menuList.Count + 2);
    }

    // 인덱스 초기화
    public void Reset() => _selectedIndex = 0;

    // 선택된 메뉴 Action 실행
    public void Select()
    {
        if (_menuList.Count < 1) return;
        _menuList[_selectedIndex].OnSelect?.Invoke();
    }

    // 메뉴 추가
    public void Add(string text, Action onSelect)
    {
        _menuList.Add((text, onSelect));

        int textwidth = text.GetTextWidth();
        if (textwidth > _maxLength) _maxLength = textwidth;

        _outline.Width = _maxLength + 6;
        _outline.Height++;
    }

    // 삭제
    public void Remove()
    {
        // 리스트에서  삭제 및 count -1
        _menuList.RemoveAt(_selectedIndex);

        int max = 0;
        // 가장 긴 텍스트 길이 구함
        foreach (var (text, onSelect) in _menuList)
        {
            int textwidth = text.GetTextWidth();
            if (textwidth > max) max = textwidth;
        }

        if (_maxLength != max) _maxLength = max;

        _outline.Width = _maxLength + 6;
        _outline.Height--;
    }

    // 인덱스 이동 관리
    public void MoveIndex(int delta)
    {
        _selectedIndex += delta;
        ClampMenuIndex();
    }
    // 메뉴 범위 고정
    private void ClampMenuIndex()
    {
        if (_selectedIndex < 0) _selectedIndex = 0;
        else if (_selectedIndex >= _menuList.Count) _selectedIndex = _menuList.Count - 1;   
    }
    
    // 메뉴 화면 출력
    public void Render(int x, int y)
    {
        _outline.X = x;
        _outline.Y = y;
        _outline.Draw();
        for (int i = 0; i < _menuList.Count; i++)
        {
            y++;
            Console.SetCursorPosition(x + 1, y);
            string prefix;
            if (i == _selectedIndex)
            {
                prefix = "▶";
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                prefix = " ";
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine($"{prefix} {_menuList[i].Text}");
        }
        Console.ResetColor();
    }


    
    
    
    
    
}