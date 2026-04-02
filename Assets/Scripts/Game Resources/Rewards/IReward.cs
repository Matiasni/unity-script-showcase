public interface IReward
{
    public string rewardID { get; }
    public int amount { get; set; }
    public void GiveReward(IRewardReceiver receiver, string source);
}

public interface IRewardGiver
{

}

public interface IRewardReceiver
{
    public void ReceiveReward(IReward reward, int amount, string source);
}