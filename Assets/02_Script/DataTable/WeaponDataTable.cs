[CSVDataTableFile(Enum_DataTableType.WeaponData)]
public class WeaponDataTable : DataTable<WeaponDataTable, WeaponData>
{
}

public class WeaponData : Data
{
    [CSVColumnName("Name")]
    public string Name { get; set; }

    [CSVColumnName("Speed")]
    public float Speed { get; set; }

    [CSVColumnName("WeaponType")] 
    public Enum_WeaponType WeaponType { get; set; }
    
    [CSVColumnName("WeaponGradeType")] 
    public Enum_WeaponGradeType WeaponGradeType { get; set; }
}
