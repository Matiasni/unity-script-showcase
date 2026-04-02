using UnityEngine.Events;

public class RewardEvent : IReward
{
    public UnityEvent OnGranted;

    public string rewardID { get; }
    public int amount { get; set; }

    public void GiveReward(IRewardReceiver receiver, string source)
    {
        OnGranted?.Invoke();
    }
}