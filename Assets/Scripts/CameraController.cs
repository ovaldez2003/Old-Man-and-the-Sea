using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;      // The boat (object with the Rigidbody)
    public float distance = 8f;   // How far behind the boat
    public float height = 4f;     // How far above the boat
    public float lookHeight = 1f; // Aim slightly above the boat's center

    void LateUpdate()
    {
        // Use only the boat's left/right heading
        Quaternion yaw = Quaternion.Euler(0f, player.eulerAngles.y, 0f);

        // Place the camera behind and above the boat
        transform.position = player.position + yaw * new Vector3(0f, height, -distance);

        // Look at the boat
        transform.LookAt(player.position + Vector3.up * lookHeight);
    }
}