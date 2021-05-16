﻿using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Warfare.Legion
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Model))]
    public class ModelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var scripts = targets.OfType<Model>();
            if (GUILayout.Button("Join Database"))
            {
                foreach (var script in scripts)
                {
                    script.SetIndex();
                    Database database = AssetDatabase.LoadAssetAtPath<Database>("Assets/_iLYuSha_Mod/Base/Warfare/Legion/Database.asset");
                    string assetName = "[" + script.m_index.ToString() + "]" + script.m_faction.ToString() + " Legion " + (script.m_legion + 1);
                    if (!database.legions.ContainsKey(script.m_index))
                    {
                        database.legions.Add(script.m_index, script);
                        Debug.Log("<color=yellow>" + script.m_index.ToString() + "</color> has been <color=lime>Joined</color>.");
                        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(script), assetName);
                    }
                    else
                    {
                        if (database.legions[script.m_index].name != assetName)
                        {
                            database.legions[script.m_index] = script;
                            Debug.Log("<color=yellow>" + script.m_index.ToString() + "</color> has been <color=cyan>Updated</color>.");
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(script), assetName);
                        }
                    }
                    EditorUtility.SetDirty(script);
                    EditorUtility.SetDirty(database);
                    AssetDatabase.SaveAssets();
                }
            }
            DrawDefaultInspector();
        }
    }
}