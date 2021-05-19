using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Warfare.Legion
{
    public class Manager : MonoBehaviour
    {
        public WarfareManager warfare;
        public GridManager[] grids;
        public int id = 1000;

        [HeaderAttribute("UI Setting")]
        public Transform reserveGroup;
        public GameObject prefabUnitButton;
        public List<Toggle> listReserveUnits = new List<Toggle>();
        public Toggle btnSelected;
        public Unit.Battle unitSelected;

        RectTransform rectTransform;
        GridLayoutGroup gridLayout;
        Scrollbar scrollbar;

        void Awake()
        {
            gridLayout = reserveGroup.GetComponent<GridLayoutGroup>();
            rectTransform = reserveGroup.GetComponent<RectTransform>();
            scrollbar = reserveGroup.parent.GetComponentInChildren<Scrollbar>();
            // Debug.LogWarning(warfare.unit.Count);
            warfare.CreateModel();
            // warfare.GenerateReverseUnitFromModel();
            for (int i = 0; i < 20; i++)
            {
                warfare.CloneNewLegionFromModel(1000 + i, 10000 + i);
            }

            Debug.Log("N1 " + warfare.legions[1000].squadron[2].order);

            // warfare.CreateModel();
            // warfare.GenerateLegionFromModel(10000, 1000);
            // for (int i = 0; i < 20; i++)
            // {
            //     warfare.GenerateLegionFromModel(10000 + i, 1000 + i);
            // }
            // warfare.GenerateReverseUnitFromDB();


            // warfare.SynchronizeLegionsToPlayerData();
            // warfare.SynchronizeUnitsToPlayerData();
            // warfare.ConverseLegionBattleModel();
            // warfare.ConverseUnitsBattleModel();
        }

        public void Start()
        {
            CreateLegionUnit();
            CreateReserveUnit();
            Debug.Log("N2 " + warfare.legions[1000].squadron[2].order);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                warfare.legions[1000].squadron[0].Data.Exp = 5000;
                warfare.legions[1000].squadron[0].Model.Price = 9000;
                Debug.Log("Change " + warfare.legions[1000].squadron[0].Model.Type.ToString());
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Show " + warfare.legions[1000].squadron[0].Model.Type.ToString());
                Debug.Log("Ori Data Exp " + warfare.legions[1000].squadron[0].Data.Exp);
                Debug.Log("Ori Model Price" + warfare.legions[1000].squadron[0].Model.Price);
                Debug.Log("Show " + warfare.legions[1000].squadron[3].Model.Type.ToString());
                Debug.Log("new Data Exp " + warfare.legions[1000].squadron[3].Data.Exp);
                Debug.Log("new Model Price " + warfare.legions[1000].squadron[3].Model.Price);

                Debug.Log("DB " + ((Unit.Type)warfare.legionsModel[10000].squadron[0].Type).ToString());
                Debug.Log("DB Data Exp " + warfare.legionsModel[10000].squadron[0].Exp);

                Debug.Log("Player " + ((Unit.Type)warfare.playerData.legions[1000].data[0].Type).ToString());
                Debug.Log("Player Data Exp " + warfare.playerData.legions[1000].data[0].Exp);

            }



            // if (Input.GetKeyDown(KeyCode.Y))
            // {
            //     warfare.reserveUnits[0].Data.Exp = 5000;
            //     warfare.reserveUnits[0].Model.Price = 9000;
            //     Debug.Log("Change " + warfare.reserveUnits[0].Model.Type.ToString());
            // }
            // if (Input.GetKeyDown(KeyCode.O))
            // {
            //     Debug.Log("Show " + warfare.reserveUnits[0].Model.Type.ToString());
            //     Debug.Log("Ori Data Exp " + warfare.reserveUnits[0].Data.Exp);
            //     Debug.Log("Ori Model Price" + warfare.reserveUnits[0].Model.Price);
            //     Debug.Log("Show " + warfare.reserveUnits[3].Model.Type.ToString());
            //     Debug.Log("new Data Exp " + warfare.reserveUnits[3].Data.Exp);
            //     Debug.Log("new Model Price " + warfare.reserveUnits[3].Model.Price);

            //     Debug.Log("DB " + ((Unit.Type)warfare.legion[99000].Squadron[0].Type).ToString());
            //     Debug.Log("DB Data Exp " + warfare.legion[99000].Squadron[0].Exp);

            //     Debug.Log("Player " + ((Unit.Type)warfare.playerData.reserve[0].Type).ToString());
            //     Debug.Log("Player Data Exp " + warfare.playerData.reserve[0].Exp);

            // }

            for (int k = 0; k < 10; k++)
            {
                if (Input.GetKeyDown((KeyCode)(k + 48)) || Input.GetKeyDown((KeyCode)(k + 256)))
                {
                    id = 1000 + k;
                    CreateLegionUnit();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Warfare.GridManager grid = hit.transform.GetComponent<Warfare.GridManager>();
                    if (grid)
                    {
                        if (grid.unit == null)
                        {
                            if (btnSelected != null && unitSelected != null)
                            {
                                if (grid.Deploy(unitSelected))
                                {
                                    warfare.reserve.Remove(unitSelected);
                                    warfare.legions[id].squadron.Add(grid.Order, unitSelected);
                                    // warfare.playerData.units.Remove (unitSelected.data);
                                    // warfare.playerData.legions[id].squadron.Add (grid.Order, unitSelected.data);
                                    listReserveUnits.Remove(btnSelected);
                                    int index = btnSelected.transform.GetSiblingIndex();
                                    DestroyImmediate(btnSelected.gameObject);
                                    if (reserveGroup.GetComponentsInChildren<Toggle>().Length > 0)
                                    {
                                        if (reserveGroup.GetComponentsInChildren<Toggle>().Length > index)
                                            btnSelected = reserveGroup.GetComponentsInChildren<Toggle>()[index];
                                        else
                                            btnSelected = reserveGroup.GetComponentsInChildren<Toggle>()[index - 1];
                                        btnSelected.isOn = true;
                                    }
                                    else
                                    {
                                        btnSelected = null;
                                        unitSelected = null;
                                    }
                                    ResetReserveGroup();
                                }
                            }
                        }
                        else
                        {
                            warfare.reserve.Add(grid.unit);
                            // warfare.playerData.units.Add (grid.unit.data);
                            RegisterReserveUnit(grid.unit);
                            ResetReserveGroup();
                            warfare.legions[id].squadron.Remove(grid.Order);
                            // warfare.playerData.legions[id].squadron.Remove (grid.Order);
                            grid.Disarmament();
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.F3))
                warfare.Save(0);
            if (Input.GetKeyDown(KeyCode.F4))
            {
                if (warfare.Load(0))
                {
                    CreateLegionUnit();
                    CreateReserveUnit();
                }
            }
        }

        // Legion Unit Grid
        public void CreateLegionUnit()
        {
            if (warfare.legions.ContainsKey(id))
            {
                Dictionary<int, Unit.Battle> squadron = warfare.legions[id].squadron;
                for (int order = 0; order < 13; order++)
                {
                    grids[order].Disarmament();
                    grids[order].Manage(order);
                    if (squadron.ContainsKey(order))
                        grids[order].Deploy(squadron[order]);
                }
            }
            else
            {
                for (int order = 0; order < 13; order++)
                {
                    grids[order].Disarmament();
                    grids[order].Disable(order);
                }
            }
        }
        // Reserve Unit Bar
        public void CreateReserveUnit()
        {
            int count = listReserveUnits.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(listReserveUnits[i].gameObject);
            }
            listReserveUnits.Clear();

            count = warfare.reserve.Count;
            for (int i = 0; i < count; i++)
            {
                RegisterReserveUnit(warfare.reserve[i]);
            }
            ResetReserveGroup();
        }
        public void RegisterReserveUnit(Unit.Battle unit)
        {
            Toggle btn = Instantiate(prefabUnitButton, reserveGroup).GetComponent<Toggle>();
            btn.gameObject.name = unit.Model.Type.ToString();
            btn.group = btn.GetComponentInParent<ToggleGroup>();
            btn.GetComponent<Image>().sprite = unit.Model.Sprite;
            btn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = unit.Model.Field == Unit.Field.Dubi ? "x " + unit.UnitCount().ToString() : unit.Data.HP.ToString();
            listReserveUnits.Add(btn);
            btn.onValueChanged.AddListener(isOn =>
           {
               if (isOn)
               {
                   unitSelected = unit;
                   btnSelected = btn;

                   grids[0].avatar.sprite = unit.Model.Sprite;
                   grids[0].textType.text = Naming.Type(unit.Data.Type);
                   grids[0].textFire.text = unit.Model.FireRate.ToString();
                   grids[0].textRange.text = Naming.Range(unit.Model.Range);
                   grids[0].textHP.text = unit.Data.HP.ToString();
                   grids[0].textCount.text = unit.UnitCount().ToString();
                   grids[0].textDubi.text = (unit.UnitCount() * unit.Model.ATK[0]).ToString();
                   grids[0].textMech.text = (unit.UnitCount() * unit.Model.ATK[1]).ToString();
                   grids[0].textAir.text = (unit.UnitCount() * unit.Model.ATK[2]).ToString();
               }
           });
            btn.transform.localScale = Vector3.one;
            btn.transform.SetSiblingIndex(0);
            btn.isOn = true;
            scrollbar.value = 0;
        }
        void ResetReserveGroup()
        {
            int count = listReserveUnits.Count;
            rectTransform.sizeDelta = new Vector2(gridLayout.cellSize.x * count + gridLayout.spacing.x * (count - 1), rectTransform.sizeDelta.y);
        }
    }
}