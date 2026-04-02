using UnityEngine;

[System.Serializable]
public class Reward : MonoBehaviour, IReward
{
    public string Type;
    public string Id;
    public string Name;
    public int amount { get; set; }

    public string rewardID => Id;
    public void GiveReward(IRewardReceiver receiver, string source) { }
}