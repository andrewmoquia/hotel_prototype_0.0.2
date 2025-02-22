using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaAdjuster : MonoBehaviour {
    private RectTransform rectTransform;
    private Rect lastSafeArea;

    void Awake() {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update() {
        // Check for screen rotation or changes and reapply
        if(Screen.safeArea != lastSafeArea) {
            ApplySafeArea();
        }
    }

    void ApplySafeArea() {
        Rect safeArea = Screen.safeArea;
        lastSafeArea = safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
