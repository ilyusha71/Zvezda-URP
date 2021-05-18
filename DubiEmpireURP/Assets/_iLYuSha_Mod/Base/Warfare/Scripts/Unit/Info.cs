using System.Collections.Generic;
using UnityEngine;

namespace Warfare.Unit
{
    [CreateAssetMenu (fileName = "Data", menuName = "Warfare/Unit/Create Data")]
    public class Info : ScriptableObject
    {
        public GameObject m_instance;
        public Sprite m_sprite;
        [HeaderAttribute ("Parameter")]
        public Type m_type;
        public int m_price;
        public int m_Hour;
        public int m_hp;
        public int m_atk;
        public int[] m_power = new int[3];
        public int m_fire;
        public Field m_field;
        public Anti m_anti;
        public Range m_range;
        public Square m_square;
        public Vector3[] m_formation;

        public void SetType ()
        {
            if (m_instance)
                m_type = (Type) int.Parse (m_instance.name.Split (new char[2] { '[', ']' }) [1]);
            else if (m_sprite)
                m_type = (Type) int.Parse (m_sprite.name.Split (new char[2] { '[', ']' }) [1]);
        }
        public void SetFormation ()
        {
            if (m_formation.Length == 0)
                m_formation = new Vector3[1] { Vector3.zero };
            if (m_square == Square.None) return;
            float height = 0;
            int side = (int) m_square;
            float offset = 12f / (side + 1f);
            m_formation = new Vector3[side * side];
            for (int i = 0; i < side; i++)
            {
                for (int j = 0; j < side; j++)
                {
                    m_formation[i * side + j] = new Vector3 (-6 + (j + 1) * offset, height, 6 - (i + 1) * offset);
                }
            }
        }
        public void SetPower ()
        {
            switch (m_anti)
            {
                case Anti.DubiLv1:
                    m_power[0] = (int) (m_atk * 1f); // 兵
                    m_power[1] = (int) (m_atk * 0.8f); // 甲
                    m_power[2] = (int) (m_atk * 0.5f); // 空
                    break;
                case Anti.DubiLv2:
                    m_power[0] = (int) (m_atk * 1.3f); // 兵
                    m_power[1] = (int) (m_atk * 0.7f); // 甲
                    m_power[2] = (int) (m_atk * 0.4f); // 空
                    break;
                case Anti.DubiLv3:
                    m_power[0] = (int) (m_atk * 1.7f); // 兵
                    m_power[1] = (int) (m_atk * 0.6f); // 甲
                    m_power[2] = (int) (m_atk * 0.3f); // 空
                    break;
                case Anti.DubiLv4:
                    m_power[0] = (int) (m_atk * 2.2f); // 兵
                    m_power[1] = (int) (m_atk * 0.4f); // 甲
                    m_power[2] = (int) (m_atk * 0.2f); // 空
                    break;
                case Anti.DubiLv5:
                    m_power[0] = (int) (m_atk * 2.8f); // 兵
                    m_power[1] = (int) (m_atk * 0.2f); // 甲
                    m_power[2] = (int) (m_atk * 0.1f); // 空
                    break;
                case Anti.DubiLv6:
                    m_power[0] = (int) (m_atk * 3.5f); // 兵
                    m_power[1] = (int) (m_atk * 0.1f); // 甲
                    m_power[2] = (int) (m_atk * 0.05f); // 空
                    break;
                case Anti.DubiLv7:
                    m_power[0] = (int) (m_atk * 5f); // 兵
                    m_power[1] = (int) (m_atk * 0f); // 甲
                    m_power[2] = (int) (m_atk * 0f); // 空
                    break;
                case Anti.MechLv1:
                    m_power[0] = (int) (m_atk * 0.8f); // 兵
                    m_power[1] = (int) (m_atk * 1f); // 甲
                    m_power[2] = (int) (m_atk * 0.5f); // 空
                    break;
                case Anti.MechLv2:
                    m_power[0] = (int) (m_atk * 0.7f); // 兵
                    m_power[1] = (int) (m_atk * 1.3f); // 甲
                    m_power[2] = (int) (m_atk * 0.4f); // 空
                    break;
                case Anti.MechLv3:
                    m_power[0] = (int) (m_atk * 0.6f); // 兵
                    m_power[1] = (int) (m_atk * 1.7f); // 甲
                    m_power[2] = (int) (m_atk * 0.3f); // 空
                    break;
                case Anti.MechLv4:
                    m_power[0] = (int) (m_atk * 0.4f); // 兵
                    m_power[1] = (int) (m_atk * 2.2f); // 甲
                    m_power[2] = (int) (m_atk * 0.2f); // 空
                    break;
                case Anti.MechLv5:
                    m_power[0] = (int) (m_atk * 0.2f); // 兵
                    m_power[1] = (int) (m_atk * 2.8f); // 甲
                    m_power[2] = (int) (m_atk * 0.1f); // 空
                    break;
                case Anti.MechLv6:
                    m_power[0] = (int) (m_atk * 0.1f); // 兵
                    m_power[1] = (int) (m_atk * 3.5f); // 甲
                    m_power[2] = (int) (m_atk * 0.05f); // 空
                    break;
                case Anti.MechLv7:
                    m_power[0] = (int) (m_atk * 0f); // 兵
                    m_power[1] = (int) (m_atk * 5f); // 甲
                    m_power[2] = (int) (m_atk * 0f); // 空
                    break;
                case Anti.AirLv1:
                    m_power[0] = (int) (m_atk * 0.8f); // 兵
                    m_power[1] = (int) (m_atk * 0.5f); // 甲
                    m_power[2] = (int) (m_atk * 1f); // 空
                    break;
                case Anti.AirLv2:
                    m_power[0] = (int) (m_atk * 0.7f); // 兵
                    m_power[1] = (int) (m_atk * 0.4f); // 甲
                    m_power[2] = (int) (m_atk * 1.3f); // 空
                    break;
                case Anti.AirLv3:
                    m_power[0] = (int) (m_atk * 0.6f); // 兵
                    m_power[1] = (int) (m_atk * 0.3f); // 甲
                    m_power[2] = (int) (m_atk * 1.7f); // 空
                    break;
                case Anti.AirLv4:
                    m_power[0] = (int) (m_atk * 0.4f); // 兵
                    m_power[1] = (int) (m_atk * 0.2f); // 甲
                    m_power[2] = (int) (m_atk * 2.2f); // 空
                    break;
                case Anti.AirLv5:
                    m_power[0] = (int) (m_atk * 0.2f); // 兵
                    m_power[1] = (int) (m_atk * 0.1f); // 甲
                    m_power[2] = (int) (m_atk * 2.8f); // 空
                    break;
                case Anti.AirLv6:
                    m_power[0] = (int) (m_atk * 0.1f); // 兵
                    m_power[1] = (int) (m_atk * 0.05f); // 甲
                    m_power[2] = (int) (m_atk * 3.5f); // 空
                    break;
                case Anti.AirLv7:
                    m_power[0] = (int) (m_atk * 0f); // 兵
                    m_power[1] = (int) (m_atk * 0f); // 甲
                    m_power[2] = (int) (m_atk * 5f); // 空
                    break;
                default:
                    m_power[0] = (int) (m_atk * 1f); // 兵
                    m_power[1] = (int) (m_atk * 1f); // 甲
                    m_power[2] = (int) (m_atk * 1f); // 空
                    break;
            }
        }
    }
     [System.Serializable]
    public class Model
    {
        public Model (Info info)
        {
            Instance = info.m_instance;
            Sprite = info.m_sprite;
            Type = info.m_type;
            Price = info.m_price;
            Hour = info.m_Hour;
            HP = info.m_hp;
            FireRate = info.m_fire;
            ATK = info.m_power;
            Field = info.m_field;
            Anti = info.m_anti;
            Range = info.m_range;
            Square = info.m_square;
            Formation = info.m_formation;
            Debug.Log(Type.ToString() + " ok!");
        }
        public GameObject Instance { get; private set; }
        public Sprite Sprite { get; private set; }
        public Type Type { get; private set; }
        public int Price { get; private set; }
        public int Hour { get; private set; }
        public int HP { get; private set; }
        public int FireRate { get; private set; }
        public int[] ATK { get; private set; }
        public Field Field { get; private set; }
        public Anti Anti { get; private set; }
        public Range Range { get; private set; }
        public Square Square { get; private set; }
        public Vector3[] Formation { get; private set; }
        public int UnitCount (float hp)
        {
            return Mathf.CeilToInt (hp / HP);
        }
    }
    [System.Serializable]
    public class Data
    {
        public int Type { get; set; }
        public int HP { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
    }
    public class DataModel
    {
        public Data data;
        public Model model;

