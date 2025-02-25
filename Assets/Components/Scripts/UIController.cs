using System.Collections.Generic;

using UnityEngine;

public class UIController : MonoBehaviour {

    [Header("Controllers")]
    [SerializeField] private RoomClickController roomClickController;
    [SerializeField] private CameraController cameraController;

    [Header("UI Panels")]
    [SerializeField] public GameObject buildPanel;

    [Header("Build Panels")]

    private Transform targetTransform;

    public void OpenPanel(Transform target) {
        roomClickController.controlsEnabled = false;
        cameraController.controlsEnabled = false;
        buildPanel.SetActive(true);
        targetTransform = target;
    }
    public void ClosePanel() => StartCoroutine(roomClickController.MoveCameraBack());
    public void RestoreCameraControl() {
        roomClickController.controlsEnabled = true;
        cameraController.controlsEnabled = true;
        buildPanel.SetActive(false);
    }
}
