using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Warfare.Legion
{
    [CreateAssetMenu(fileName = "Data", menuName = "Warfare/Legion/Create Data")]
    public class Model : ScriptableObject
    {
        public int m_index;
        public Faction m_faction;
        public int m_legion;
        public SquadronInspector[] m_squadron = new SquadronInspector[13];

#if UNITY_EDITOR
        public void SetIndex()
        {
            m_index = (int)m_faction * 100 + m_legion;
            for (int i = 0; i < m_squadron.Length; i++)
            {
                m_squadron[i].SetUnit();
            }
        }
#endif
    }

    [System.Serializable]
    public class SquadronInspector
    {
        public Texture m_texture;
        public Unit.Type type;
        public Unit.Model data;
        [Range(0f, 1f)]
        public float m_percent = 1.0f;

#if UNITY_EDITOR
        public void SetUnit()
        {
            if (m_texture)
            {
                type = (Unit.Type)int.Parse(m_texture.name.Split(new char[2] { '[', ']' })[1]);
                data = AssetDatabase.LoadAssetAtPath<Unit.Database>("Assets/_iLYuSha_Mod/Base/Warfare/Data/Unit/Database.asset").data[type];
            }
            else
            {
                type = Unit.Type.None;
                data = null;
            }

        }
#endif
        public int Stack
        {
            get
            {
                return type == 0 ? 1 : Mathf.Max(1, Mathf.CeilToInt(data.m_formation.Length * m_percent));
            }
            set
            {
                m_percent = type == 0 ? 0 : Mathf.Clamp01((float)value / data.m_formation.Length);
            }
        }
        public int HP
        {
            get
            {
                return Stack * data.m_hp;
            }
        }
    }
    [System.Serializable]
    public class Data
    {
        public int Id { get; private set; }
        public Dictionary<int, Unit.Data> squadron = new Dictionary<int, Unit.Data>();

        public Data(int id)
        {
            Id = id;
        }
    }
    public class DataModel<T>
    {
        public Dictionary<int, T> squadron = new Dictionary<int, T>();
        public DataModel() { }
        public DataModel(Dictionary<int, T> t)
        {
            squadron = t;
        }
    }
    public class BattleModel : DataModel<Unit.BattleModel>
    {
        public List<Unit.BattleModel>[] rangeList = new List<Unit.BattleModel>[5];
        public BattleModel(Dictionary<int, Unit.BattleModel> t)
        {
            this.squadron = t;
        }
        public void Rearrange(int wave)
        {
            // Range list for enermy fire
            // 0 = short range
            // 1 = medium range
            // 2 = long range
            for (int i = 0; i < rangeList.Length; i++)
            {
                rangeList[i] = new List<Unit.BattleModel>();
                rangeList[i].Clear();
            }
            int range = 0;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (this.squadron.ContainsKey(row * 3 + column))
                    {
                        // 內部迴圈是針對同列的單位都能加入正確的射程列表
                        for (int order = 0; order < 3; order++)
                        {
                            int index = row * 3 + order;
                            if (this.squadron.ContainsKey(index))
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    if (range <= i)
                                        rangeList[i].Add(this.squadron[index]);
                                }
                            }
                        }
                        range++;
                        break; // 已經檢查完同列的單位直接跳出column迴圈
                    }
                }
            }
            for (int side = 0; side < 2; side++)
            {
                for (int order = 0; order < 2; order++)
                {
                    int index = 9 + side * 2 + order;
                    if (this.squadron.ContainsKey(index))
                        rangeList[side + 3].Add(this.squadron[index]);
                }
                if (wave > 1 && rangeList[side + 3].Count == 0)
                    rangeList[side + 3] = rangeList[0];
            }
        }
    }
    // [System.Serializable]
    // public class Squadron
    // {
    //     public Unit.Property model;
    //     public int hp, level, exp;

    //     public int UnitCount
    //     {
    //         get { return Mathf.CeilToInt((float)hp / model.HP); }
    //     }
    //     public int TotalDamage(Unit.Field field)
    //     {
    //         return UnitCount * model.ATK[(int)field];
    //     }
    // }

    public enum Faction
    {
        Experimental = 0,
        Wakaka = 10,
        NO1 = 11,
        NO2 = 12,
        NO3 = 13,
        NO4 = 14,
        NO5 = 15,
        Reserve = 99,
    }
}