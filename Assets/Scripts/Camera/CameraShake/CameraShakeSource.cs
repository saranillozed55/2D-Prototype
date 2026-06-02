using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeSource : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera(float intensity, Vector3 direction)
    {
        _impulseSource.GenerateImpulse(direction * intensity);
    }
}
