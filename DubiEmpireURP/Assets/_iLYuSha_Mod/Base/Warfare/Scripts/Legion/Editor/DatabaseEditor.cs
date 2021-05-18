using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Warfare.Legion
{
    public class DatabaseEditor : EditorWindow
    {
        private Legion.Info source;
        private Vector2 scrollPos;
        private Editor editor;
        private Database database;

        [MenuItem("Warfare/Legion Database #F6")]
        public static void ShowDatabaseWindow()
        {
            var window = EditorWindow.GetWindow<DatabaseEditor>(false, "Legion Database", true);
            window.database = UnityEditor.AssetDatabase.LoadAssetAtPath<Database>("Assets/_iLYuSha_Mod/Base/Warfare/Data/Legion/Database.asset");
            window.editor = Editor.CreateEditor(window.database);
        }
        public void OnGUI()
        {
            if (!editor)
                ShowDatabaseWindow();
            EditorGUILayout.BeginVertical(GUILayout.MinHeight(position.height));
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawDatabaseInspector();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        void DrawDatabaseInspector()
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            GUI.skin.label.fontSize = 21;
            GUILayout.Label("Warfare Legion Database");

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            {
                GUI.skin.label.alignment = TextAnchor.MiddleLeft;
                GUI.skin.label.fontStyle = FontStyle.Normal;
                GUI.skin.label.fontSize = 18;
                GUILayout.Label("Total: ", GUILayout.Width(55));

                GUI.skin.label.fontStyle = FontStyle.Bold;
                GUI.contentColor = Color.cyan;
                GUILayout.Label(database.data.Count.ToString(), GUILayout.Width(50));

                GUI.backgroundColor = Color.red;
                GUI.contentColor = Color.white;
                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(database, "Modify Types");
                if (GUILayout.Button("Sort", GUILayout.Height(25), GUILayout.Width(66)))
                    database.Sort();
                GUI.backgroundColor = Color.white;
                if (EditorGUI.EndChangeCheck())
                    EditorUtility.SetDirty(database);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUIUtility.labelWidth = 10;
            foreach (KeyValuePair<int, Legion.Info> legion in database.data.ToList())
            {
                // Faction Title
                if (legion.Key % 10 == 0)
                {
                    GUILayout.Space(20);
                    GUILayout.BeginHorizontal();
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.skin.label.fontSize = 21;
                    GUILayout.Label(legion.Value.m_faction.ToString());
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                {
                    GUI.skin.label.fontSize = 16;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                    GUI.contentColor = Color.green;
                    GUILayout.Label(legion.Value.m_faction.ToString() + " " + legion.Value.m_type.ToString() + " Legion", GUILayout.Width(500));
                    GUI.contentColor = Color.white;
                }
                GUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(legion.Value, "Modify Types");
                GUILayout.BeginHorizontal();
                {
                    source = EditorGUILayout.ObjectField(legion.Value, typeof(Legion.Info), true, GUILayout.Width(200)) as Legion.Info;

                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove", GUILayout.Width(66)))
                    {
                        Debug.Log("<color=yellow>" + legion.Key.ToString() + "</color> has been <color=#fdb4ca>removed.</color>");
                        Undo.RecordObject(database, "Modify Types");
                        database.DeleteKey(legion.Key);
                    }
                    GUI.backgroundColor = Color.white;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                legion.Value.m_squadron[0].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[0].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[1].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[1].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[2].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[2].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int order = 0; order < 3; order++)
                {
                    EditorGUILayout.LabelField("Lv.", GUILayout.Width(18));
                    legion.Value.m_squadron[order].Level = EditorGUILayout.IntField(legion.Value.m_squadron[order].Level, GUILayout.Width(30));
                    legion.Value.m_squadron[order].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[order].Stack, GUILayout.Width(46));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                legion.Value.m_squadron[9].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[9].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[10].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[10].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                GUILayout.Space(30);
                legion.Value.m_squadron[3].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[3].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[4].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[4].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[5].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[5].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                GUILayout.Space(30);
                legion.Value.m_squadron[11].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[11].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[12].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[12].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int order = 9; order <= 10; order++)
                {
                    EditorGUILayout.LabelField("Lv.", GUILayout.Width(18));
                    legion.Value.m_squadron[order].Level = EditorGUILayout.IntField(legion.Value.m_squadron[order].Level, GUILayout.Width(30));
                    legion.Value.m_squadron[order].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[order].Stack, GUILayout.Width(46));
                }
                GUILayout.Space(30);
                for (int order = 3; order <= 5; order++)
                {
                    EditorGUILayout.LabelField("Lv.", GUILayout.Width(18));
                    legion.Value.m_squadron[order].Level = EditorGUILayout.IntField(legion.Value.m_squadron[order].Level, GUILayout.Width(30));
                    legion.Value.m_squadron[order].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[order].Stack, GUILayout.Width(46));
                }
                GUILayout.Space(30);
                for (int order = 11; order <= 12; order++)
                {
                    EditorGUILayout.LabelField("Lv.", GUILayout.Width(18));
                    legion.Value.m_squadron[order].Level = EditorGUILayout.IntField(legion.Value.m_squadron[order].Level, GUILayout.Width(30));
                    legion.Value.m_squadron[order].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[order].Stack, GUILayout.Width(46));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(15);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                legion.Value.m_squadron[6].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[6].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[7].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[7].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                legion.Value.m_squadron[8].m_texture = EditorGUILayout.ObjectField(legion.Value.m_squadron[8].m_texture, typeof(Texture), true, GUILayout.Height(100), GUILayout.Width(100)) as Texture;
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int order = 6; order <= 8; order++)
                {
                    EditorGUILayout.LabelField("Lv.", GUILayout.Width(18));
                    legion.Value.m_squadron[order].Level = EditorGUILayout.IntField(legion.Value.m_squadron[order].Level, GUILayout.Width(30));
                    legion.Value.m_squadron[order].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[order].Stack, GUILayout.Width(46));
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                {
                    for (int i = 0; i < 13; i++)
                    {
                        legion.Value.m_squadron[i].Rebuild();
                    }
                    EditorUtility.SetDirty(legion.Value);
                    EditorUtility.SetDirty(database);
                }
            }
            GUILayout.Space(15);
        }
    }
}