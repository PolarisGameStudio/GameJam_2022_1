using System;

public class PlayerStatManager : StatManager<PlayerStatManager> , GameEventListener<RefreshEvent>
{
    // private PlayerData _playerData;
    // public PlayerData PlayerData => _playerData;
    

    private PlayerObject _playerObject;
    
    protected override void Awake()
    {
        base.Awake();

        // _playerData = PlayerDataTable.Instance.GetEntity(0);

        CalculateStat();
        
    }

    private void Start()
    {
        this.AddGameEventListening<RefreshEvent>();
    }

    protected override void InitStat()
    {
        // PlayerDataManager.PlayerDataContainer.Stat[Enum_StatType.MaxHealth];
        // Stat = PlayerDataManager.PlayerDataContainer.Stat.Copy();
        var Stat2 = PlayerDataManager.PlayerDataContainer.Stat;
        Stat[Enum_StatType.AttackSpeed] = Stat2[Enum_StatType.AttackSpeed];
        Stat[Enum_StatType.MaxHealth] = Stat2[Enum_StatType.MaxHealth];
        Stat[Enum_StatType.CriticalDamage] = Stat2[Enum_StatType.CriticalDamage];
        Stat[Enum_StatType.CriticalChance] = Stat2[Enum_StatType.CriticalChance];
        Stat[Enum_StatType.MoveSpeed] = Stat2[Enum_StatType.MoveSpeed];
        Stat[Enum_StatType.Damage] = Stat2[Enum_StatType.Damage];
        
        //Stat[Enum_StatType.Health] = Stat2[Enum_StatType.MaxHealth];

        // Stat[Enum_StatType.Health] = _playerData.Health;
        // Stat[Enum_StatType.MaxHealth] = _playerData.Health;
        // Stat[Enum_StatType.Damage] = _playerData.Damage;
        // Stat[Enum_StatType.CriticalDamage] = _playerData.CriticalDamage;
        // Stat[Enum_StatType.CriticalChance] = _playerData.CriticalChance;
        // Stat[Enum_StatType.AttackSpeed] = _playerData.AttackSpeed;
        // Stat[Enum_StatType.MoveSpeed] = _playerData.MoveSpeed;
        
        Stat[Enum_StatType.DetectRange] = 6;        // TODO: 관리가 필요한가?
        Stat[Enum_StatType.AttackRange] = 2;        // TODO: 관리가 필요한가?
    }

    protected override void CalculateStat()
    {
        InitStat();

        //CalculatePlusOperation();

        // CalculateMultipleOperation();

        if (_playerObject == null)
        {
            _playerObject = FindObjectOfType<PlayerObject>();
        }

        if (_playerObject != null)
        {
            _playerObject.CalculateStat();
        }
    }

    public void OnGameEvent(RefreshEvent e)
    {
        if (e.Type == Enum_RefreshEventType.Stat)
        {
            CalculateStat();
        }
    }

    // 합 연산
    // private void CalculatePlusOperation()
    // {
    //     Stat.Plus(GoldGrowthManager.Instance.Stat);
    //     //Stat.Plus(LevelGrowthManager.Instance.Stat);
    //     
    //     Stat.Plus(WeaponManager.Instance.Stat);
    // }

    // 곱 연산
    // private void CalculateMultipleOperation()
    // {
    //     if (BerserkManager.Instance.IsBerserkMode)
    //     {
    //         Stat.Multiple(BerserkManager.Instance.Stat);
    //     }
    // }

    public void InitHealth()
    {
        // Stat[Enum_StatType.Health] = PlayerDataManager.PlayerDataContainer.Stat[Enum_StatType.Health];
        Stat[Enum_StatType.MaxHealth] = PlayerDataManager.PlayerDataContainer.Stat[Enum_StatType.MaxHealth];
    }
}