using UnityEngine;

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
}
