using UnityEngine;

public class BoatDebug : MonoBehaviour
{
    Rigidbody rb;

    void Start() { rb = GetComponent<Rigidbody>(); }

    void OnGUI()
    {
        if (rb == null) { GUI.Label(new Rect(10, 10, 500, 30), "NO RIGIDBODY ON THIS OBJECT"); return; }

        string s =
            "Root rotation: " + transform.eulerAngles.ToString("F1") + "\n" +
            "Root position: " + transform.position.ToString("F2") + "\n" +
            "Velocity: " + rb.linearVelocity.ToString("F2") + "\n" +
            "Angular velocity (rad/s): " + rb.angularVelocity.ToString("F2") + "\n" +
            "Constraints: " + rb.constraints + "\n" +
            "Kinematic: " + rb.isKinematic + "   Gravity: " + rb.useGravity + "\n" +
            "Extra Rigidbodies in children: " + (GetComponentsInChildren<Rigidbody>().Length - 1) + "\n" +
            "Colliders (root + children): " + GetComponentsInChildren<Collider>().Length;

        GUI.Label(new Rect(10, 10, 600, 200), s);
    }
}