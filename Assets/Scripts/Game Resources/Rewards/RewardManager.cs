using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RewardGameResource
{
    [SerializeField] public GameResource _item;

    public int amount = 1;
    public GameResource Item => _item;
}

public class RewardManager : MonoBehaviour
{
    public List<IReward> Rewards = new List<IReward>();

    public event Action<IReward> onAddReward;
    public event Action<IReward, IRewardReceiver, IRewardGiver> onGiveReward;
    public event Action<List<IReward>, IRewardReceiver, IRewardGiver> onGiveListRewards;
    public event Action<IReward, IRewardReceiver, IRewardGiver> onGiveSingleReward;

    public void AddReward(IReward reward)
    {
        Rewards.Add(reward);
        onAddReward?.Invoke(reward);
    }

    public void SetRewardList(List<IReward> rewardsToAdd)
    {
        Rewards = rewardsToAdd.ToList();
    }

    public void GiveReward(string rewardId, IRewardReceiver receiver, IRewardGiver giver, string source)
    {
        var reward = Rewards.Find(r => r.rewardID == rewardId);
        reward.GiveReward(receiver, source);
        onGiveReward?.Invoke(reward, receiver, giver);
        onGiveSingleReward?.Invoke(reward, receiver, giver);
    }

    public void GiveAllReward(IRewardReceiver receiver, IRewardGiver giver, string source)
    {
        onGiveListRewards?.Invoke(Rewards, receiver, giver);
        foreach (var reward in Rewards)
        {
            reward.GiveReward(receiver, source);
            onGiveReward?.Invoke(reward, receiver, giver);
        }
    }

    public void AddToGameResourcesList(List<RewardGameResource> currencies)
    {
        foreach (var resource in currencies)
        {
            var createdReward = resource.Item;
            createdReward.amount = resource.amount;

            Rewards.Add(createdReward);
        }
    }
}