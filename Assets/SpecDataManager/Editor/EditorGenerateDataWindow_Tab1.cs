using System.Linq;
using PCG.String;

#if UNITY_EDITOR
namespace Sirenix.OdinInspector
{
    using System.Data;
    using GoogleSheetsForUnity;
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities.Editor;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;

    
    public partial class EditorGenerateDataWindow : OdinEditorWindow
    {
        private const string PrefKeyForGenerateFilePath = "generateFilePath";
        private const string TabName1 = "Google Sheet";

        [PropertyOrder(-10)]
        [TabGroup(TabName1)]
        [AssetsOnly]
        public ConnectionData [] connectionData;

        [TabGroup(TabName1)]
        [DictionaryDrawerSettings(KeyLabel = "SheetName", ValueLabel = "DataTable")]
        [SerializeField]
        [Space]
        [ShowIf("isSheetDataReady")]
        private Dictionary<string, DataTable> dicSheetData = new Dictionary<string, DataTable>();

        private bool isDataNull => connectionData.IsNullOrEmpty();
        private bool isSheetDataReady => dicSheetData.Count > 0;
        private bool isReadyToGenerateFiles => isSheetDataReady && !GenerateFilePath.IsNullOrEmpty();

        [PropertyOrder(-9)]
        [ShowIf("isDataNull")]
        [TabGroup(TabName1)]
        [Button]
        public void InitConnectionInfo()
        {
            InitTab1();
        }

        void InitTab1()
        {
            this.ShowStateLog("Init tab1");
            dicSheetData.Clear();
            connectionData = EditorUtil.GetAllScriptableInstances<ConnectionData>();
            
            if(EditorPrefs.HasKey(PrefKeyForGenerateFilePath))
                GenerateFilePath = EditorPrefs.GetString(PrefKeyForGenerateFilePath);

        }
        
        [PropertyOrder(-8)]
        [HideIf("isDataNull")]
        [TabGroup(TabName1)] 
        [Button]
        public void OnLoadAllSheetData()
        {
            if (connectionData == null)
            {
                ShowStateLog("connectionData is null");
                return;
            }

            dicSheetData.Clear();
            foreach (var connection in connectionData)
            {
                GoogleDocsManager manager = new GoogleDocsManager(connection);
                manager.GetAllSheetData( (s, table) =>
                {
                    Debug.LogError($"callback : {s}");
                    if (dicSheetData.ContainsKey(s))
                    {
                        dicSheetData[s] = table;
                    }
                    else
                    {
                        dicSheetData.Add(s, table);
                    }
                });
            }
        }


        #region Generate file buttons
        [Title("Generate Buttons")]
        [TabGroup(TabName1)]
        [ShowIf("isSheetDataReady")]
        [FolderPath(AbsolutePath = true, ParentFolder = "Assets/GeneratedData")]
        [OnValueChanged("OnGenerateFilePathChanged")]
        public string GenerateFilePath;
        void OnGenerateFilePathChanged()
        {
            if (GenerateFilePath.IsNullOrEmpty())
                return;
            
            EditorPrefs.SetString(PrefKeyForGenerateFilePath, GenerateFilePath);
        }
        
        // [Button]
        // [TabGroup(TabName1)]
        // [ShowIf("isReadyToGenerateFiles")]
        // public void CheckData()
        // {
        //     foreach (var va in this.dicSheetData)
        //     {
        //         Debug.LogError($"{va.Key}");
        //         foreach (DataColumn column in va.Value.Columns)
        //         {
        //             Debug.LogError($"         {column.ColumnName}");
        //         }
        //     }
        // }
        [Button]
        [TabGroup(TabName1)]
        [ShowIf("isReadyToGenerateFiles")]
        public void GenerateScriptFiles()
        {
            if (this.dicSheetData.Count == 0)
            {
                ShowStateLog("dicSheetData.Count == 0");
                return;
            }
            
            GenerateScriptFile cs = new GenerateScriptFile(GenerateFilePath, 2);
            foreach (var va in this.dicSheetData)
            {
                cs.GenerateCS(va.Value, va.Key);
            }

            GenerateEnum();
        }
        // [Button]
        // [TabGroup(TabName1)]
        // [ShowIf("isReadyToGenerateFiles")]
        public void GenerateEnum()
        {
            if (this.dicSheetData.Count == 0)
            {
                ShowStateLog("dicSheetData.Count == 0");
                return;
            }
            
            GenerateScriptFile cs = new GenerateScriptFile(GenerateFilePath, 2);
            cs.GenerateEnum(dicSheetData.Keys.ToArray());
            cs.GenerateSpecDataBase(dicSheetData.Keys.ToArray());
        }

        
        [Button]
        [TabGroup(TabName1)]
        [ShowIf("isReadyToGenerateFiles")]
        public void GenerateScriptableObject()
        {
            if (this.dicSheetData.Count == 0)
            {
                ShowStateLog("dicSheetData.Count == 0");
                return;
            }
            
            GenerateScriptFile cs = new GenerateScriptFile(GenerateFilePath, 2);
            foreach (var va in this.dicSheetData)
            {
                cs.GenerateScriptableObject(va.Value, va.Key);

            }
        }
        
        #endregion

        
        // [TabGroup(TabName1)]
        // [Button]
        // public void LoadData()
        // {
        //     if (connectionData == null)
        //     {
        //         ShowStateLog("connectionData is null");
        //         return;
        //     }
        //     GoogleDocsManager manager = new GoogleDocsManager(connectionData[0]);
        //     manager.GetTable("SpecLevelUp", (s, table) =>
        //     {
        //         
        //     });
        // }
        
    }
}
#endif
