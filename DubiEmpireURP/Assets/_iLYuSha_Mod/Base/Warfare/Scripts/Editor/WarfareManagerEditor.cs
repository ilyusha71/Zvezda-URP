using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Warfare
{
    [CustomEditor(typeof(WarfareManager))]
    public class WarfareManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var scripts = targets.OfType<WarfareManager>();
            if (GUILayout.Button("Update"))
            {
                foreach (var script in scripts)
                {
                    script.MasterModelCollector();
                    EditorUtility.SetDirty(script);
                    AssetDatabase.SaveAssets();
                }
            }
            DrawDefaultInspector();
        }
    }
}
