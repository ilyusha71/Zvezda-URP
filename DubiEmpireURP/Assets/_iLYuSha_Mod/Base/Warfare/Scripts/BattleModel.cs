namespace Warfare
{
    [System.Serializable]
    public class BattleModel
    {
        public Legion.BattleModel[] legions = new Legion.BattleModel[2];
        public GridManager[] grids;
        public bool quickBattle = false, isFinish = false;
        public int wave = 0, maxWave = 5, action = 0, maxAction = 30;

        public BattleModel (Legion.BattleModel rightSide, Legion.BattleModel leftSide)
        {
            legions[0] = rightSide;
            legions[1] = leftSide;
            QuickBattle ();
        }
        public BattleModel (Legion.BattleModel rightSide, Legion.BattleModel leftSide, GridManager[] grids)
        {
            legions[0] = rightSide;
            legions[1] = leftSide;
            this.grids = grids;
        }
        void QuickBattle ()
        {
            quickBattle = true;
            for (wave = 1; wave <= maxWave; wave++)
            {
                for (action = 0; action < maxAction; action += 0)
                {
                    Rearrange ();
                    Fire ();
                    ActionResult ();
                    if (isFinish)
                        return;
                }
            }
        }
        public void Rearrange ()
        {
            for (int side = 0; side < 2; side++)
            {
                legions[side].UpdateRangeList (wave);
            }
            action++;
        }
        public void Fire ()
        {
            for (int side = 0; side < 2; side++)
            {
                for (int order = 0; order < 13; order++)
                {
                    if (legions[side].squadron.ContainsKey (order))
                        if (legions[side].squadron[order].Fire (action, legions[1 - side].rangeList) && !quickBattle)
                            grids[side * 17 + order].Fire ();
                }
            }
        }
        public bool ActionResult ()
        {
            for (int side = 0; side < 2; side++)
            {
                for (int order = 0; order < 13; order++)
                {
                    if (legions[side].squadron.ContainsKey (order))
                    {
                        Unit.BattleModel unit = legions[side].squadron[order];
                        // Debug.LogWarning(wave + " / " + action + " / " + side + " / " + order);
                        if (unit.ActionResult ())
                        {
                            if (!quickBattle)
                                grids[side * 17 + order].Hit ();
                            unit.Clear ();
                            if (unit.Data.HP == 0)
                            {
                                legions[side].squadron.Remove (order);
                                if (!quickBattle)
                                    grids[side * 17 + order].Disable (order);
                            }
                        }
                    }
                }
                if (legions[side].squadron.Count == 0)
                {
                    isFinish = true;
                    return true;
                }
            }
            return false;
        }
    }
}