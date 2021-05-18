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
        public Unit.BattleModel unitSelected;

        RectTransform rectTransform;
        GridLayoutGroup gridLayout;
        Scrollbar scrollbar;

        void Awake()
        {
            gridLayout = reserveGroup.GetComponent<GridLayoutGroup>();
            rectTransform = reserveGroup.GetComponent<RectTransform>();
            scrollbar = reserveGroup.parent.GetComponentInChildren<Scrollbar>();
            warfare.InitializeUnitProperty();
            // warfare.SynchronizeLegionsToPlayerData();
            // warfare.SynchronizeUnitsToPlayerData();
            // warfare.ConverseLegionBattleModel();
            // warfare.ConverseUnitsBattleModel();
        }

        public void Start()
        {
            CreateLegionUnit();
            CreateReserveUnit();
        }

        void Update()
        {
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
                                    warfare.listReserveUnit.Remove(unitSelected);
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
                            warfare.listReserveUnit.Add(grid.unit);
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
                Dictionary<int, Unit.BattleModel> squadron = warfare.legions[id].squadron;
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

            count = warfare.listReserveUnit.Count;
            for (int i = 0; i < count; i++)
            {
                RegisterReserveUnit(warfare.listReserveUnit[i]);
            }
            ResetReserveGroup();
        }
        public void RegisterReserveUnit(Unit.BattleModel unit)
        {
            Toggle btn = Instantiate(prefabUnitButton, reserveGroup).GetComponent<Toggle>();
            btn.gameObject.name = unit.model.Type.ToString();
            btn.group = btn.GetComponentInParent<ToggleGroup>();
            btn.GetComponent<Image>().sprite = unit.model.Sprite;
            btn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = unit.model.Field == Unit.Field.Dubi ? "x " + unit.UnitCount().ToString() : unit.data.HP.ToString();
            listReserveUnits.Add(btn);
            btn.onValueChanged.AddListener(isOn =>
           {
               if (isOn)
               {
                   unitSelected = unit;
                   btnSelected = btn;

                   grids[0].avatar.sprite = unit.model.Sprite;
                   grids[0].textType.text = Naming.Type(unit.data.Type);
                   grids[0].textFire.text = unit.model.FireRate.ToString();
                   grids[0].textRange.text = Naming.Range(unit.model.Range);
                   grids[0].textHP.text = unit.data.HP.ToString();
                   grids[0].textCount.text = unit.UnitCount().ToString();
                   grids[0].textDubi.text = (unit.UnitCount() * unit.model.ATK[0]).ToString();
                   grids[0].textMech.text = (unit.UnitCount() * unit.model.ATK[1]).ToString();
                   grids[0].textAir.text = (unit.UnitCount() * unit.model.ATK[2]).ToString();
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