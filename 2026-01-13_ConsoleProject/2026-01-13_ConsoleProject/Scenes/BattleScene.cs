class BattleScene : Scene
{
    private UIFrame _frame;

    private Rectangle _helpArea;
    private Rectangle _myArea;
    private Rectangle _enemyArea;
    private Rectangle _messageArea;

    private TrainerPokemon? _myPokemon;
    private TrainerPokemon? _enemyPokemon;

    private int _stageLv;
    private bool _isGameOver;


    // 배틀 내역, 무조건 플레이어가 먼저 공격
    private Queue<string> _messageQueue = new();

    // 현재 메세지
    private string _currentMessage = "";

    private BattlePhase _phase = BattlePhase.PlayerTurn;

    // 메세지 읽고 나서 행동
    private PendingAction _pendingAction = PendingAction.None;


    // 엔터 에서 초기화 
    private void Init()
    {
        _isGameOver = false;
        // 메시지 초기화
        _messageQueue.Clear();
        _currentMessage = "";

        // 주력 포켓몬 설정 포켓몬없으면 겜오벌
        _myPokemon = Trainer.ActivePokemon;
        if (_myPokemon == null)
        {
            _isGameOver = true;
            return;
        }

        // 스테이지 초기화
        _stageLv = 0;

        // 적 포켓몬 생성
        CreatEnemy();

        // 무조건 플레이가 먼저 공격(선빵 필승)
        // 스피드 구현은 나중에...
        _phase = BattlePhase.PlayerTurn;
        _pendingAction = PendingAction.None;

        // 시작 멘트
        _messageQueue.Enqueue("Z 키는 메세지를 읽을 수 있습니다.");
        //_messageQueue.Enqueue("메세지를 다 읽으면 A키로 공격 가능합니다.");
        ShowNextMessage();
    }

    private void OnPressZ()
    {
        // 메세지 전부 읽기
        if (_messageQueue.Count > 0)
        {
            ShowNextMessage();
            return;
        }

        // 메세지 전부 읽고, 행동 실행 
        if (_pendingAction == PendingAction.EnemyAttack)
        {
            _pendingAction = PendingAction.None;
            StartEnemyAttack();
            return;
        }

        if (_pendingAction == PendingAction.StageClear)
        {
            _pendingAction = PendingAction.None;
            StartNextStage();
            return;
        }

        if (_pendingAction == PendingAction.GameOver)
        {
            _pendingAction = PendingAction.None;
            _currentMessage = "";
            _isGameOver = true;
            return;
        }
        
        _currentMessage = "";
    }

    private void StartPlayerAttack()
    {
        if (_myPokemon == null || _enemyPokemon == null) return;

        int damage = _myPokemon.Atk;

        _messageQueue.Clear();

        _messageQueue.Enqueue($"나의 {_myPokemon.BasePokemonData.Name}의 공격");
        _enemyPokemon.TakeDamage(damage);
        _messageQueue.Enqueue($"{_enemyPokemon.BasePokemonData.Name}는" +
                              $" {damage}의 데미지를 받았다.");
        _messageQueue.Enqueue($"{_enemyPokemon.BasePokemonData.Name}의 체력은" +
                              $"{_enemyPokemon.Hp}으로 떨어졌다.");

        if (_enemyPokemon.Hp <= 0)
        {
            _messageQueue.Enqueue($"{_enemyPokemon.BasePokemonData.Name}는 쓰러졌다.");
            _pendingAction = PendingAction.StageClear;
        }
        else
        {
            _pendingAction = PendingAction.EnemyAttack;
        }

        _messageQueue.Enqueue("Z를 눌러 계속...");
        ShowNextMessage();
    }

    private void StartEnemyAttack()
    {
        if (_myPokemon == null || _enemyPokemon == null) return;

        int damage = _enemyPokemon.Atk;

        _messageQueue.Clear();

        _messageQueue.Enqueue($"상대 트레이너의 {_enemyPokemon.BasePokemonData.Name}의 공격!");
        _myPokemon.TakeDamage(damage);
        _messageQueue.Enqueue($"{_myPokemon.BasePokemonData.Name}는 {damage}의 데미지를 받았다.");
        _messageQueue.Enqueue($"{_myPokemon.BasePokemonData.Name}의 체력은 {_myPokemon.Hp} 남았다.");

        // 나의 포켓몬이 죽으면 게임오버
        if (_myPokemon.Hp <= 0)
        {
            _messageQueue.Enqueue($"{_myPokemon.BasePokemonData.Name}는 쓰러졌다!");
            _messageQueue.Enqueue("Z를 눌러 계속...");
            _pendingAction = PendingAction.GameOver;
            ShowNextMessage();


            return;
        }

        // 적 턴 끝 -> 플레이어 턴으로
        _phase = BattlePhase.PlayerTurn;
        _messageQueue.Enqueue("Z를 눌러 계속...");
        ShowNextMessage();
    }


    private void StartNextStage()
    {
        if (_myPokemon == null) return;

        _stageLv++;

        _messageQueue.Clear();
        _messageQueue.Enqueue($"스테이지 {_stageLv + 1}로 이동!!");

        _myPokemon.OnStageUp();
        _messageQueue.Enqueue($"{_myPokemon.BasePokemonData.Name}는 레벨업!! Lv{_myPokemon.Level}");
        _messageQueue.Enqueue("체력이 전부 회복되었다.");

        CreatEnemy();
        if (_enemyPokemon != null)
            _messageQueue.Enqueue($"새로운 적{_enemyPokemon.BasePokemonData.Name} 가 나타났다!!");

        _phase = BattlePhase.PlayerTurn;

        ShowNextMessage();
    }


    private void ShowNextMessage()
    {
        if (_messageQueue.Count > 0) _currentMessage = _messageQueue.Dequeue();
        else _currentMessage = "";
    }


    // 우선 모든 상대 포켓몬은 랜덤하게
    private void CreatEnemy()
    {
        List<PokemonData> allPokemon = PokemonCatalog.ById.Values.ToList();

        // 적 포켓몬은 모든 도감 에서 랜덤하게 생성
        // 임시 
        int randomIndex = Random.Shared.Next(0, allPokemon.Count);
        PokemonData enemyBase = allPokemon[randomIndex];

        TrainerPokemon enemy = new TrainerPokemon(enemyBase);

        // 스테이지가 오르면 레벨업 및 스탯 상승
        // 임시 규칙
        for (int i = 0; i < _stageLv; i++)
        {
            enemy.OnStageUp();
        }

        _enemyPokemon = enemy;
    }


    public override void Enter()
    {
        _frame = new UIFrame();
        BuildAreas();
        
        TrainerPokemon currentMy = Trainer.ActivePokemon;
        if (currentMy == null)
        {
            _stageLv = 0;
            _isGameOver = true;
            return;
        }



        bool isNewBattle = (_myPokemon != currentMy);
        _myPokemon = currentMy;
        if (isNewBattle)
        {
            Init();            
        }
        
    }

    public override void Update()
    {
        if (InputManager.IsCurrentKey(ConsoleKey.Q))
        {
            if (_myPokemon == null) return;
            PokemonView(_myPokemon.BasePokemonData.Id);
            return;
        }
        
        if (InputManager.IsCurrentKey(ConsoleKey.W))
        {
            if (_enemyPokemon == null) return;
            PokemonView(_enemyPokemon.BasePokemonData.Id);
            return;
        }
        
        
        if (_isGameOver && InputManager.IsCurrentKey(ConsoleKey.Enter))
            SceneManager.Change(SceneType.Title);
        
        if (InputManager.IsCurrentKey(ConsoleKey.Z))
        {
            // 메세지  읽기
            OnPressZ();
            return;
        }

        if (InputManager.IsCurrentKey(ConsoleKey.Escape))
            SceneManager.Change(SceneType.Title);
        
        if (_currentMessage != "" || _messageQueue.Count > 0) return;
        if (_phase == BattlePhase.PlayerTurn && InputManager.IsCurrentKey(ConsoleKey.A))
        {
            StartPlayerAttack();
            return;
        }
    }

    private void PokemonView(int pokemonId)
    {
        Scene scene = SceneManager.GetScene(SceneType.PokemonView);
        PokemonViewScene viewScene = (PokemonViewScene)scene;
        viewScene.Set(pokemonId);
        SceneManager.Change(SceneType.PokemonView);
    }
    
    
    
    public override void Render()
    {
        _frame.Draw();
        _helpArea.Draw();
        _myArea.Draw();
        _enemyArea.Draw();
        _messageArea.Draw();
        DrawHelp();
        
        if (_isGameOver)
        {
            DrawGameOver();
            return;
        }
        
        DrawMyStatus();
        DrawEnemyStatus();

        int messageX = _messageArea.X + 2;
        int messageY = _messageArea.Y + 1;
        
        Console.SetCursorPosition(messageX, messageY);
        _currentMessage.Print(ConsoleColor.White);
    }

    // 배틀에 필요한 트레이너 포켓몬의 스탯/정보
    private void DrawMyStatus()
    {
        if (_myPokemon == null) return;

        int x = _frame.X + 4;
        int y = _frame.Y + 6;

        Console.SetCursorPosition(x, y);
        $"[나의트레이너의 포켓몬] {_myPokemon.BasePokemonData.Name}".Print(ConsoleColor.Yellow);

        Console.SetCursorPosition(x, y + 1);
        $"Lv   : {_myPokemon.Level}".Print();

        Console.SetCursorPosition(x, y + 2);
        $"HP   : {_myPokemon.Hp} / {_myPokemon.MaxHp}".Print();

        Console.SetCursorPosition(x, y + 3);
        $"ATK  : {_myPokemon.Atk}".Print();

        Console.SetCursorPosition(x, y + 4);
        $"DEF  : {_myPokemon.Def}".Print();
    }

    // 배틀에 필요한 상대 포켓몬의 정보/스탯
    private void DrawEnemyStatus()
    {
        if (_enemyPokemon == null) return;


        int x = _frame.X + 58;
        int y = _frame.Y + 6;

        Console.SetCursorPosition(x, y);
        $"[상대트레이너의 포켓몬] {_enemyPokemon.BasePokemonData.Name}".Print(ConsoleColor.Red);

        Console.SetCursorPosition(x, y + 1);
        $"Lv   : {_enemyPokemon.Level}".Print();

        Console.SetCursorPosition(x, y + 2);
        $"HP   : {_enemyPokemon.Hp} / {_enemyPokemon.MaxHp}".Print();

        Console.SetCursorPosition(x, y + 3);
        $"ATK  : {_enemyPokemon.Atk}".Print();

        Console.SetCursorPosition(x, y + 4);
        $"DEF  : {_enemyPokemon.Def}".Print();
    }


    private void DrawGameOver()
    {
        int x = _frame.X + 45;
        int y = _frame.Y + 12;

        Console.SetCursorPosition(x, y);
        "Game Over".Print(ConsoleColor.Red);

        Console.SetCursorPosition(x, y + 2);
        "Enter를 누르면 타이틀로 이동합니다.".Print(ConsoleColor.Yellow);
        
    }

    private void DrawHelp()
    {
        int x = _helpArea.X + 2;
        int y = _helpArea.Y + 1;

        Console.SetCursorPosition(x, y);
        "조작: A 공격 / Z 진행 / ESC 타이틀 화면".Print(ConsoleColor.Green);
        Console.SetCursorPosition(x + 82, y);
        $"스테이지 레벨 : {_stageLv + 1}".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(x + 42, y);
        "/ Q / W 포켓몬 확인 가능!!".Print(ConsoleColor.Yellow);
    }

    private void BuildAreas()
    {
        int frameW = 110;
        int frameH = 30;

        // 조작키 안내
        int helpX = 2;
        int helpY = 1;
        int helpW = frameW - 4;
        int helpH = 3;
        _helpArea = new Rectangle(helpX, helpY, helpW, helpH);

        // 포켓몬들 스탯  
        int statusY = helpY + helpH + 1;
        int statusH = 12;

        int myX = 2;
        int myW = 52;
        _myArea = new Rectangle(myX, statusY, myW, statusH);

        int enemyX = myX + myW + 2;
        int enemyW = frameW - enemyX - 2;
        _enemyArea = new Rectangle(enemyX, statusY, enemyW, statusH);

        // 메세지 
        int msgX = 2;
        int msgH = 7;
        int msgY = frameH - msgH - 1;
        int msgW = frameW - 4;
        _messageArea = new Rectangle(msgX, msgY, msgW, msgH);
    }

    public override void Exit()
    {
    }
}