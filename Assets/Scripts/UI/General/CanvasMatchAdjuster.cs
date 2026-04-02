using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasMatchAdjuster : MonoBehaviour
{
    public float thresholdAspect = 1.5f;
    private CanvasScaler scaler;

    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        AdjustMatch();
    }

    void AdjustMatch()
    {
        float aspect = (float)Screen.width / Screen.height;

        if (aspect < thresholdAspect)
            scaler.matchWidthOrHeight = 0f;
        else
            scaler.matchWidthOrHeight = 0.5f;
    }
}