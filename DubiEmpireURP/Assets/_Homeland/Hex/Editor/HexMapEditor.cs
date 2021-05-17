using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Warfare.Legion
{
public class HexMapEditor : EditorWindow
{
    private HexMap database;

    [MenuItem("Warfare/HexMap #F9")]
    public static void ShowDatabaseWindow()
    {
        var window = EditorWindow.GetWindow<HexMapEditor>(false, "Hex Database", true);
        // window.database = UnityEditor.AssetDatabase.LoadAssetAtPath<Database>("Assets/_iLYuSha_Mod/Base/Warfare/Data/Legion/Database.asset");
        // window.editor = Editor.CreateEditor(window.database);
    }
    public void OnGUI()
    {
        // if (!editor)
        //     ShowDatabaseWindow();
        // EditorGUILayout.BeginVertical(GUILayout.MinHeight(position.height));
        // scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        // // this.editor.OnInspectorGUI();
        // DrawDatabaseInspector();
        // EditorGUILayout.EndScrollView();
        // EditorGUILayout.EndVertical();
    }
}

}

