using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseQuest
{
    [HideInInspector] public int ID;

    public string name;

    public enum types
    {
        GATHER,
        KILLTARGETS,
        BOOLEAN
    }
    public types type;

    public int level;

    public string description;

    public List<BaseQuestGatherRequirement> gatherReqs = new List<BaseQuestGatherRequirement>();
    public List<BaseQuestKillRequirement> killReqs = new List<BaseQuestKillRequirement>();
    public List<BaseQuestBoolRequirement> boolReqs = new List<BaseQuestBoolRequirement>();

    public int rewardGold;
    public int rewardExp;

    public List<BaseItemReward> rewardItems = new List<BaseItemReward>();

    public bool fulfilled;
}
