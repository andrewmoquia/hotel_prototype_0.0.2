using System.Collections;

using UnityEngine;

public class RoomClickController : MonoBehaviour {

    [Header("Camera Settings")]
    [SerializeField] public Camera mainCamera;
    [SerializeField] public CameraController cameraController;
    [SerializeField] public UIController uIController;
    public bool controlsEnabled = true;
    public float moveSpeed = 2f;
    public float zoomSize = 5f;
    public float targetXOffset = -3.05f;
    public float targetYOffset = 8.43f;
    public float targetZOffset = -10.84f;
    public float targetYRotation = -8.89f;
    public float targetXRotation = 39.21f;
    public float targetOrthographicSize = 14.67f;

    private bool isMoving = false;
    private Vector3 previousCameraPosition;
    private Quaternion previousCameraRotation;
    private Transform targetTransform;

    void Start() {
        if(mainCamera == null) mainCamera = Camera.main;
    }

    void Update() {
        if(!controlsEnabled) return;
        if(Input.GetMouseButtonUp(0) && !cameraController.isMoving) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit hit)) {
                Transform clickedObject = hit.transform;
                targetTransform = Helper.GetRootParent(clickedObject);
                if(!isMoving) {
                    previousCameraPosition = mainCamera.transform.position;
                    previousCameraRotation = mainCamera.transform.rotation;
                    StartCoroutine(MoveCamera(targetTransform));
                }
            }
        }

        if(Input.GetMouseButtonUp(0) && cameraController.isMoving) {
            cameraController.isMoving = false;
        }
    }

    IEnumerator MoveCamera(Transform target) {
        isMoving = true;
        Vector3 targetPosition = new(target.position.x + targetXOffset, target.position.y + targetYOffset, targetZOffset);
        Quaternion targetRotation = Quaternion.Euler(
            targetXRotation,
            targetYRotation,
            target.rotation.eulerAngles.z
        );

        float elapsedTime = 0f;
        float duration = 1.5f;
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        while(elapsedTime < duration) {
            mainCamera.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                elapsedTime / duration
            );
            mainCamera.transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
        mainCamera.orthographicSize = targetOrthographicSize;
        isMoving = false;
        uIController.OpenPanel(targetTransform);
    }

    public IEnumerator MoveCameraBack() {
        isMoving = true;
        float elapsedTime = 0f;
        float duration = 1.5f;

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        while(elapsedTime < duration) {
            mainCamera.transform.position = Vector3.Lerp(
                startPosition,
                previousCameraPosition,
                elapsedTime / duration
            );
            mainCamera.transform.rotation = Quaternion.Slerp(
                startRotation,
                previousCameraRotation,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        mainCamera.transform.position = previousCameraPosition;
        mainCamera.transform.rotation = previousCameraRotation;
        isMoving = false;
        uIController.RestoreCameraControl();
    }
}
