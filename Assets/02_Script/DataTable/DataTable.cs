using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

public class CSVDataTableFileAttribute : Attribute
{
    private readonly Enum_DataTableType _dataTableType;

    public Enum_DataTableType DataTableType => _dataTableType;

    public CSVDataTableFileAttribute(Enum_DataTableType dataTableType)
    {
        _dataTableType = dataTableType;
    }
}

public class CSVColumnNameAttribute : Attribute
{
    private readonly string _columnName;

    public string ColumnName
    {
        get { return _columnName; }
    }

    public CSVColumnNameAttribute(string columnName)
    {
        _columnName = columnName;
    }
}

public abstract class Data
{
    [CSVColumnName("Index")]
    public int Index { get; set; }
}

public abstract class DataTable<T, DATA> : Singleton<T>
where T : class, new()
where DATA : Data, new()
{
    private Enum_DataTableType _dataTableType;

    protected bool _loaded = false;

    protected Dictionary<int, DATA> _dictionary = new Dictionary<int, DATA>();
    protected List<DATA> _list = new List<DATA>();

    private Dictionary<PropertyInfo, string> _properties = new Dictionary<PropertyInfo, string>();

    private void Load()
    {
        _properties.Clear();

        // CSV 테이블 네임(=테이블 파일 경로)
        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(typeof(T));
        bool haveName = false;
        foreach (var attr in attrs)
        {
            if (attr is CSVDataTableFileAttribute)
            {
                haveName = true;

                CSVDataTableFileAttribute csvTableFileAttr = (CSVDataTableFileAttribute)attr;
                _dataTableType = csvTableFileAttr.DataTableType;

                break;
            }
        }

        Debug.Log(_dataTableType + " 테이블 로딩 중...");

        if (haveName == false)
        {
            Debug.LogError(string.Format("{0}의 파일 경로가 지정되어 있지 않습니다.", typeof(T)));
            return;
        }

        // CSV 프로퍼티
        Type dataType = typeof(DATA);
        PropertyInfo[] propertys = dataType.GetProperties();
        foreach (var property in propertys)
        {
            var attributes = property.GetCustomAttributes(typeof(CSVColumnNameAttribute), false);

             //Debug.LogError(attributes.Length == 0, string.Format("{0} 테이블의 {1} 프로퍼티는 CSVInfo 애트리뷰트가 없습니다.", dataType.Name, property.Name));

            if (attributes.Length > 0)
            {
                string columnName = (attributes[0] as CSVColumnNameAttribute).ColumnName;

                _properties.Add(property, columnName);
            }
        }

        // CSV 파싱
        List<Dictionary<string, string>> csvData = DataReader.ReadFromLocal(_dataTableType);
        if (csvData == null)
        {
            Debug.LogError(string.Format("{0}의 파싱결과에 이상이 있습니다.", dataType.Name));
            return;
        }

        if (csvData.Count == 0)
        {
            Debug.LogError(string.Format("{0}의 데이터 개수가 0개입니다.", dataType.Name));
            return;
        }

        foreach (var prop in _properties)
        {
            var columnName = prop.Value;

            if(csvData[0].ContainsKey(columnName) == false)
                Debug.LogError( string.Format("{0} 테이블에 {1} 컬럼이 없습니다.", _dataTableType, columnName));
        }

        int count = csvData.Count;

        _list = new List<DATA>(count);
        _dictionary = new Dictionary<int, DATA>(count);
        
        DATA data = null;

        
        for (int i = 0; i < count; ++i)
        {
            data = new DATA();

            // 프로퍼티(컬럼) 값 설정
            foreach (var prop in _properties)
            { 
                TypeCode typeCode = Type.GetTypeCode(prop.Key.PropertyType);
                var columnName = prop.Value;

                //Debug.LogError($"{columnName}({typeCode})");
                
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        try
                        {
                            prop.Key.SetValue(data, Boolean.Parse(csvData[i][columnName]));
                        }
                        catch (Exception e)
                        {
                            prop.Key.SetValue(data, default(bool));

                            Debug.LogError(e.Message);
                            Debug.LogError(string.Format("{0} 테이블의 {1} 컬럼({2}행)에서 {3} 타입으로 변환하는데 문제가 있습니다.", dataType.Name, columnName, i + 1, typeCode));
                        }

                        break;

                    case TypeCode.Int32:
                        try
                        {
                            prop.Key.SetValue(data, Int32.Parse(csvData[i][columnName]));
                        }
                        catch (Exception e)
                        {
                            prop.Key.SetValue(data, default(int));

                            Debug.LogError(e.Message);
                            Debug.LogError(string.Format("{0} 테이블의 {1} 컬럼({2}행)에서 {3} 타입으로 변환하는데 문제가 있습니다.", dataType.Name, columnName, i + 1, typeCode));
                        }

                        break;

                    case TypeCode.Single:
                        try
                        {
                            prop.Key.SetValue(data, Single.Parse(csvData[i][columnName]));
                        }
                        catch (Exception e)
                        {
                            prop.Key.SetValue(data, default(float));
                        
                            Debug.LogError(e.Message);
                            Debug.LogError(string.Format("{0} 테이블의 {1} 컬럼({2}행)에서 {3} 타입으로 변환하는데 문제가 있습니다.", dataType.Name, columnName, i + 1, typeCode));
                        }

                        break;
                    
                    case TypeCode.Double:
                        try
                        {
                            prop.Key.SetValue(data, Double.Parse(csvData[i][columnName]));
                        }
                        catch (Exception e)
                        {
                            prop.Key.SetValue(data, default(double));
                        
                            Debug.LogError(e.Message);
                            Debug.LogError(string.Format("{0} 테이블의 {1} 컬럼({2}행)에서 {3} 타입으로 변환하는데 문제가 있습니다.", dataType.Name, columnName, i + 1, typeCode));
                        }

                        break;

                    case TypeCode.String:
                        try
                        {
                            prop.Key.SetValue(data, csvData[i][columnName]);
                            //Debug.LogError(csvData[i][columnName]);
                        }
                        catch (Exception e)
                        {
                            prop.Key.SetValue(data, "");

                            Debug.LogError(e.Message);
                            Debug.LogError(string.Format("{0} 테이블의 {1} 컬럼({2}행)에서 {3} 타입으로 변환하는데 문제가 있습니다.", dataType.Name, columnName, i + 1, typeCode));
                        }

                        break;

                    default:
                        Debug.LogError(string.Format("{0} index({1})'s coluName({2}) : {3} is not Supported", dataType.Name, data.Index, prop.Value, typeCode));
                        break;
                }
            }

            // 데이터 중복검사 (index 기준)
            if (_dictionary.ContainsKey(data.Index) == true)
            {
                Debug.LogError(string.Format("{0} 테이블의 index({1})이 중복됩니다.", dataType.Name, data.Index));
                continue;
            }

            _list.Add(data);
            _dictionary.Add(data.Index, data);
        }

        _loaded = true;

        Debug.Log(_dataTableType + " 테이블 로딩 완료.");
    }

    public virtual List<DATA> GetAll()
    {
        LoadCheck();

        return _list;
    }
    
    public virtual DATA GetEntity(int index)
    {
        LoadCheck();

        DATA data = null;

        if (_list == null)
        {
            Debug.LogError($"{_dataTableType} index({index}) is null");

            return null;
        }

        if (_dictionary.TryGetValue(index, out data))
        {
            return data;
        }
        else
        {
            Debug.LogError($"{_dataTableType} index({index}) is null");

            return null;
        }
    }

    public int CountEntities
    {
        get
        {
            LoadCheck();

            if (_list == null)
            {
                return 0;
            }

            return _list.Count;
        }
    }

    private void LoadCheck()
    {
        if (!_loaded)
        {
            Load();
        }
    }
}