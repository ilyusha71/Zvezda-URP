using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Warfare.Faction
{
    [CreateAssetMenu(fileName = "Data", menuName = "Warfare/Faction/Create Data")]
    public class Model : ScriptableObject
    {
        public Type m_type;
        public int m_index;
        public string m_name;
        public int m_capital;
        public int[] m_towns;
        public Legion.Model[] m_legions;

#if UNITY_EDITOR
        public void SetIndex()
        {
            m_index = (int)m_type * 10;
        }
#endif
    }
    public enum Type
    {
        None = 0,
        Anfail = 1,
        Talmon = 2,
        Warlord = 3,
        Kingdom = 4,
        Pass = 5,
        Trading = 6,
        Academy = 7,
    }
}