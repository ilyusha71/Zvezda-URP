using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Warfare
{
    public class Database<Tkey, TValue> : ScriptableObject, ISerializationCallbackReceiver
    {
        // [HideInInspector]
        public List<Tkey> keyList = new List<Tkey>();
        // [HideInInspector]
        public List<TValue> valueList = new List<TValue>();
        public Dictionary<Tkey, TValue> data = new Dictionary<Tkey, TValue>();

        public void DeleteKey(Tkey key)
        {
            data.Remove(key);
        }
        public void Sort()
        {
            Dictionary<Tkey, TValue> dic1Asc = data.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            data = dic1Asc;
        }
        public void OnBeforeSerialize()
        {
            keyList.Clear();
            valueList.Clear();

            foreach (var pair in data)
            {
                keyList.Add(pair.Key);
                valueList.Add(pair.Value);
            }
        }
        public void OnAfterDeserialize()
        {
            data.Clear();

            for (int i = 0; i < keyList.Count; ++i)
            {
                data[keyList[i]] = valueList[i];
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