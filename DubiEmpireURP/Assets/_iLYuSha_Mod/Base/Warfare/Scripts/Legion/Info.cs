using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Warfare.Legion
{
    [CreateAssetMenu(fileName = "Data", menuName = "Warfare/Legion/Create Data")]
    public class Info : ScriptableObject
    {
        public int m_legion;

        public Faction m_faction;
        public Type m_type;
        public Squadron[] m_squadron = new Squadron[Slot.size];
        // public int m_index; // Warning! This is legion model number (5 digits), not legion number (4 digits) in game.
        public int LegionModelNumber { get; private set; }

#if UNITY_EDITOR
        public void JoinDatabase()
        {
            LegionModelNumber = (int)m_faction * 1000 + (int)m_type;
            for (int i = 0; i < m_squadron.Length; i++)
            {
                m_squadron[i].Rebuild();
            }
        }
#endif
    }
    public class Model
    {
        public List<Unit.Data> squadron = new List<Unit.Data>();
        public Model(Info info)
        {
            for (int slot = 0; slot < Slot.size; slot++)
            {
                Unit.Data unit = new Unit.Data(info.m_squadron[slot]);
                squadron.Add(unit);
            }
        }
        // Option
        // public void Clone(Dictionary<int, Unit.Model> unitsModel, out Data oData, out Battle oBattle) 
        // {
        //     oData = new Data(new Dictionary<int, Unit.Data>());
        //     oBattle = new Battle(new Dictionary<int, Unit.Battle>());
        //     for (int order = 0; order < Slot.size; order++)
        //     {
        //         int type = (int)squadron[order].Type;
        //         if (type == 0) continue;
        //         Unit.Data data = squadron[order].Clone();
        //         Unit.Battle model = new Unit.Battle(unitsModel[type], data);
        //         oData.data.Add(order, data);
        //         oBattle.squadron.Add(order, model);
        //     }
        // }
    }
    [System.Serializable]
    public class Data
    {
        public Dictionary<int, Unit.Data> squadron = new Dictionary<int, Unit.Data>();
        public Data() { }
        // public void Convert2Battle(Dictionary<int, Unit.Model> unitsModel, out Battle oBattle)
        // {
        //     oBattle = new Battle(new Dictionary<int, Unit.Battle>());
        //     for (int order = 0; order < Slot.size; order++)
        //     {
        //         if (!data.ContainsKey(order)) continue;
        //         oBattle.squadron.Add(order, new Unit.Battle(unitsModel[data[order].Type], data[order]));
        //     }
        // }
    }
    public class Entity<T>
    {
        public Dictionary<int, T> squadron = new Dictionary<int, T>();
        public Entity() { }
        public Entity(Dictionary<int, T> t)
        {
            squadron = t;
        }
    }
    public class Battle : Entity<Unit.Battle>
    {
        public List<Unit.Battle>[] rangeList = new List<Unit.Battle>[5];
        public Battle() { }
        public Battle(Dictionary<int, Unit.Battle> t)
        {
            this.squadron = t;
        }
        public Battle Clone()
        {
            Battle newModel = new Battle(squadron);
            return newModel;
        }

        public void UpdateRangeList(int wave)
        {
            // Range list for enermy fire
            // 0 = short range
            // 1 = medium range
            // 2 = long range
            for (int i = 0; i < rangeList.Length; i++)
            {
                rangeList[i] = new List<Unit.Battle>();
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
    [System.Serializable]
    public class Squadron
    {
        public Texture m_texture;
        public Unit.Type type;
        public Unit.Info info;
        [Range(0f, 1f)]
        public float m_percent = 1.0f;
        public int Stack
        {
            get { return type == 0 ? 1 : Mathf.Max(1, Mathf.CeilToInt(info.m_formation.Length * m_percent)); }
            set { m_percent = type == 0 ? 0 : Mathf.Clamp01((float)value / info.m_formation.Length); }
        }
        public int HP
        {
            get { return Stack * info.m_hp; }
        }
        [Range(1, 100)]
        public int level;
        public int Level
        {
            get { return level; }
            set { level = Mathf.Clamp(value, 1, 100); }
        }

#if UNITY_EDITOR
        public void Rebuild()
        {
            if (m_texture)
            {
                type = (Unit.Type)int.Parse(m_texture.name.Split(new char[2] { '[', ']' })[1]);
                info = AssetDatabase.LoadAssetAtPath<Unit.Database>("Assets/_iLYuSha_Mod/Base/Warfare/Data/Unit/Database.asset").data[type];
            }
            else
            {
                type = Unit.Type.None;
                info = null;
            }
        }
#endif
    }
    public enum Faction
    {
        Experimental = 0,
        Anfail = 10,
        NO1 = 11,
        NO2 = 12,
        NO3 = 13,
        NO4 = 14,
        NO5 = 15,
        KingdomA = 41,
        KingdomB = 42,
        KingdomC = 43,
        KingdomD = 44,
        Reserve = 99,
    }
    public enum Type
    {
        Scout = 0, // 偵查
        Light = 1, // 輕型
        Mech = 2, // 機械
        Aviation = 3, // 航空
        Light2 = 11, // 宣戰時期 輕型
        Mech2 = 12, // 宣戰時期 機械
        Aviation2 = 13, // 宣戰時期 航空
        Capital = 101, // 宣戰時期 主力軍
        Reserve = 102, // 宣戰時期  預備
    }
}