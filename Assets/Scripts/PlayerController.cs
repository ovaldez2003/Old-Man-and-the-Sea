using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;            // Thrust (acceleration)
    public float turnSpeed = 30f;       // Degrees per second
    public float grip = 3f;             // Sideways drift reduction

    public Transform visualModel;       // VisualPivot
    public float maxRollAngle = 15f;
    public float rollSmoothing = 4f;

    private Rigidbody rb;
    private float throttle;
    private float turnInput;
    private float currentRoll;
    private Quaternion baseRotation;
    private bool controlsLocked;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set up the body in code so Inspector settings can't interfere
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ
                       | RigidbodyConstraints.FreezePositionY;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (visualModel != null) baseRotation = visualModel.localRotation;
    }

    void OnMove(InputValue value)   { throttle = value.Get<Vector2>().y; }
    void OnRotate(InputValue value) { turnInput = value.Get<float>(); }

    void Update()
    {
        if (visualModel == null) return;

        float speedFactor = Mathf.Clamp01(rb.linearVelocity.magnitude / 2f);
        float targetRoll = -turnInput * maxRollAngle * speedFactor;
        currentRoll = Mathf.Lerp(currentRoll, targetRoll, rollSmoothing * Time.deltaTime);

        // Roll around the parent's forward axis, then apply the pivot's own offset
        visualModel.localRotation = Quaternion.Euler(0f, 0f, currentRoll) * baseRotation;
    }

    void FixedUpdate()
    {
        if(controlsLocked) return;
        // Turn (yaw only)
        Vector3 av = rb.angularVelocity;
        av.y = turnInput * turnSpeed * Mathf.Deg2Rad;
        rb.angularVelocity = av;
        // Quaternion turn = Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
        // rb.MoveRotation(rb.rotation * turn);

        // Thrust along facing direction
        rb.AddForce(transform.forward * throttle * speed, ForceMode.Acceleration);

        // Reduce sideways drift
        Vector3 vel = rb.linearVelocity;
        Vector3 sideways = Vector3.Project(vel, transform.right);
        rb.linearVelocity = vel - sideways * Mathf.Clamp01(grip * Time.fixedDeltaTime);
    }

    public void SetControlsLocked(bool locked)
    {
        controlsLocked = locked;
        if(locked)
        {
            throttle = 0f;
            turnInput = 0f;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}