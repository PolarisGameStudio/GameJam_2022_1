using System.Collections.Generic;

public class WeaponManager : SingletonBehaviour<WeaponManager>
{
    // 무기 리스트
    private List<Weapon> _list;
    public List<Weapon> List => _list;

    private Stat _stat;
    public Stat Stat => _stat;

    public Weapon CurrentWeapon = null; 

    
    // 장착된 무기
    private readonly Dictionary<Enum_HandType, Weapon> _equippedWeapons = new Dictionary<Enum_HandType, Weapon>(2)
    {
        {Enum_HandType.Left, null},
        {Enum_HandType.Right, null},
    };
    public Dictionary<Enum_HandType, Weapon> EquippedWeapons => _equippedWeapons;
    public Weapon LeftHandWeapon => _equippedWeapons[Enum_HandType.Left];
    public Weapon RightHandWeapon => _equippedWeapons[Enum_HandType.Right];

    //
    // 무기 콤비네이션 (애니메이션 판별)
    public Enum_WeaponCombinationType WeaponCombinationType => Enum_WeaponCombinationType.Sword_Sword;
    
    
    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    private void Init()
    {
        _stat = new Stat();
        _stat.Init();

        var weaponCount = WeaponDataTable.Instance.CountEntities;

        _list = new List<Weapon>(weaponCount);
        for (int i = 0; i < weaponCount; ++i)
        {
            var weaponData = WeaponDataTable.Instance.GetEntity(i);
            
            //todo: 유저 데이터 읽어와서 WeaponOptions 리스트 생성 후 전달, 없으면 새로 생성

            List<WeaponOption> options = new List<WeaponOption>();
           var weapon = new Weapon(weaponData, options);
            
            _list.Add(weapon);
        }
        //
        // // TODO: 저장된 인덱스 불러오기
        // TryLeftHandEquip(_list[WeaponConfig.DefaultLeftHandWeaponIndex]);
        // TryRightHandEquip(_list[WeaponConfig.DefaultRightHandWeaponIndex]);

        
        //todo: 무기데이터 입력되면 저장된 유저 정보로 장착
        TryEquip(_list[0]);

        CalculateStat();
    }

    public void CalculateStat()
    {
        _stat[Enum_StatType.Damage] = 0;
        
        // 보유 효과
        foreach (var weapon in _list)
        {
            _stat[Enum_StatType.Damage] += weapon.WeaponData.Speed;
        }

        // 장착 효과
        _stat[Enum_StatType.Damage] += CurrentWeapon.WeaponData.Speed;
        
        
        // 추가 옵션 효과
        foreach (var option in CurrentWeapon.Options)
        {
            _stat[option.StatType] += option.Value;
        }
    }

    public bool TryEquip(Weapon weapon)
    {
        CurrentWeapon?.SetEquip(false);
        CurrentWeapon = weapon;
        weapon.SetEquip(true);
        
        RefreshEvent.Trigger(Enum_RefreshEventType.Weapon);
        
        CalculateStat();
        
        return true;
    }

    
    //
    // // 무기 장착
    // public bool TryLeftHandEquip(Weapon weapon) => TryEquip(Enum_HandType.Left, weapon);
    // public bool TryRightHandEquip(Weapon weapon) => TryEquip(Enum_HandType.Right, weapon);

    // public bool TryEquip(Weapon weapon)
    // {
    //     _equippedWeapons[handType]?.SetEquip(false);
    //     
    //     _equippedWeapons[handType] = weapon;
    //     weapon.SetEquip(true);
    //
    //     if (LeftHandWeapon != null && RightHandWeapon != null)
    //     {
    //         RefreshEvent.Trigger(Enum_RefreshEventType.Weapon);
    //     }
    //     
    //     CalculateStat();
    //     
    //     return true;
    // }
}
