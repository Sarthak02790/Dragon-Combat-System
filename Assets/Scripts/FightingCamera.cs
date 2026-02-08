using UnityEngine;

public class FightingCamera : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;
    public Transform enemy;

    [Header("Positioning")]
    public Vector3 offset = new Vector3(0, 2f, -10f);
    public float smoothSpeed = 5f;

    [Header("Zoom Settings")]
    public float minDistance = 5f;
    public float maxDistance = 15f;
    public float zoomSpeed = 2f;

    void LateUpdate()
    {
        if (player == null || enemy == null) return;

        // Find the Center Point between both dragons
        Vector3 midpoint = (player.position + enemy.position) / 2f;

        // Calculate zoom based on how far apart they are
        float distanceBetweenFighters = Vector3.Distance(player.position, enemy.position);
        float zoomFactor = Mathf.Clamp(distanceBetweenFighters, minDistance, maxDistance);

        Vector3 targetPosition = midpoint + offset.normalized * zoomFactor;
        targetPosition.y = midpoint.y + offset.y;

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Always look at the midpoint
        transform.LookAt(midpoint + Vector3.up * 1.5f);
    }
}