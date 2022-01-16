using System;
[Serializable]
public class Currency
{
    public Enum_CurrencyType CurrencyType;

    public double Amount;

    public Currency()
    {
    }
    
    public Currency(Enum_CurrencyType type,double amount)
    {
        CurrencyType = type;
        Amount = amount;
    }

    public bool IsEnough(double amount)
    {
        return Amount >= amount;
    }

    public void Consume(double amount)
    {
        Amount -= amount;

        if (Amount < 0)
        {
            Amount = 0;
        }
    }

    public void Add(double amount)
    {
        Amount += amount;
    }
    
}
