using UnityEngine;

public class CameraController : MonoBehaviour {
    [Header("Camera Movement")]
    [SerializeField] private float moveSpeed = 50f; // Length of the ray
    [SerializeField] private float moveSmoothTime = 0.05f; // Length of the ray
    private Vector3? moveHitPoint = null; // Store the hit position
    private Vector3? moveHitNormal = null; // Store the hit normal
    public float dragSpeed = 0.1f; // Adjust for sensitivity
    private Vector2 dragOrigin;   // To store the initial touch position
    public bool isDragging = false;
    public bool isMoving = false;


    [Header("Camera Zoom")]
    [SerializeField] private float zoomSpeed = 5f; // Speed of zooming
    [SerializeField] private float minZoomHeight = 5f; // Minimum zoom height
    [SerializeField] private float maxZoomHeight = 50f; // Maximum zoom height
    [SerializeField] private float touchEndTime = 1.5f; // Time the touch started
    private float touchStartTime = 0f; // Time the touch started
    private bool isRayCreated = false; // Whether the ray is created
    private Ray zoomRay; // The created ray
    private bool isZooming = false;
    private Vector3 initialCameraPosition;
    private Vector3 initialZoomHitPoint;


    [Header("Camera Rotation")]
    [SerializeField] private float rayLength = 100f; // Length of the ray
    [SerializeField] private float rotationSpeed = 50f; // Speed of camera rotation
    private Vector3? hitPoint = null; // Store the hit position
    private Vector3? hitNormal = null; // Store the hit normal
    private bool isRotating = false; // Track if the camera is currently rotating


    public bool controlsEnabled = true;
    private Vector3 velocity = Vector3.zero;

