using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace Warfare
{
    [CreateAssetMenu(fileName = "Warfare Manager", menuName = "Warfare/Create Warfare Manager")]
    public class WarfareManager : ScriptableObject
    {
        [Header("Database")]
        public Legion.Database legionDB;
        public Unit.Database unitDB;
        [Header("Model")]
        public Dictionary<int, Unit.Model> unitsModel = new Dictionary<int, Unit.Model>();
        public Dictionary<int, Legion.Model> legionsModel = new Dictionary<int, Legion.Model>();
        [Header("Player")]
        public PlayerData playerData;
        public PlayerEntity playerEntity = new PlayerEntity();

        public void CreateModel()
        {
            unitsModel.Clear();
            int count = unitDB.data.Count;
            for (int index = 0; index < count; index++)
            {
                Unit.Model model = new Unit.Model(unitDB.valueList[index]);
                unitsModel.Add((int)unitDB.keyList[index], model);
            }
            Debug.Log("<color=yellow>" + unitsModel.Count + " unit models</color> has been <color=lime>created</color>.");

            legionsModel.Clear();
            count = legionDB.data.Count;
            for (int index = 0; index < count; index++)
            {
                Legion.Model model = new Legion.Model(legionDB.valueList[index]);
                legionsModel.Add(legionDB.keyList[index], model);
            }
            Debug.Log("<color=yellow>" + legionsModel.Count + " legion models</color> has been <color=lime>created</color>.");
        }
        public void CloneNewLegionFromModel(int id, int index)
        {
            if (!legionsModel.ContainsKey(index)) return;
            playerData.legions.Add(id, new Legion.Data());
            playerEntity.legions.Add(id, new Legion.Battle());
            List<Unit.Data> squadron = legionsModel[index].squadron;
            for (int slot = 0; slot < Slot.size; slot++)
            {
                int type = (int)squadron[slot].Type;
                if (type == 0) continue;
                Unit.Data data = squadron[slot].Clone();
                playerData.legions[id].squadron.Add(slot, data);
                playerEntity.legions[id].squadron.Add(slot, new Unit.Battle(unitsModel[type], data));
            }
            // Debug.Log("<color=yellow>" + id + " legion (model: " + index + ")</color> has been <color=lime>cloned</color>.");
        }
        public void GenerateReverseUnitFromModel()
        {
            playerData.reserve.Clear();
            playerEntity.reserve.Clear();
            foreach (KeyValuePair<int, Legion.Model> legion in legionsModel.ToList())
            {
                if (legion.Key < 99000) continue;
                List<Unit.Data> squadron = legion.Value.squadron;
                for (int slot = 0; slot < Slot.size; slot++)
                {
                    int type = (int)squadron[slot].Type;
                    if (type == 0) continue;
                    Unit.Data data = squadron[slot].Clone();
                    playerData.reserve.Add(data);
                    playerEntity.reserve.Add(new Unit.Battle(unitsModel[type], data));
                }
            }
        }

        #region Save
        public bool Save(int index)
        {
            ConvertLegionData();
            ConvertReserveData();
            string fileName = "Save" + index + ".wak";
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open(Application.dataPath + "/" + fileName, FileMode.Create);
            bf.Serialize(s, playerData);
            s.Close();
            Debug.Log("<color=cyan>Save</color> " + fileName + " <color=lime>completed</color>");
            return true;
        }
        public bool ConvertLegionData()
        {
            playerData.legions.Clear();
            List<int> keys = playerEntity.legions.Keys.ToList();
            for (int index = 0; index < keys.Count; index++)
            {
                playerData.legions.Add(keys[index], new Legion.Data());
                Dictionary<int, Unit.Battle> data = playerEntity.legions[keys[index]].squadron;
                for (int slot = 0; slot < Slot.size; slot++)
                {
                    if (data.ContainsKey(slot))
                        playerData.legions[keys[index]].squadron.Add(slot, data[slot].Data);
                }
            }
            return true;
        }
        public bool ConvertReserveData()
        {
            playerData.reserve.Clear();
            int count = playerEntity.reserve.Count;
            for (int index = 0; index < count; index++)
            {
                playerData.reserve.Add(playerEntity.reserve[index].Data);
            }
            return true;
        }
        #endregion
        #region Load
        public bool Load(int index)
        {
            string fileName = "Save" + index + ".wak";
            if (!System.IO.File.Exists(Application.dataPath + "/" + fileName))
            {
                Debug.LogWarning("Bug");
                return false;
                // System.IO.File.Create (Application.dataPath + "/" + fileName).Dispose ();
            }
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open(Application.dataPath + "/" + fileName, FileMode.Open);
            playerData = (PlayerData)bf.Deserialize(s);
            s.Close();
            Debug.Log("<color=cyan>Load</color> " + fileName + " <color=lime>completed</color>");
            playerData.Fix();
            GenerateLegionUnitFromData();
            GenerateReverseUnitFromData();
            return true;
        }
        public bool GenerateLegionUnitFromData()
        {
            playerEntity.legions.Clear();
            List<int> keys = playerData.legions.Keys.ToList();
            for (int index = 0; index < keys.Count; index++)
            {
                playerEntity.legions.Add(keys[index], new Legion.Battle());
                Dictionary<int, Unit.Data> data = playerData.legions[keys[index]].squadron; ;
                for (int slot = 0; slot < Slot.size; slot++)
                {
                    if (data.ContainsKey(slot))
                        playerEntity.legions[keys[index]].squadron.Add(slot, new Unit.Battle(unitsModel[data[slot].Type], data[slot]));
                }
            }
            return true;
        }
        public bool GenerateReverseUnitFromData()
        {
            playerEntity.reserve.Clear();
            int count = playerData.reserve.Count;
            for (int order = 0; order < count; order++)
            {
                Unit.Data data = playerData.reserve[order];
                playerEntity.reserve.Add(new Unit.Battle(unitsModel[data.Type], data));
            }
            return true;
        }
        #endregion
    }

    [System.Serializable]
    public class PlayerData
    {
        public Dictionary<int, Legion.Data> legions = new Dictionary<int, Legion.Data>();
        public List<Unit.Data> reserve = new List<Unit.Data>();

        public void Fix()
        {
            // 用於修復不同版本存檔 變數名詞不同
            //reserveNew = reserve
        }
    }
    public class PlayerEntity
    {
        public Dictionary<int, Legion.Battle> legions = new Dictionary<int, Legion.Battle>();
        public List<Unit.Battle> reserve = new List<Unit.Battle>();
    }

    public static class Naming
    {
        public static string Type(int type)
        {
            switch (type)
            {
                case 1001:
                    return "神偷机兵";
                case 1002:
                    return "邪恶机兵";
                case 1011:
                    return "红牛能量";
                case 1012:
                    return "魔爪能量";
                case 1021:
                    return "普鲸";
                case 1022:
                    return "凯尔鲸";
                case 1031:
                    return "纸飞机";
                case 1041:
                    return "咕咕鸡";
                case 1042:
                    return "紫燕";
                case 1051:
                    return "炮弹比尔";
                case 1052:
                    return "苏打炮弹";
                case 1061:
                    return "时光机MK.I";
                case 1062:
                    return "时光机MK.II";
                case 1071:
                    return "王牌狗屋";
                case 1081:
                    return "卡比之星";
                case 1091:
                    return "血之蝎";
                case 1092:
                    return "苍之蝎";
                case 1101:
                    return "恩威迪亚";
                case 1111:
                    return "快餐侠";
                case 1121:
                    return "圣诞驯鹿";
                case 1122:
                    return "极地驯鹿";
                case 1131:
                    return "北极星特快";
                case 1132:
                    return "养乐多快线";
                case 1141:
                    return "虚空飞鱼";
                case 1142:
                    return "化骨鱼";
                case 1151:
                    return "玩具独角兽";
                case 1161:
                    return "幽灵南瓜";
                case 1171:
                    return "赏金猎人";
                case 1181:
                    return "鹰纽特";
                case 1191:
                    return "新葡鲸";
                case 1192:
                    return "粉红葡鲸";
                case 1193:
                    return "朱红普鲸";
                case 1201:
                    return "捣蛋财神";
                case 2001:
                    return "小小兵戴夫";
                case 2002:
                    return "邪恶小小兵";
                case 2011:
                    return "维特尔";
                case 2012:
                    return "王尼玛";
                case 2021:
                    return "大毛";
                case 2022:
                    return "萌总";
                case 2031:
                    return "阿楞";
                case 2061:
                    return "哆啦啦";
                case 2062:
                    return "迷你哆啦啦";
                case 2091:
                    return "无面人";
                case 2101:
                    return "熊猫人张学友";
                case 2102:
                    return "熊猫人金馆长";
                case 2111:
                    return "阿痞";
                case 2112:
                    return "Awesom-O 4000";
                case 2131:
                    return "滑稽";
                case 2141:
                    return "海绵宝宝";
                case 2142:
                    return "粉红海绵宝宝";
                case 2151:
                    return "公主阿尼";
                case 2171:
                    return "凯子";
                case 2181:
                    return "印第安屎蛋";
                case 2182:
                    return "屎蛋";
                case 2191:
                    return "安格瑞";
            }
            return "";
        }
        public static string Range(Unit.Range range)
        {
            switch (range)
            {
                case Unit.Range.Near:
                    return "近程";
                case Unit.Range.Medium:
                    return "中程";
                case Unit.Range.Far:
                    return "远程";
            }
            return "";
        }
    }
    public static class Slot
    {
        public static readonly int size = 13;
    }
}