        public DataModel () { }
        public DataModel (Model model, Data data)
        {
            this.model = model;
            this.data = data;
        }
        public int UnitCount ()
        {
            return model.UnitCount (data.HP);
        }
    }
    public class BattleModel : DataModel
    {
        public int order;
        public Unit.BattleModel target;
        public int totalFire, totalAttackers, totalDamage;
        public Range hitRange;
        public int countDestroy, countHit;

        public BattleModel (int order, Model model, Data data)
        {
            this.order = order;
            this.model = model;
            this.data = data;
        }
        public bool Fire (int action, List<Unit.BattleModel>[] rangeList)
        {
            if (action % model.FireRate != 0) return false;
            if (target != null)
                if (target.data.HP == 0) target = null;
            if (target == null)
            {
                if (order > 10)
                    target = rangeList[3][Random.Range (0, rangeList[3].Count)];
                else if (order > 8)
                    target = rangeList[4][Random.Range (0, rangeList[3].Count)];
                else
                {
                    List<Unit.BattleModel> list = rangeList[(int) model.Range];
                    target = list[Random.Range (0, list.Count)];
                }
            }
            target.hitRange = (Range) Mathf.Max ((int) target.hitRange, (int) model.Range);
            target.totalFire++;
            target.totalAttackers += model.UnitCount (data.HP);
            target.totalDamage += model.UnitCount (data.HP) * model.ATK[(int) target.model.Field];
            return true;
        }
        public bool ActionResult ()
        {
            int unitCount = UnitCount ();
            data.HP = Mathf.Max (0, data.HP - totalDamage);
            countDestroy = unitCount - UnitCount ();
            countHit = Mathf.Min (UnitCount (), totalAttackers - countDestroy);
            // Debug.Log(totalFire + " / " + totalAttackers + " / " + totalDamage + " / " + data.HP);
            return true;
        }
        public void Clear ()
        {
            totalFire = 0;
            totalAttackers = 0;
            totalDamage = 0;
            hitRange = Range.Near;
            countDestroy = 0;
            countHit = 0;
        }
    }
    public enum Type
    {
        None = 0,
        Mech = 1000,
        DespicableMech = 1001,
        EvilMech = 1002,
        Energy = 1010,
        RedBullEnergy = 1011,
        MonsterEnergy = 1012,
        Ceti = 1020,
        Putin = 1021,
        Kells = 1022,
        Aeroplane = 1030,
        PaperAeroplane = 1031,
        CuckooChicken = 1040,
        Cuckoo = 1041,
        Progne = 1042,
        Bullet = 1050,
        BulletBill = 1051,
        BulletSoda = 1052,
        TimeMachine = 1060,
        TimeMachineMK1 = 1061,
        TimeMachineMK2 = 1062,
        Kennel = 1070,
        AceKennel = 1071,
        Star = 1080,
        KirbyStar = 1081,
        Scopio = 1090,
        ScopioBlood = 1091,
        ScopioFirmament = 1092,
        nWidiaProto = 1100,
        nWidia = 1101,
        FastFood = 1110,
        FastFoodMan = 1111,
        Reindeer = 1120,
        XmasReindeer = 1121,
        ArcticReindeer = 1122,
        Express = 1130,
        PolarisExpress = 1131,
        YogurtExpress = 1132,
        AncientFish = 1140,
        VoidFish = 1141,
        FossilFish = 1142,
        Unicorn = 1150,
        PapoyUnicorn = 1151,
        Pumpkin = 1160,
        GhostPumpkin = 1161,
        Hunter = 1170,
        BoundyHunter = 1171,
        Inuit = 1180,
        InuitScout = 1181,
        Lisboa = 1190,
        GrandLisboa = 1191,
        PinkLisboa = 1192,
        ScarletLisboa = 1193,
        Piggy = 1200,
        PiggyCracker = 1201,
        Dave = 2001,
        EvilMinion = 2002,
        Vettel = 2011,
        WangNiMa = 2012,
        DaMao = 2021,
        MengZong = 2022,
        Dorara = 2061,
        MiniDorara = 2062,
        NoFace = 2091,
        PandamanZhang = 2101,
        PandamanJin = 2102,
        Eric = 2111,
        AwesomO4000 = 2112,
        HuaJi = 2131,
        SpongeBob = 2141,
        PinkSpongeBob = 2142,
        Kenny = 2151,
        Kyle = 2171,
        IndianStan = 2181,
        Stan = 2182,
        AngryMan = 2191,
    }
    public enum Square
    {
        None = 0,
        _3x3 = 3,
        _4x4 = 4,
        _5x5 = 5,
        _6x6 = 6,
        _7x7 = 7,
    }
    public enum Field
    {
        Dubi = 0,
        Mech = 1,
        Air = 2,
    }
    public enum Anti
    {
        Normal = 0,

        DubiLv1 = 101,
        DubiLv2 = 102,
        DubiLv3 = 103,
        DubiLv4 = 104,
        DubiLv5 = 105,
        DubiLv6 = 106,
        DubiLv7 = 107,

        MechLv1 = 201,
        MechLv2 = 202,
        MechLv3 = 203,
        MechLv4 = 204,
        MechLv5 = 205,
        MechLv6 = 206,
        MechLv7 = 207,

        AirLv1 = 301,
        AirLv2 = 302,
        AirLv3 = 303,
        AirLv4 = 304,
        AirLv5 = 305,
        AirLv6 = 306,
        AirLv7 = 307,
    }
    public enum Range
    {
        Near = 0,
        Medium = 1,
        Far = 2,
    }
}