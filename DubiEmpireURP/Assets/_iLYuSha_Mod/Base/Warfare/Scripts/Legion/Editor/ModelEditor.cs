using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Warfare.Legion
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Info))]
    public class ModelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var scripts = targets.OfType<Info>();
            if (GUILayout.Button("Join Database"))
            {
                foreach (var script in scripts)
                {
                    script.JoinDatabase();
                    Database database = AssetDatabase.LoadAssetAtPath<Database>("Assets/_iLYuSha_Mod/Base/Warfare/Data/Legion/Database.asset");
                    string assetName = "[" + script.LegionModelNumber.ToString() + "]" + script.m_faction.ToString() + " Legion " + (script.m_legion + 1);
                    if (!database.data.ContainsKey(script.LegionModelNumber))
                    {
                        database.data.Add(script.LegionModelNumber, script);
                        Debug.Log("<color=yellow>" + script.LegionModelNumber.ToString() + "</color> has been <color=lime>Joined</color>.");
                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(script), assetName);
                    }
                    else
                    {
                        if (database.data[script.LegionModelNumber].name != assetName)
                        {
                            database.data[script.LegionModelNumber] = script;
                            Debug.Log("<color=yellow>" + script.LegionModelNumber.ToString() + "</color> has been <color=cyan>Updated</color>.");
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(script), assetName);
                        }
                    }
                    database.Sort();
                    EditorUtility.SetDirty(script);
                    EditorUtility.SetDirty(database);
                    AssetDatabase.SaveAssets();
                }
            }
            DrawDefaultInspector();
        }
    }
}