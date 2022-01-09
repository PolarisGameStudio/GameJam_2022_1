
using System.Text;

#if UNITY_EDITOR
namespace Sirenix.OdinInspector
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities.Editor;
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;

    
    public partial class EditorGenerateDataWindow : OdinEditorWindow
    {
        [MenuItem("Tools/GenerateData/open Window")]
        private static void OpenWindow()
        {
            GetWindow<EditorGenerateDataWindow>().position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        void OnInitEditorWindow()
        {
            Debug.LogError("OnInit");
            InitTab1();
            InitTab12();
        }

        
        // You can also override the method that draws each editor.
        // This come in handy if you want to add titles, boxes, or draw them in a GUI.Window etc...
        protected override void DrawEditor(int index)
        {
            base.DrawEditor(index);

            if (index != this.CurrentDrawingTargets.Count - 1)
            {
                SirenixEditorGUI.DrawThickHorizontalSeparator(15, 15);
            }
        }

        private StringBuilder sb = new StringBuilder();
        void ShowStateLog(string msg)
        {           
            Debug.Log(msg);

            sb.AppendLine(msg);
            this._logMessage = sb.ToString();
        }
        
        
        
        [Space(40)]
        [PropertyOrder(100)]
        [HideLabel]
        [ReadOnly]
        [Multiline(6)]
        [TextArea]
        public string _logMessage;

        [PropertyOrder(101)]
        [Button]
        void InitAllData()
        {
            this.OnInitEditorWindow();
        }
    }
}
#endif
