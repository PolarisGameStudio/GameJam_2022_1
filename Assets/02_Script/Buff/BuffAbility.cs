using System.Collections.Generic;

public class BuffAbility : CharacterAbility
{
    // List<버프> : 
    // 버프 상태 : 면역, 기절, 흡혈(흡혈 %식으로 수치표현하게되면 Stat으로 ) 등 수치외적인 것
    // ex) TryStun() => { if(BuffAbility.Immune == true) 면역, else 기절 }
    // ex) PlayerStunState => Update() => { if(BuffAbility.Stun == false) IdleState }

    // 버프 스탯, Stat[Damage] = 30 => 공격력 30%증가
    // 버프디버프 Add될때마다 버프 스탯 갱신 => PlayerStat Calculate()

    public List<Buff> Buffs;

    private Stat _buffStat;
    public Stat BuffStat => _buffStat;

    public override void Init()
    {
        base.Init();

        if (_buffStat == null)
        {
            _buffStat = new Stat();
            _buffStat.Init();
        }
    }

    public void AddBuff(Buff buff)
    {
        if (Buffs.Contains(buff))
        {
            return;
        }

        Buffs.Add(buff);
        
        _buffStat.Plus(buff.Stat);
        
        _onwerObject.CalculateStat();
    }

    public void RemoveBuff(Buff buff)
    {
        if (!Buffs.Contains(buff))
        {
            return;
        }

        Buffs.Remove(buff);

        _buffStat.Minus(buff.Stat);
        
        _onwerObject.CalculateStat();
    }

    public override void ProcessAbility(float deltaTime)
    {
        for (var i = Buffs.Count - 1; i >= 0; i--)
        {
            var buff = Buffs[i];
            
            if (buff.CheckBuffEnd(deltaTime))
            {
                RemoveBuff(buff);
            }
        }
    }
}