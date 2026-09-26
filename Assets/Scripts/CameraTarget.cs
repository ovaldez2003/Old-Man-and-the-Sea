using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public Transform boat;   // the Boat root

    void LateUpdate()
    {
        transform.position = boat.position;
        transform.rotation = Quaternion.Euler(0f, boat.eulerAngles.y, 0f);
        
    }
}