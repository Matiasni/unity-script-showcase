using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Basic Resource", menuName = "ScriptableObjects/Basic Resource", order = 1)]
public class GameResource : ScriptableObject, IReward
{
    public string id;
    public int amount { get; set; }

    public string rewardID => id;

    public virtual void SetValues(int amount)
    {
        this.amount = amount;
    }

    public void GiveReward(IRewardReceiver receiver, string source)
    {
        receiver.ReceiveReward(this, amount, source);
    }
}