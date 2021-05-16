using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Warfare.Legion
{
    [CreateAssetMenu(fileName = "Warfare Legion Database", menuName = "Warfare/Legion/Create Warfare Legion Database")]
    public class Database : ScriptableObject, ISerializationCallbackReceiver
    {
        public WarfareManager warfare;
        [HideInInspector]
        public List<int> keyList = new List<int>();
        [HideInInspector]
        public List<Legion.Model> valueList = new List<Legion.Model>();
        public Dictionary<int, Legion.Model> legions = new Dictionary<int, Legion.Model>();

        public void DeleteKey(int key)
        {
            legions.Remove(key);
        }
        public void Sort()
        {
            Dictionary<int, Legion.Model> dic1Asc = legions.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            legions = dic1Asc;
        }
        public void OnBeforeSerialize()
        {
            keyList.Clear();
            valueList.Clear();

            foreach (var pair in legions)
            {
                keyList.Add(pair.Key);
                valueList.Add(pair.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            legions.Clear();

            for (int i = 0; i < keyList.Count; ++i)
            {
                legions[keyList[i]] = valueList[i];
            }
        }

#if UNITY_EDITOR
        public void SaveDatabase()
        {
            Debug.Log("<color=yellow>Database has been updated!</color>");
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }
}