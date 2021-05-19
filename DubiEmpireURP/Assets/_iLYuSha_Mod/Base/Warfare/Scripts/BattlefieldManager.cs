using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Warfare
{
    public class BattlefieldManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent onStart;
        [HeaderAttribute("Main")]
        public WarfareManager warfare;
        public GridManager[] grids;

        [HeaderAttribute("Battle")]
        public BattleModel battle;
        private int orderSelected;
        private List<Unit.Battle> targetList;
        float nextActionRearrangeTime;
        float nextActionFireTime;
        float nextActionResultTime;
        public enum BattleState
        {
            Deploy = 0,
            Window = 9,
            Ready = 10,
            Aim = 11,
            Fighting = 20,
            Finish = 30,
        }
        BattleState state = BattleState.Deploy;

        [HeaderAttribute("UI")]
        public AudioClip clipClick;
        private AudioSource audioSource;
        public GameObject windowTips, windowFight;
        public Text textTips;
        public Button btnOK, btnFight;
        public TextMeshProUGUI textWave;
        public TextMeshProUGUI textAction;

        void Awake()
        {
            windowTips.SetActive(false);
            warfare.CreateModel();
            warfare.SynchronizeLegionsToPlayerData();
            warfare.SynchronizeUnitsToPlayerData();
            warfare.ConverseLegionBattleModel();
            warfare.ConverseUnitsBattleModel();
            // warfare.Load (3);
            windowTips.SetActive(false);
            windowFight.SetActive(false);
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            onStart.Invoke();
            battle = new BattleModel(warfare.legions[2], warfare.legions[3], grids);
            FormUp();
        }

        bool FormUp()
        {
            for (int side = 0; side < 2; side++)
            {
                for (int order = 0; order < 17; order++)
                {
                    int index = side * 17 + order;
                    if (state == BattleState.Deploy)
                        grids[index].Disarmament();
                    if (battle.legions[side].squadron.ContainsKey(order))
                    {
                        grids[index].Ready(side + 100, order);
                        if (state == BattleState.Deploy)
                            grids[index].Deploy(battle.legions[side].squadron[order]);
                    }
                    else
                        grids[index].Disable(order);
                }
                battle.legions[side].UpdateRangeList(battle.wave);
            }
            textWave.text = "第" + (++battle.wave) + "波";
            textAction.text = (battle.action = 0).ToString();
            state = BattleState.Window;
            windowTips.SetActive(true);
            btnOK.Select();
            textTips.text = "第" + battle.wave + "波會戰即將開始，請下命令吧！";
            return true;
        }
        public void CloseWindow()
        {
            state = BattleState.Ready;
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire2") && state == BattleState.Ready)
            {
                state = BattleState.Window;
                windowFight.SetActive(true);
                btnFight.Select();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                windowTips.SetActive(true);
                btnOK.Select();
                textTips.text = "第" + battle.wave + "波會戰即將開始，請下命令吧！";
            }
            if (Input.GetKeyDown(KeyCode.F3))
                warfare.Save(3);
            if (Input.GetKeyDown(KeyCode.B) && state == BattleState.Ready)
                Fight();
            // if (Input.GetKeyDown (KeyCode.F5))
            //     Initialize (new int[] { 2, 3 }, true);
            // if (Input.GetKeyDown (KeyCode.F6))
            //     Initialize (new int[] { 2, 3 }, false);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Warfare.GridManager grid = hit.transform.GetComponent<Warfare.GridManager>();
                    if (grid)
                    {
                        if (state == BattleState.Ready && grid.state == GridState.Friend)
                        {
                            audioSource.PlayOneShot(clipClick);
                            orderSelected = grid.Order;
                            if (grid.Order > 10)
                                targetList = battle.legions[1].rangeList[3];
                            else if (grid.Order > 8)
                                targetList = battle.legions[1].rangeList[4];
                            else
                                targetList = battle.legions[1].rangeList[(int)grid.unit.Model.Range];

                            int count = targetList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                grids[17 + targetList[i].order].Aim();
                            }
                            state = BattleState.Aim;
                        }
                        else if (state == BattleState.Aim && grid.isTarget)
                        {
                            audioSource.PlayOneShot(clipClick);
                            battle.legions[0].squadron[orderSelected].target = battle.legions[1].squadron[grid.Order];
                            int count = targetList.Count;
                            for (int i = 0; i < count; i++)
                            {
                                grids[17 + targetList[i].order].Ready();
                            }
                            state = BattleState.Ready;
                        }
                    }
                }
            }

            if (state == BattleState.Fighting)
            {
                textAction.text = battle.action.ToString();
                if (Time.time > nextActionRearrangeTime)
                {
                    nextActionRearrangeTime = Time.time + 1f;
                    battle.Rearrange();
                }
                if (Time.time > nextActionFireTime)
                {
                    nextActionFireTime = Time.time + 1f;
                    battle.Fire();
                }
                if (Time.time > nextActionResultTime)
                {
                    nextActionResultTime = Time.time + 1f;
                    battle.ActionResult();
                    if (battle.action == battle.maxAction)
                        FormUp();
                }
            }
        }

        public void Fight()
        {
            state = BattleState.Fighting;
            nextActionRearrangeTime = Time.time + 0.9f;
            nextActionFireTime = Time.time + 1f;
            nextActionResultTime = Time.time + 1.2f;
            for (int side = 0; side < 2; side++)
            {
                for (int order = 0; order < 17; order++)
                {
                    if (battle.legions[side].squadron.ContainsKey(order))
                        grids[side * 17 + order].Battle();
                }
            }
        }
    }
}