    void Update() {
        if(!controlsEnabled) return;
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    void HandleZoom() {
        if(Input.touchCount == 1) {
            Touch touch0 = Input.GetTouch(0);

            // Detect if the touch has been held for 3 seconds
            if(touch0.phase == TouchPhase.Began) {
                touchStartTime = Time.time;
                isRayCreated = false;
            }
            else if(touch0.phase == TouchPhase.Stationary && !isRayCreated) {
                if(Time.time - touchStartTime >= touchEndTime) {
                    // Create the ray from the center of the screen
                    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    zoomRay = Camera.main.ScreenPointToRay(screenCenter);

                    if(Physics.Raycast(zoomRay, out RaycastHit hit)) {
                        // Store the initial hit point and the camera position
                        initialZoomHitPoint = hit.point;
                        initialCameraPosition = Camera.main.transform.position;
                        isRayCreated = true;
                    }
                }
            }
            else if(touch0.phase == TouchPhase.Moved && isRayCreated) {
                // Set isZooming to true when zooming starts
                isZooming = true;

                // Get the movement delta on the Y-axis
                float deltaY = touch0.deltaPosition.y;

                // Calculate the zoom factor based on Y-axis movement
                float zoomFactor = deltaY * zoomSpeed * Time.deltaTime;

                // Calculate the direction from the camera to the initial zoom hit point
                Vector3 directionToHitPoint = (initialZoomHitPoint - Camera.main.transform.position).normalized;

                // Calculate the potential new camera position based on zoom factor
                Vector3 potentialNewCameraPosition = Camera.main.transform.position + directionToHitPoint * zoomFactor;

                // Check if the new camera position is within the allowed height range
                if(potentialNewCameraPosition.y >= minZoomHeight && potentialNewCameraPosition.y <= maxZoomHeight) {
                    // Move the camera along the ray direction based on zoomFactor
                    Camera.main.transform.position = potentialNewCameraPosition;
                }

                // Clamp the height of the camera to ensure it stays within bounds
                Vector3 cameraPosition = Camera.main.transform.position;
                cameraPosition.y = Mathf.Clamp(cameraPosition.y, minZoomHeight, maxZoomHeight);
                Camera.main.transform.position = cameraPosition;
            }
        }

        // Stop zooming after the touch ends
        if(Input.touchCount == 0 || Input.GetTouch(0).phase == TouchPhase.Ended) {
            isZooming = false;
        }
    }

    void HandleMovement() {
        // Skip movement if zooming is in progress
        if(isZooming) return;

        if(Input.touchCount == 1) {
            Touch touch0 = Input.GetTouch(0);

            if(touch0.phase == TouchPhase.Began) {
                dragOrigin = touch0.position; // Store the initial touch position
                // isDragging = true;           // Enable dragging
                // Debug.Log($"isMoving: {isMoving}, isDragging: {isDragging}, dragOrigin: {dragOrigin}");
            }
            else if(touch0.phase == TouchPhase.Moved) {
                isMoving = true; // Enable moving

                // Calculate the difference in viewport coordinates
                Vector3 difference = Camera.main.ScreenToViewportPoint(touch0.position - dragOrigin);

                // Camera's right direction, constrained to the XZ plane
                Vector3 rightDirection = new Vector3(transform.right.x, 0, transform.right.z).normalized;

                // Camera's forward direction, constrained to the XZ plane
                Vector3 forwardDirection = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

                // Calculate movement in the XZ plane
                Vector3 move = (rightDirection * -difference.x + forwardDirection * -difference.y) * moveSpeed;

                // Apply movement using SmoothDamp
                transform.position += move;

                // Update drag origin to the current touch position
                dragOrigin = touch0.position;
            }
        }
    }

    void HandleRotation() {
        // Check if exactly 2 fingers are touching the screen
        if(Input.touchCount == 2) {
            // Get the touches
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Check for initial touch phase to detect hit point
            if(touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began) {
                // Get the center point of the screen
                Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

                // Create a ray from the camera's center
                Ray ray = Camera.main.ScreenPointToRay(screenCenter);

                // Perform a raycast
                if(Physics.Raycast(ray, out RaycastHit hit, rayLength)) {
                    hitPoint = hit.point; // Store the hit point
                    hitNormal = hit.normal;
                    isRotating = true; // Start rotating
                }
            }

            // Rotate the camera around the hit point
            if(isRotating && hitPoint.HasValue) {
                // Calculate the relative movement between the two fingers
                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                Vector2 touch0CurrentPos = touch0.position;
                Vector2 touch1CurrentPos = touch1.position;

                // Determine the direction of the rotation
                float prevTouchDelta = Vector2.SignedAngle(touch1PrevPos - touch0PrevPos, touch1PrevPos);
                float currentTouchDelta = Vector2.SignedAngle(touch1CurrentPos - touch0CurrentPos, touch1CurrentPos);

                float deltaAngle = currentTouchDelta - prevTouchDelta;

                // Rotate the camera around the hit point
                Camera.main.transform.RotateAround(hitPoint.Value, Vector3.up, deltaAngle * rotationSpeed * Time.deltaTime);
            }
        }
        else {
            // Reset rotation state if fingers are lifted
            isRotating = false;
            hitPoint = null;
        }
    }

    void OnDrawGizmos() {
        // Check if there's a valid hit point
        if(hitPoint.HasValue && hitNormal.HasValue) {
            // Draw the ray in green
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Camera.main.transform.position, hitPoint.Value);

            // Draw a small sphere at the hit point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hitPoint.Value, 0.05f);

            // Draw the surface normal at the hit point
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(hitPoint.Value, hitPoint.Value + hitNormal.Value * 0.5f);
        }

        // Check if there's a valid hit point
        if(moveHitPoint.HasValue && moveHitNormal.HasValue) {
            // Draw the ray in green
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Camera.main.transform.position, moveHitPoint.Value);

            // Draw a small sphere at the hit point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(moveHitPoint.Value, 0.05f);

            // Draw the surface normal at the hit point
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(moveHitPoint.Value, moveHitPoint.Value + moveHitNormal.Value * 0.5f);
        }

        if(isRayCreated) {
            // Draw the ray in the Scene view
            Gizmos.color = Color.red;
            Gizmos.DrawLine(zoomRay.origin, zoomRay.origin + zoomRay.direction * 100f);

            // Draw the hit point
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(initialZoomHitPoint, 0.5f);
        }
    }

    public void DisableControls() {
        controlsEnabled = false;
    }

    public void EnableControls() {
        controlsEnabled = true;
    }
}