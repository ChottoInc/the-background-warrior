using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBaitHandler : AbstractConsumableHandler<BaitSO>
{
    protected override bool UseItem(BaitSO bait)
    {
        // get buff data
        PlayerBuffsData buffData = PlayerManager.Instance.PlayerBuffsData;

        // check which bait is being used to get the correct buff type
        UtilsBuffs.BuffType activatingBuff;
        switch (bait.AttractsMoment)
        {
            default:
            case UtilsGeneral.DayMoment.Morning: activatingBuff = UtilsBuffs.BuffType.MorningAngler; break;
            case UtilsGeneral.DayMoment.Afternoon: activatingBuff = UtilsBuffs.BuffType.AfternoonAngler; break;
            case UtilsGeneral.DayMoment.Night: activatingBuff = UtilsBuffs.BuffType.NightAngler; break;
        }

        // create buff
        Buff anglerBuff = new Buff(activatingBuff, bait.Duration);

        // check if already active, in that case add duration, else remove other bait effect and active new one
        if (buffData.HasBuff(anglerBuff))
        {
            buffData.AddBuff(anglerBuff);
        }
        else
        {
            var morningAnglerBuff = buffData.GetBuffByType(UtilsBuffs.BuffType.MorningAngler);
            if(morningAnglerBuff != null)
            {
                buffData.RemoveBuff(morningAnglerBuff);
            }

            var afternoonAnglerBuff = buffData.GetBuffByType(UtilsBuffs.BuffType.AfternoonAngler);
            if (afternoonAnglerBuff != null)
            {
                buffData.RemoveBuff(afternoonAnglerBuff);
            }

            var nightAnglerBuff = buffData.GetBuffByType(UtilsBuffs.BuffType.NightAngler);
            if (nightAnglerBuff != null)
            {
                buffData.RemoveBuff(nightAnglerBuff);
            }

            buffData.AddBuff(anglerBuff);
            PlayerManager.Instance.PlayerFisherData.SetActiveBait(bait);
            PlayerManager.Instance.SaveFisherData();
        }

        UtilsAchievement.AddStat(UtilsAchievement.STAT_BAITS_CONSUMED, 1, UtilsAchievement.STAT_BAITS_CONSUMED_MAX);

        return true;
    }
}
