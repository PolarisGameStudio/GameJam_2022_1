using System.Collections.Generic;
using System.Text;

public sealed class Stat
{
    private readonly List<double> _values = new List<double>(new double[(int) Enum_StatType.Count]);

    public double this[Enum_StatType statType]
    {
        get => _values[(int) statType];
        set => _values[(int) statType] = value;
    }

    public double this[int statTypeIndex]
    {
        get => _values[statTypeIndex];
        set => _values[statTypeIndex] = value;
    }

    public void Init()
    {
        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            _values[i] = 0;
        }
    }

    public Stat Copy()
    {
        Stat stat = new Stat();

        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            stat._values[i] = _values[i];
        }

        return stat;
    }

    public void Multiple(Stat stat)
    {
        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            _values[i] *= (100f + stat[i]) / 100f;
        }
        
        ValidCheck();
    }

    public void Plus(Stat stat)
    {
        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            _values[i] += stat[i];
        }
        
        ValidCheck();
    }

    public void Minus(Stat stat)
    {
        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            _values[i] -= stat[i];
        }
        
        ValidCheck();
    }

    public void Divide(Stat stat)
    {
        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            _values[i] /= (100f + stat[i]) / 100f;
        }

        ValidCheck();
    }

    public void Reset()
    {
        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            _values[i] = 0;
        }
    }

    private void ValidCheck()
    { 
        int statCount = (int) Enum_StatType.Count;

        for (int i = 0; i < statCount; ++i)
        {
            if (_values[i] < 0)
            {
                _values[i] = 0;
            }
        }
    }
    
    #if UNITY_EDITOR
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        int statCount = (int) Enum_StatType.Count;
        for (int i = 0; i < statCount; ++i)
        {
            sb.AppendLine($"{((Enum_StatType)i).ToString()} {_values[i]}");
        }

        return sb.ToString();
    }
    #endif
}