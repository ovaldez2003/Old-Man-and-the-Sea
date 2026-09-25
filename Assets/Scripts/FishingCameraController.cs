using UnityEngine;
using Unity.Cinemachine;

public class FishingCameraController : MonoBehaviour
{
    public CinemachineCamera boatCamera;
    public CinemachineCamera fishingCamera;
    public float zoomedFOV = 30f;   // Lower = more zoomed in

    private float defaultFOV;

    void Start()
    {
        defaultFOV = fishingCamera.Lens.FieldOfView;
    }

    public void ShowFishingView(Transform lookTarget)
    {
        // Start from wherever the camera currently is, not a fixed anchor
        Transform current = Camera.main.transform;
        Vector3 dir = (lookTarget.position - current.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

        fishingCamera.transform.SetPositionAndRotation(current.position, rot);

        var lens = fishingCamera.Lens;
        lens.FieldOfView = zoomedFOV;
        fishingCamera.Lens = lens;

        fishingCamera.Priority = boatCamera.Priority + 10;
    }

    public void ReturnToBoatView()
    {
        fishingCamera.Priority = boatCamera.Priority - 10;

        var lens = fishingCamera.Lens;
        lens.FieldOfView = defaultFOV;
        fishingCamera.Lens = lens;
    }
}