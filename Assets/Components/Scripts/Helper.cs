using System.Collections.Generic;

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

    public static void InstantiateObject(Transform instantiateLocation, FurnitureData furniture, float XPositionOffset, float YPositionOffset, float ZPositionOffset) {
        if(instantiateLocation != null && instantiateLocation.childCount > 0) {
            List<GameObject> childrenToDestroy = new();

            foreach(Transform child in instantiateLocation) {
                childrenToDestroy.Add(child.gameObject);
            }

            foreach(GameObject obj in childrenToDestroy) {
                Object.DestroyImmediate(obj);
            }
        }

        // Cast the ray from above to detect the highest object below
        Vector3 startPosition = instantiateLocation.position + Vector3.up * 2f;
        float rayLength = 5f;

        RaycastHit[] hits = new RaycastHit[5];
        int hitCount = Physics.RaycastNonAlloc(startPosition, Vector3.down, hits, rayLength);

        float highestPoint = float.MinValue;
        Vector3 furniturePosition = instantiateLocation.position;

        for(int i = 0; i < hitCount; i++) {
            RaycastHit hit = hits[i];

            if(hit.collider.gameObject != instantiateLocation.gameObject) // Ignore self
            {
                float topSurface = hit.point.y;
                if(topSurface > highestPoint) {
                    highestPoint = topSurface;
                }
            }
        }

        // Adjust the position of the new object
        if(highestPoint != float.MinValue) {
            furniturePosition.y = highestPoint; // Align with detected surface
        }
        else {
            Debug.LogWarning("No object detected below! Defaulting to targetFurniture position.");
        }

        // Instantiate new object at the adjusted position
        GameObject newObject = Object.Instantiate(
            furniture.FurniturePrefab,
            furniturePosition,
            Quaternion.identity,
            instantiateLocation
        );

        newObject.name = furniture.Name;

        // Adjust position based on newObject's collider
        Collider objCollider = newObject.GetComponent<Collider>();
        if(objCollider != null) {
            float objectBottom = newObject.transform.position.y - objCollider.bounds.min.y;

            newObject.transform.position = new Vector3(
                furniturePosition.x + XPositionOffset,
                furniturePosition.y + objectBottom + YPositionOffset, // Adjust to sit perfectly
                furniturePosition.z + ZPositionOffset
            );
        }
        else {
            Debug.LogWarning("No collider found on newObject! It may not align properly.");
        }
    }
}
