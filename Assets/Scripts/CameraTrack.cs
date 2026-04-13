using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Transform player;
    private PlayerMovement playerMovement;
    public float followSpeed = 5f;

    [Header("Mouse Look Offset")]
    public float maxOffsetDistance = 3f;
    public float offsetSmoothing = 5f;

    [Header("Dead Zone")]
    public float deadZoneRadius = 1.5f;

    [Header("Dash Effect")]
    public float dashStretchDistance = 2f;
    public float dashReturnSpeed = 5f;

    private Camera cam;
    private Vector3 currentOffset;
    private Vector3 dashOffset;

    void Start()
    {
        cam = Camera.main;
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;

        Vector3 direction = (mouseWorld - player.position);
        float distance = direction.magnitude;

        Vector3 offset = Vector3.zero;

        if (distance > maxOffsetDistance)
        {
            float adjustedDistance = distance - deadZoneRadius;

            float t = adjustedDistance / maxOffsetDistance;
            t = Mathf.SmoothStep(0, 1, t);
            offset = direction.normalized * (t * maxOffsetDistance);

            offset = Vector3.ClampMagnitude(offset, maxOffsetDistance);
        }

        currentOffset = Vector3.Lerp(currentOffset, offset, offsetSmoothing * Time.deltaTime);

        if (playerMovement != null && playerMovement.IsDashing)
        {
            dashOffset = Vector3.Lerp(
            dashOffset,
            (Vector3)playerMovement.DashDirection.normalized * dashStretchDistance,
            15f * Time.deltaTime
            );
        }
        else
        {
            dashOffset = Vector3.Lerp(dashOffset, Vector3.zero, dashReturnSpeed * Time.deltaTime);
        }

        Vector3 targetPosition = player.position + currentOffset;
        targetPosition.z = -10f;

        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}