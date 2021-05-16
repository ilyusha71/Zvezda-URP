using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Warfare.Legion
{
    public class DatabaseEditor : EditorWindow
    {
        private Legion.Model source;
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
                GUILayout.Label(database.legions.Count.ToString(), GUILayout.Width(50));

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
            foreach (KeyValuePair<int, Legion.Model> legion in database.legions.ToList())
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
                    switch (legion.Key % 10)
                    {
                        case 0:
                            GUILayout.Label((legion.Value.m_legion + 1).ToString() + "st Legion", GUILayout.Width(90));
                            break;
                        case 1:
                            GUILayout.Label((legion.Value.m_legion + 1).ToString() + "nd Legion", GUILayout.Width(90));
                            break;
                        case 2:
                            GUILayout.Label((legion.Value.m_legion + 1).ToString() + "rd Legion", GUILayout.Width(90));
                            break;
                        default:
                            GUILayout.Label((legion.Value.m_legion + 1).ToString() + "th Legion", GUILayout.Width(90));
                            break;
                    }
                    GUI.contentColor = Color.white;
                }
                GUILayout.EndHorizontal();

                EditorGUI.BeginChangeCheck();
                Undo.RecordObject(legion.Value, "Modify Types");
                GUILayout.BeginHorizontal();
                {
                    source = EditorGUILayout.ObjectField(legion.Value, typeof(Legion.Model), true, GUILayout.Width(200)) as Legion.Model;

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
                legion.Value.m_squadron[0].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[0].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[1].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[1].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[2].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[2].Stack, GUILayout.Width(100));
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
                legion.Value.m_squadron[9].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[9].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[10].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[10].Stack, GUILayout.Width(100));
                GUILayout.Space(30);
                legion.Value.m_squadron[3].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[3].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[4].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[4].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[5].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[5].Stack, GUILayout.Width(100));
                GUILayout.Space(30);
                legion.Value.m_squadron[11].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[11].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[12].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[12].Stack, GUILayout.Width(100));
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
                legion.Value.m_squadron[6].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[6].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[7].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[7].Stack, GUILayout.Width(100));
                legion.Value.m_squadron[8].Stack = EditorGUILayout.IntField("x", legion.Value.m_squadron[8].Stack, GUILayout.Width(100));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck())
                {
                    for (int i = 0; i < 13; i++)
                    {
                        legion.Value.m_squadron[i].SetUnit();
                        // if (legion.Value.m_squadron[i].m_texture)

                        // else
                        //     legion.Value.m_squadron[i].type = Unit.Type.None;
                    }
                    EditorUtility.SetDirty(legion.Value);
                    EditorUtility.SetDirty(database);
                }
            }
            GUILayout.Space(15);
        }
    }
}