using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementAndRotation : MonoBehaviour
{
    public float speed = 10f;
    public float turnSpeed = 90f;
    public Transform visualModel;
    public float maxRollAngle = 15f;
    public float rollSmoothing = 4f;
    public float grip = 3f;

    private Rigidbody rb;
    private float throttle;
    private float turnInput;
    private float currentRoll;
    private Quaternion baseRotation;

    void Update()
    {
        if(visualModel == null) return;

        float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / 5f);
        float targetRoll = -turnInput * maxRollAngle * speedFactor;
        currentRoll = Mathf.Lerp(currentRoll, targetRoll, rollSmoothing * Time.deltaTime);
        visualModel.localRotation = baseRotation * Quaternion.Euler(0f, 0f, currentRoll);

        Debug.Log("turnINput: " + turnInput + " roll: " + currentRoll);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (visualModel != null) baseRotation = visualModel.localRotation;
    }

    void OnMove(InputValue movementValue)
    {
        throttle = movementValue.Get<Vector2>().y;
    }

    void OnRotate(InputValue value)
    {
        turnInput = value.Get<float>();
    }

    void FixedUpdate()
    {
        //rb.angularVelocity = new Vector3(0f, turnInput * turnSpeed * Mathf.Deg2Rad, 0f);
        Vector3 av = rb.angularVelocity;
        av.y = turnInput * turnSpeed * Mathf.Deg2Rad;
        rb.angularVelocity = av;

        rb.AddForce(transform.forward * throttle * speed);

        // Turn the boat's momentum toward where it's facing
        Vector3 vel = rb.linearVelocity;                       // rb.velocity in older Unity
        Vector3 sideways = Vector3.Project(vel, transform.right);
        rb.linearVelocity = vel - sideways * Mathf.Clamp01(grip * Time.fixedDeltaTime);
    }
}
