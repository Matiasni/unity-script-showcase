using System;

public static class GameResourceEvents
{
    public static event Action<GameResource> OnGainGameSourceEvent;
    public static event Action<GameResource> OnRemoveGameSourceEvent;
    public static event Action<GameResource> OnGainCurrencyEvent;
    public static event Action<GameResource> OnRemoveCurrencyEvent;

    public static void OnGainGameResource(GameResource gameResource)
    {
        OnGainGameSourceEvent?.Invoke(gameResource);
    }

    public static void OnRemoveGameResource(GameResource gameResource)
    {
        OnRemoveGameSourceEvent?.Invoke(gameResource);
    }

    public static void OnGainCurrency(GameResource gameResource)
    {
        OnGainCurrencyEvent?.Invoke(gameResource);
    }

    public static void OnRemoveCurrency(GameResource gameResource)
    {
        OnRemoveCurrencyEvent?.Invoke(gameResource);
    }
}