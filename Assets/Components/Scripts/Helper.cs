using UnityEngine;
using UnityEngine.UI;

public static class Helper {
    public static Transform FindParentWithTag(Transform child, string tag) {
        while(child != null) {
            if(child.CompareTag(tag))
                return child;
            child = child.parent; // Move up the hierarchy
        }
        return null;
    }

    public static string GetRootParentTag(Transform child) {
        while(child.parent != null) // Traverse up to the topmost parent
        {
            child = child.parent;
        }
        return child.tag; // Return the tag of the topmost parent
    }

    public static Transform GetRootParent(Transform child) {
        while(child.parent != null) // Traverse up to the topmost parent
        {
            child = child.parent;
        }
        return child; // Return the topmost parent
    }

    public static Toggle CreateToggle(GameObject prefab, Transform panel) {
        GameObject toggleObj = Object.Instantiate(prefab, panel);
        Toggle toggle = toggleObj.GetComponent<Toggle>();
        return toggle;
    }

    public static Toggle CreateToggle(GameObject prefab, Transform panel, Sprite icon) {
        GameObject toggleObj = Object.Instantiate(prefab, panel);
        Toggle toggle = toggleObj.GetComponent<Toggle>();

        Transform iconTransform = toggleObj.transform.Find("Icon");
        if(iconTransform != null) {
            if(iconTransform.TryGetComponent<Image>(out var iconImage)) iconImage.sprite = icon;
        }
        else Debug.LogWarning("Icon image not found inside Toggle prefab.");
        return toggle;
    }
}
