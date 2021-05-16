using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Warfare
{
    public class ArmyManager : MonoBehaviour
    {
        // public Legion.Database legionDatabase;
        // public Unit.Database unitDatabase;
        // public PlayerData playerData;


        // // public List<unit>
        // public ArmyData nowArmy;
        // public List<Button> listReadyUnit = new List<Button>();

        // [Header("Ready")]
        // public Transform readyListGroup;
        // public GameObject prefabUnitButton;
        // public Warfare.Legion.Squadron modelSelection;

        // // Start is called before the first frame update
        // void Start()
        // {

        //     // Database 的資料 用於遊戲初始化或 模板生成
        //     // foreach (KeyValuePair<int, Legion.Data> legion in legionDatabase.legions.ToList())
        //     // {
        //     //     Legion.Squadron[] squadrons = legion.Value.GetSquadrons();
        //     //     for (int i = 0; i < 13; i++)
        //     //     {
        //     //         playerData.squadrons.Add(legion.Value.m_index * 100 + i, squadrons[i]);
        //     //     }
        //     // }
        //     // 來自存檔的資料

        // }
        // public Legion.Squadron InitializeSquadron()
        // {

        //  }

        // Model to Button List
        // public void RegisterReserveUnit(Warfare.Legion.Squadron unit)
        // {
        //     Button btn = Instantiate(prefabUnitButton, readyListGroup).GetComponent<Button>();
        //     listReadyUnit.Add(btn);
        //     btn.onClick.AddListener(() =>
        //    {
        //        modelSelection = unit;
        //    });
        //     btn.transform.localScale = Vector3.one;
        //     // unit.squadron = 0;
        // }

        // Update is called once per frame
        // void Update()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     { // if left button pressed...
        //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //         RaycastHit hit;
        //         if (Physics.Raycast(ray, out hit))
        //         {
        //             Warfare.GridManager grid = hit.transform.GetComponent<Warfare.GridManager>();
        //             if (grid)
        //             {
        //                 if (grid.unit == null)
        //                     grid.Deploy(modelSelection);
        //                 else
        //                 {
        //                     // RegisterReserveUnit(grid.m_unit);
        //                     grid.Disarmament();
        //                 }
        //             }
        //             // the object identified by hit.transform was clicked
        //             // do whatever you want
        //             // Debug.Log(hit.transform.name);
        //         }
        //     }


        //     if (Input.GetKeyDown(KeyCode.S))
        //     {
        //         PlayerData data = new PlayerData();
        //         Warfare.Legion.Squadron squa = new Warfare.Legion.Squadron();
        //         squa.type = Warfare.Unit.Type.nWidia;
        //         squa.level = 999;
        //         data.squadrons.Add(1202, squa);


        //         BinaryFormatter bf = new BinaryFormatter();
        //         Stream s = File.Open(Application.dataPath + "/Save.wak", FileMode.Create);
        //         bf.Serialize(s, data);
        //         s.Close();
        //         Debug.Log("Save");
        //     }
        //     if (Input.GetKeyDown(KeyCode.L))
        //     {


        //         BinaryFormatter bf = new BinaryFormatter();
        //         Stream s = File.Open(Application.dataPath + "/Save.wak", FileMode.Open);
        //         PlayerData data = (PlayerData)bf.Deserialize(s);
        //         Debug.Log("Load");

        //         Debug.Log(data.squadrons.Count);
        //         Debug.Log(data.squadrons[1202].level);

        //     }



        // }
    }
}