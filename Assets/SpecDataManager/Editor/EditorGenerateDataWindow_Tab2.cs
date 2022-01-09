using System.Collections.Generic;
using System.Text;
using GoogleSheetsForUnity;
using PCG.String;
using Sirenix.Utilities;
using UnityEditor;

#if UNITY_EDITOR
namespace Sirenix.OdinInspector
{
    using UnityEngine;
    using Sirenix.OdinInspector.Editor;

    public partial class EditorGenerateDataWindow : OdinEditorWindow
    {
        private const string TabName2 = "Data";
        private const string PrefKeyForFilePath = "loadScriptableFilePath";
        
        
        
        [TabGroup(TabName2)]
        [FolderPath(AbsolutePath = true, ParentFolder = "Assets/GeneratedData")]
        [OnValueChanged("OnFilePathChanged")]
        public string loadScriptableFilePath;

        void OnFilePathChanged()
        {
            if (loadScriptableFilePath.IsNullOrEmpty())
                return;
            
            EditorPrefs.SetString(PrefKeyForFilePath, loadScriptableFilePath);
        }
        
        [TabGroup(TabName2)]
        [AssetsOnly]
        public List<ScriptableObject> specData = new List<ScriptableObject>();
        
        private bool isSpecDataNull => specData.IsNullOrEmpty();
        
        void InitTab12()
        {
            if(EditorPrefs.HasKey(PrefKeyForFilePath))
                loadScriptableFilePath = EditorPrefs.GetString(PrefKeyForFilePath);
            
            this.ShowStateLog("Init tab2");
            SearchDataSet();
        }

        void SearchDataSet()
        {
            specData.Clear();
            var foundObjects = EditorUtil.GetAllScriptableInstances<ScriptableObject>();
            foreach (var obj in foundObjects)
            {
                if(specData.Contains(obj))
                    continue;
                
                if (obj.name.StartsWith("DataSet_"))
                {
                    global::Debug.Log(obj.name);
                    specData.Add(obj);
                }
            }
        }
        
        [TabGroup(TabName2)]
        [EnableIf("isSpecDataNull")]
        [Button]
        public void LoadLocalData()
        {
            SearchDataSet();
        }

        
        
    }
}
#endif
