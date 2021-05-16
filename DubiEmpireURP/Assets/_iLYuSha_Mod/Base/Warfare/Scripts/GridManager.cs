using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Warfare
{
    public class GridManager : MonoBehaviour
    {
        public GridState state;
        public int Order { get; private set; }

        [HeaderAttribute("Unit")]
        public Unit.BattleModel unit;
        public Dictionary<int, GameObject> stacks = new Dictionary<int, GameObject>();
        [HeaderAttribute("Battle")]
        public bool isTarget;
        public List<int> index = new List<int>(); // 無序
        public List<int> order = new List<int>(); // 有序
        [HeaderAttribute("Grid")]
        public AudioClip clipHover;
        public Color32 orange = new Color32(227, 79, 0, 255);
        public Color32 gray97 = new Color32(97, 97, 97, 255);
        private AudioSource audioSource;
        private Color32 enterColor;
        private Color32 exitColor;
        private MeshRenderer gridRender;
        private SpriteRenderer gridSprite;

        [HeaderAttribute("UI")]
        public Image avatar;
        public TextMeshProUGUI textType, textLevel, textExp, textHP, textFire, textRange, textCount, textDubi, textMech, textAir;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            textType.text = "";
            textHP.text = "";
            textCount.text = "";
            textDubi.text = "";
            textMech.text = "";
            textAir.text = "";

            gridRender = GetComponent<MeshRenderer>();
            gridSprite = GetComponentInChildren<SpriteRenderer>();
            enterColor = orange;
            if (state == GridState.Deploy)
                exitColor = gray97;
            else
            {
                gridRender.enabled = false;
                exitColor = Color.clear;

            }
            gridSprite.color = exitColor;
        }
        // void OnMouseDown ()
        // {
        //     // Debug.Log (transform.name);
        // }
        // void OnMouseUpAsButton ()
        // {
        //     // Debug.Log (transform.name + "--- up");

        // }
        void OnMouseEnter()
        {
            if (state == GridState.Disable) return;
            if (unit == null) return;
            if (unit.data.HP == 0)
                Debug.LogError("QQ");
            audioSource.PlayOneShot(clipHover);
            gridSprite.color = enterColor;
            avatar.sprite = unit.model.Sprite;
            textType.text = Property.Type(unit.data.Type);
            textFire.text = unit.model.FireRate.ToString();
            textRange.text = Property.Range(unit.model.Range);
            textHP.text = unit.data.HP.ToString();
            textCount.text = unit.UnitCount().ToString();
            textDubi.text = (unit.UnitCount() * unit.model.ATK[0]).ToString();
            textMech.text = (unit.UnitCount() * unit.model.ATK[1]).ToString();
            textAir.text = (unit.UnitCount() * unit.model.ATK[2]).ToString();
        }
        void OnMouseOver()
        {
            if (unit == null) return;
            if (unit.data.HP == 0)
                Debug.LogError("QQ");
            textHP.text = unit.data.HP.ToString();
            textCount.text = unit.UnitCount().ToString();
            textDubi.text = (unit.UnitCount() * unit.model.ATK[0]).ToString();
            textMech.text = (unit.UnitCount() * unit.model.ATK[1]).ToString();
            textAir.text = (unit.UnitCount() * unit.model.ATK[2]).ToString();
        }
        void OnMouseExit()
        {
            if (state == GridState.Disable) return;
            gridSprite.color = exitColor;
        }
        public void Manage(int order)
        {
            state = GridState.Deploy;
            Order = order;
            enterColor = orange;
            exitColor = gray97;
            gridRender.enabled = true;
            gridSprite.enabled = true;
            gridSprite.color = exitColor;
        }
        public void Ready(int side, int order)
        {
            state = (GridState)side;
            Order = order;
            enterColor = orange;
            exitColor = gray97;
            gridRender.enabled = true;
            gridSprite.color = exitColor;
        }
        public void Disable(int order)
        {
            state = GridState.Disable;
            Order = order;
            gridRender.enabled = false;
            gridSprite.enabled = false;
            unit = null;
        }
        public void Aim()
        {
            isTarget = true;
            enterColor = Color.red;
            exitColor = Color.red;
            gridSprite.color = exitColor;
        }
        public void Ready()
        {
            isTarget = false;
            enterColor = orange;
            exitColor = gray97;
            gridSprite.color = exitColor;
        }
        public void Battle()
        {
            gridRender.enabled = false;
            exitColor = Color.clear;
            gridSprite.color = exitColor;
        }
        public void Disarmament()
        {
            List<GameObject> list = stacks.Values.ToList();
            stacks.Clear();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                GameObject go = list[i];
                Destroy(go);
            }
            unit = null;
        }
        public bool Deploy(Unit.BattleModel unit)
        {
            if (state != GridState.Deploy)
            {
                gridRender.enabled = true;
                exitColor = gray97;
                gridSprite.color = exitColor;
            }
            this.unit = unit;
            unit.order = Order;
            int[] array = new int[unit.model.UnitCount(unit.data.HP)]; // 目前數量
            int[] array2 = new int[unit.model.Formation.Length]; // 最大數量
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = 1; // 取得權重，各位置權重相同
            }
            for (int j = 0; j < array.Length; j++)
            {
                int lotteryIndex = unit.model.Field == Unit.Field.Dubi ? GetLotteryIndex(array2) : j; // 只有Dubi要抽位置

                if (state == GridState.Foe)
                    stacks.Add(lotteryIndex, Instantiate(unit.model.Instance, transform.position + unit.model.Formation[unit.model.Formation.Length - 1 - lotteryIndex] * 1, Quaternion.Euler(0, 180, 0)));
                else
                    stacks.Add(lotteryIndex, Instantiate(unit.model.Instance, transform.position + unit.model.Formation[lotteryIndex] * 1, Quaternion.identity));
                array2[lotteryIndex] = 0; // 抽中後將權重改為0
            }
            if (state != GridState.Deploy)
                UpdateList();
            return true;
        }
        public static int GetLotteryIndex(int[] rates)
        {
            if (rates == null)
            {
                return -1;
            }
            int num = 0;
            for (int i = 0; i < rates.Length; i++)
            {
                num += rates[i];
            }
            int num2 = Random.Range(1, num + 1);
            for (int j = 0; j < rates.Length; j++)
            {
                num2 -= rates[j];
                if (num2 <= 0)
                {
                    return j;
                }
            }
            return rates.Length - 1;
        }
        void UpdateList()
        {
            index.Clear();
            order.Clear();
            Dictionary<int, GameObject> dic1Asc = stacks.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            index = stacks.Keys.ToList();
            order = dic1Asc.Keys.ToList();
        }

        public void Fire()
        {
            UpdateList();
            for (int i = 0; i < stacks.Count; i++)
            {
                // if (stacks[index[i]].GetComponentInChildren<EffectController>())
                stacks[index[i]].GetComponentInChildren<EffectController>().Fire();
                if (i < 2)
                    stacks[index[i]].GetComponentInChildren<EffectController>().FireSound();

            }
        }

        public void Hit()
        {
            UpdateList();
            List<int> listDestroy = new List<int>();
            for (int i = 0; i < unit.countDestroy; i++)
            {
                if (unit.hitRange == Unit.Range.Far)
                {
                    listDestroy.Add(index[0]);
                    index.RemoveAt(0);
                }
                else
                {
                    listDestroy.Add(order[0]);
                    order.RemoveAt(0);
                }
            }

            // 需要補Hit特效
            if (unit.hitRange == Unit.Range.Far)
            {
                for (int i = 0; i < unit.countHit; i++)
                {
                    stacks[index[i]].GetComponentInChildren<EffectController>().Hit();
                }
            }
            else
            {
                for (int i = 0; i < unit.countHit; i++)
                {
                    stacks[order[i]].GetComponentInChildren<EffectController>().Hit();
                }
            }
            // 陣亡
            for (int i = 0; i < listDestroy.Count; i++)
            {
                GameObject go = stacks[listDestroy[i]];
                stacks.Remove(listDestroy[i]);
                Destroy(go);
            }
        }
    }
    public enum Definition
    {
        Front = 0,
        Middle = 1,
        Back = 2,
        Left = 100,
        Right = 200
    }
    public enum GridState
    {
        Deploy = 0,
        Disable = -1,
        Friend = 100,
        Foe = 101,
    }
}