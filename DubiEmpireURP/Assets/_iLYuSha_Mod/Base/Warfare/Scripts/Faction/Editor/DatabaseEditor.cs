using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Warfare.Faction
{
    public class DatabaseEditor : EditorWindow
    {
        private WarfareManager warfare;
        private Vector2 scrollPos;
        private Editor editor;
        private Database database;

        [MenuItem("Warfare/Faction Database #F4")]
        public static void ShowDatabaseWindow()
        {
            var window = EditorWindow.GetWindow<DatabaseEditor>(false, "Faction Database", true);
            window.warfare = UnityEditor.AssetDatabase.LoadAssetAtPath<WarfareManager>("Assets/_iLYuSha_Mod/Base/Warfare/Data/Warfare Manager.asset");
            window.database = UnityEditor.AssetDatabase.LoadAssetAtPath<Database>("Assets/_iLYuSha_Mod/Base/Warfare/Data/Faction/Database.asset");
            window.editor = Editor.CreateEditor(window.database);
        }
        public void OnGUI()
        {
            if (!editor)
                ShowDatabaseWindow();
            EditorGUILayout.BeginVertical(GUILayout.MinHeight(position.height));
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            // this.editor.OnInspectorGUI();
            DrawDatabaseInspector();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        void DrawDatabaseInspector()
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.label.fontSize = 21;
            GUILayout.Label("Faciton Database");

        }
    }
}