using UnityEngine;


[RequireComponent(typeof(RectTransform))]
public class UISafeArea : MonoBehaviour
{
    private RectTransform rectTransform; 
    private Rect safeArea;
    private Vector2 minAnchor; 
    private Vector2 maxAnchor; 

    private void Awake() { OnRectTransformDimensionsChange(); }
    void OnRectTransformDimensionsChange() { StartSafeArea(); }

    private void StartSafeArea() 
    { 
        rectTransform = GetComponent<RectTransform>(); SafeAreaCalculation(); 
    }
    private void SafeAreaCalculation() 
    { 
        safeArea = Screen.safeArea; 
        minAnchor = safeArea.position; 
        maxAnchor = minAnchor + safeArea.size; 
        minAnchor.x /= Screen.width; 
        minAnchor.y /= Screen.height; 
        maxAnchor.x /= Screen.width; 
        maxAnchor.y /= Screen.height; 
        
        if (Screen.orientation == ScreenOrientation.LandscapeRight) 
        { 
            rectTransform.anchorMin = new Vector2(0, 0); 
            rectTransform.anchorMax = new Vector2(1, 1); 
        } 
        else 
        { 
            rectTransform.anchorMin = new Vector2(0, 0); 
            rectTransform.anchorMax = new Vector2(1, maxAnchor.y); 
        } }

}