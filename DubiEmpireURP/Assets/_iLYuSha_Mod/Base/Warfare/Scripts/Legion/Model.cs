using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Warfare.Legion
{
    [CreateAssetMenu(fileName = "Data", menuName = "Warfare/Legion/Create Warfare Legion Data")]
    public class Model : ScriptableObject
    {
        public int m_index;
        public Faction m_faction;
        public int m_legion;
        public SquadronInspector[] m_squadron = new SquadronInspector[13];

        public void SetIndex()
        {
            m_index = (int)m_faction * 100 + m_legion;
            for (int i = 0; i < m_squadron.Length; i++)
            {
                m_squadron[i].SetUnit();
            }
        }
        // public Squadron[] GetSquadrons()
        // {
        //     Squadron[] squadrons = new Squadron[13];
        //     for (int i = 0; i < squadrons.Length; i++)
        //     {
        //         squadrons[i].model.Type = m_squadron[i].type;
        //         squadrons[i].hp = m_squadron[i].Stack * m_squadron[i].data.model.m_hp;
        //     }
        //     return squadrons;
        // }
    }

    [System.Serializable]
    public class SquadronInspector
    {
        public Texture m_texture;
        public Unit.Type type;
        public Unit.Property data;
        [Range(0f, 1f)]
        public float m_percent = 1.0f;

        public void SetUnit()
        {
            if (m_texture)
            {
                type = (Unit.Type)int.Parse(m_texture.name.Split(new char[2] { '[', ']' })[1]);
                data = AssetDatabase.LoadAssetAtPath<Unit.Database>("Assets/_iLYuSha_Mod/Base/Warfare/Unit/Database.asset").units[type];
            }
            else
            {
                type = Unit.Type.None;
                data = null;
            }

        }
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
                        break;
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

            // Debug.LogWarning(" ----------------------------");
            // for (int i = 0; i < 5; i++)
            // {
            //     int c = rangeList[i].Count;
            //     Debug.LogWarning("Range " + i);
            //     for (int k = 0; k < c; k++)
            //     {
            //         Debug.Log(rangeList[i][k].order);

            //     }
            // }
        }
    }
    [System.Serializable]
    public class Squadron
    {
        public Unit.Model model;
        public int hp, level, exp;

        public int UnitCount
        {
            get { return Mathf.CeilToInt((float)hp / model.HP); }
        }
        public int TotalDamage(Unit.Field field)
        {
            return UnitCount * model.ATK[(int)field];
        }
    }
}