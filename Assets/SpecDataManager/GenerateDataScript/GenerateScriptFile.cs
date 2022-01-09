using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class GenerateScriptFile
{
    private const string DataSetPrefix = "DataSet_";
    private readonly string _savePath;
    private readonly int _headerRows;

    /// <summary>
    /// google docs :2, excel : 3
    /// google docs 는 첫 header 는 ColumnName 으로 온다.
    /// </summary>
    /// <param name="savePath"></param>
    /// <param name="headerRows"></param>
    public GenerateScriptFile(string savePath, int headerRows = 2)    
    {
        this._savePath = savePath;
        this._headerRows = headerRows;
    }

    public void GenerateCS(DataTable sheet, string tableName)
    {
        SaveSheetCs(sheet, tableName);
    }

    public void GenerateEnum(string [] fileNames)
    {
        SaveEnum(fileNames);
    }
    public void GenerateSpecDataBase(string [] fileNames)
    {
        SaveSpecDataBase(fileNames);
    }
    
    
    public void GenerateScriptableObject(DataTable sheet, string tableName)
    {
        SaveTableAsset(sheet, tableName);
    }

    #region Save Class File

    bool SaveSheetCs(DataTable sheet, string tableName)
    {
        if (sheet.Rows.Count < _headerRows)
        {
            Debug.LogError("Excel Sheet is empty: " + sheet.TableName);
            return false;
        }

        // write table data class
        SaveTableClass(sheet, tableName);

        // write scriptable object DataSet class
        SaveDataSet(sheet, tableName);
        
        return true;
    }

    void SaveTableClass(DataTable sheet, string tableName)
    {        
        string keyType = sheet.Rows[0][0].ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using UnityEngine;\n");
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated Class From Table: {tableName}");
        sb.AppendLine("/// </summary>");
        sb.AppendFormat("public class {0} : DataFieldBase<{1}>\r\n{{", tableName, keyType);
        sb.AppendLine();

        bool isContainsGeneric = false;
        foreach (DataColumn column in sheet.Columns)
        {
            if(column.ColumnName == FieldIDName)
                continue;

            string name = column.ColumnName;
            string type = sheet.Rows[0][column].ToString();
            string comment = sheet.Rows[1][column].ToString();
            
            if (string.IsNullOrEmpty(name)) continue;
            if (string.IsNullOrEmpty(type)) type = "string";
            if (!string.IsNullOrEmpty(comment))
            {
                comment = comment.Replace("\n", ",").Replace("\r", ",");
            }
            
            sb.AppendFormat("\tpublic {0} {1};\t // {2}", type, name, comment);
            sb.AppendLine();

            if (type.StartsWith("List"))
            {
                isContainsGeneric = true;
            }
        }

        if (isContainsGeneric)
        {
            sb.Insert(0, "using System.Collections.Generic;\n");
        }
        
        
        sb.Append('}');
        sb.AppendLine();

        SaveFileAsCS(sb, tableName);
    }
    
    void SaveDataSet(DataTable sheet, string fileName)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated ScriptableObject DataSetBase.");
        sb.AppendLine("/// </summary>");

        string keyType = sheet.Rows[0][0].ToString();
        string tSetName = DataSetPrefix + fileName;
        string tClassName = fileName;
        
        sb.AppendFormat("public class {0} : DataSetBase<{1}, {2}>\r\n{{", tSetName, keyType, tClassName);
        sb.AppendLine();
        sb.Append('}');
        sb.AppendLine();
        
        SaveFileAsCS(sb, tSetName);
    }


    void SaveEnum(string [] enumNames, string fileName = "DataSetTypes")
    {        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated Enum");
        sb.AppendLine("/// </summary>");
        sb.AppendFormat("public enum Enum_{0}\r\n{{", fileName);
        sb.AppendLine();
        
        foreach (string typeName in enumNames)
        {
            sb.AppendFormat("\t{0},", typeName);
            sb.AppendLine();
        }
        
        sb.Append('}');
        sb.AppendLine();

        SaveFileAsCS(sb, fileName);
    }
    
    void SaveSpecDataBase(string [] enumNames, string fileName = "SpecDataBase")
    {        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated SpecDataBase");
        sb.AppendLine("/// </summary>");
        sb.AppendFormat("public class {0} : SingletonBehaviour<{0}>\r\n{{", fileName);
        sb.AppendLine();
        
        foreach (string typeName in enumNames)
        {
            sb.AppendLine("\t[SerializeField]");
            sb.AppendFormat("\tprotected {0}{1} _dataSet{1};", DataSetPrefix, typeName);
            sb.AppendLine();
            sb.AppendFormat("\tpublic static {0}{1} Instance{1} => Instance._dataSet{1};", DataSetPrefix, typeName);
            sb.AppendLine();
            sb.AppendLine();
        }
        
        sb.AppendLine("#if UNITY_EDITOR");
        sb.AppendLine("\tpublic void Reset()");
        sb.AppendLine("\t{");
        foreach (string typeName in enumNames)
        {
            sb.AppendFormat("\t\t_dataSet{1} = EditorUtil.EditorSetInspector<{0}{1}>();", DataSetPrefix, typeName);
            sb.AppendLine();
        }
        sb.AppendLine("\t}");
        sb.AppendLine();
        sb.AppendLine("#endif");
        sb.Append('}');
        sb.AppendLine();

        SaveFileAsCS(sb, fileName);
    }
    #endregion
    
    void SaveFileAsCS(StringBuilder sbData, string fileName)
    {
        // save to file
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }
        
        string path = Path.Combine(_savePath,  $"{fileName}.cs");
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(sbData.ToString());
                Debug.Log("Table saved: " + path);
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    #region Save ScriptableObject

    private const string FieldIDName = "fieldID";
    bool SaveTableAsset(DataTable sheet, string tableName)
    {
        if (sheet.Rows.Count < _headerRows)
        {
            Debug.LogError("Excel Sheet is empty: " + sheet.TableName);
            return false;
        }
        string tClassName = tableName;
        string tSetName = DataSetPrefix + tableName;

        // create Main Asset of table Set type by reflection
        var methods = typeof(ScriptableObject).GetMethods().Where(m => m.Name == "CreateInstance");
        var methodInfo = methods.First(m => m.IsGenericMethod);

        Assembly assembly = Assembly.Load("Assembly-CSharp");
        var tSetType = assembly.GetType(tSetName);
        if (tSetType == null)
            return false;
        
        MethodInfo createMethod = methodInfo.MakeGenericMethod(tSetType);

        UnityEngine.Object assetObj = (UnityEngine.Object)Activator.CreateInstance(tSetType);
        createMethod.Invoke(assetObj, null);

        // save to file
        string dstFolder = _savePath + "/ScriptableObjects/";
        if (!Directory.Exists(dstFolder))
        {
            Directory.CreateDirectory(dstFolder);
        }

        int idx = dstFolder.IndexOf("Assets/", StringComparison.Ordinal);
        string path = dstFolder.Substring(idx) + tSetName + ".asset";
        AssetDatabase.CreateAsset(assetObj, path);
        Debug.Log("Main Asset saved: " + path);

        // create asset of every row data, and add them to main asset
        for (int i = _headerRows; i < sheet.Rows.Count; i++)
        {
            var tableType = assembly.GetType(tClassName);

            UnityEngine.Object dataObj = (UnityEngine.Object)Activator.CreateInstance(tableType);
            createMethod.Invoke(dataObj, null);

            // set every field data
            object keyVal = null;
            for (var j = 0; j < sheet.Columns.Count; j++)
            {
                string name = sheet.Columns[j].ColumnName;
                string type = sheet.Rows[0][j].ToString();
                string val = sheet.Rows[i][j].ToString();

                if (string.IsNullOrEmpty(name))
                    continue;
                
                // if type is empty, treat as string value.
                if (string.IsNullOrEmpty(type))
                    type = "string";

                var retVal = SetObjectFiled(dataObj, tableType.GetField(name), type, val);
                
                if (j == 0) //name == FieldIDName && 
                {
                    keyVal = retVal;
                    // set key data with first column value
                    tableType.GetField(name).SetValue(dataObj, keyVal);
                    if(name != FieldIDName)
                        tableType.GetField(FieldIDName).SetValue(dataObj, keyVal);
                }
            }

            // add data asset to main asset
            var methodAdd = tSetType.GetMethod("Add");
            methodAdd.Invoke(assetObj, new[] { keyVal, dataObj });

            dataObj.name = tClassName + "_" + keyVal;
            AssetDatabase.AddObjectToAsset(dataObj, path);
        }

        EditorUtility.SetDirty(assetObj);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return true;
    }

    #endregion

    object SetObjectFiled(object obj, FieldInfo field, string type, string param)
    {
        object pObj = param;

        string lowerType = type.ToLower();
        try
        {
            switch (lowerType)
            {
                case "string":
                    field.SetValue(obj, param);
                    break;
                case "bool":
                    pObj = bool.Parse(param);
                    field.SetValue(obj, bool.Parse(param));
                    break;
                case "byte":
                    pObj = byte.Parse(param);
                    field.SetValue(obj, byte.Parse(param));
                    break;
                case "int":
                    pObj = int.Parse(param);
                    field.SetValue(obj, int.Parse(param));
                    break;
                case "short":
                    pObj = short.Parse(param);
                    field.SetValue(obj, short.Parse(param));
                    break;
                case "long":
                    pObj = long.Parse(param);
                    field.SetValue(obj, long.Parse(param));
                    break;
                case "ulong":
                    pObj = ulong.Parse(param);
                    field.SetValue(obj, ulong.Parse(param));
                    break;
                case "float":
                    pObj = float.Parse(param);
                    field.SetValue(obj, float.Parse(param));
                    break;
                case "double":
                    pObj = double.Parse(param);
                    field.SetValue(obj, double.Parse(param));
                    break;
                case "decimal":
                    pObj = decimal.Parse(param);
                    field.SetValue(obj, decimal.Parse(param));
                    break;
                case "vector3":
                    string[] temp = param.Substring(1, param.Length - 2).Split(',');
                    pObj = new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
                    field.SetValue(obj, pObj);
                    break;
                default:
                    if (lowerType.StartsWith("list"))
                    {
                        Regex regex = new Regex("(?<=\\<)(.*?)(?=\\>)");
                        var foundType = regex.Match(type);
                        // Debug.LogError(foundType);

                        MakeArray(ref obj, ref field, foundType.ToString(), param, false);
                        break;
                    }
                    else if (lowerType.Contains("[]"))
                    {
                        string foundType = Regex.Replace(lowerType, @"[^a-zA-Z]", "");
                        // Debug.LogError($"//{type}// //{foundType}//");

                        MakeArray(ref obj, ref field, foundType, param, true);
                        break;
                    }
                    
                    Debug.Log(lowerType);
                    Assembly assembly = Assembly.Load("Assembly-CSharp");
                    var t = assembly.GetType(type);
                    if (t != null)
                    {
                        if (t.IsEnum)
                        {
                            pObj = Enum.Parse(t, param);
                            field.SetValue(obj, Enum.Parse(t, param));
                        }
                    }
                    else
                    {
                        field.SetValue(obj, param);
                    }
                    break;
            }
        }
        catch (Exception e)
        {
            global::Debug.LogError(e);
            global::Debug.LogError($"{field.Name}, {type}, {param}");
            
            throw;
        }

        return pObj;
    }

    void MakeArray(ref object obj, ref FieldInfo field, string foundType, string param, bool isArray)
    {
        switch (foundType)
        {
            case "int":
            {
                if (isArray)
                {
                    var numbers = param.Split(',').Select(int.Parse).ToArray();
                    field.SetValue(obj, numbers);     
                }
                else
                {
                    var numbers = param.Split(',').Select(int.Parse).ToList();
                    field.SetValue(obj, numbers);     
                }
                break;
            }
            case "float":
            {
                if (isArray)
                {
                    var numbers = param.Split(',').Select(float.Parse).ToArray();
                    field.SetValue(obj, numbers);     
                }
                else
                {
                    var numbers = param.Split(',').Select(float.Parse).ToList();
                    field.SetValue(obj, numbers);     
                }      
                break;
            }
            default:
            {
                Debug.LogError($"No match : {foundType} {param}");
                break;
            }
        }
    }
}
#endif
