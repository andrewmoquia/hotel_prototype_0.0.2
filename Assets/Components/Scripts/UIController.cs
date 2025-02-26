using System.Collections.Generic;

using UnityEngine;

public class UIController : MonoBehaviour {

    [Header("Controllers")]
    [SerializeField] private RoomClickController roomClickController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private BuildPanelController buildPanelController;

    private Transform targetTransform;

    public void OpenPanel(Transform target) {
        roomClickController.controlsEnabled = false;
        cameraController.controlsEnabled = false;
        targetTransform = target;
        switch(target.tag) {
            case "Apartment Room":
                buildPanelController.OpenPanel(target);
                break;
            default:
                break;
        }
    }
    public void ClosePanel() => StartCoroutine(roomClickController.MoveCameraBack());
    public void RestoreCameraControl() {
        roomClickController.controlsEnabled = true;
        cameraController.controlsEnabled = true;
        switch(targetTransform.tag) {
            case "Apartment Room":
                buildPanelController.ClosePanel();
                break;
            default:
                break;
        }
    }
}
