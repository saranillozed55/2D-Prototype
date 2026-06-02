using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : GenericSingleton<CameraManager>
{

    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineCamera[] allVirtualCameras;

    [Header("Y Damping Settings")]
    [SerializeField] private float normalYDamping = 0.5f;
    [SerializeField] private float fallingYDamping = 0.1f;
    [SerializeField] private float climbingYDamping = 1.5f;
    [SerializeField] private float fallSpeedThreshhold = -2f;
    [SerializeField] private float dampingLerpSpeed = 5f;
    [SerializeField] private float fallingYOffset = -0.3f;

    private Coroutine _panCameraCoroutine;
    private CinemachineCamera currentCamera;
    private CinemachinePositionComposer positionComposer;
    private Vector3 _startingTrackedObjectOffset;

    public bool IsCameraPanning;
    private Rigidbody2D _playerRb;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i];

                positionComposer = currentCamera.GetComponent<CinemachinePositionComposer>();
            }
        }
        _startingTrackedObjectOffset = positionComposer.TargetOffset;
    }
    
    public void SetPlayerRigidody(Rigidbody2D rb)
    {
        _playerRb = rb;
    }

    private void Update()
    {
        if (_playerRb == null || IsCameraPanning) return;

        float velY = _playerRb.linearVelocity.y;

        float targetDamping;
        float targetOffsetY;

        if(velY < fallSpeedThreshhold)
        {
            targetDamping = fallingYDamping;
            targetOffsetY = fallingYOffset;
        }
        else if( velY > 0f)
        {
            targetDamping = climbingYDamping;
            targetOffsetY = _startingTrackedObjectOffset.y;
        }
        else
        {
            targetDamping = normalYDamping;
            targetOffsetY = _startingTrackedObjectOffset.y;
            
        }
        positionComposer.Damping.y = Mathf.Lerp(positionComposer.Damping.y, targetDamping, Time.deltaTime * dampingLerpSpeed);
        Vector3 offset = positionComposer.TargetOffset;
        offset.y = Mathf.Lerp(offset.y, targetOffsetY, Time.deltaTime * dampingLerpSpeed);
        positionComposer.TargetOffset = offset;
    }

    #region Pan Camera
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        if (_panCameraCoroutine != null) StopCoroutine(_panCameraCoroutine);
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }   

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if(!panToStartingPos)
        {
            switch (panDirection) {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;

            startingPos = _startingTrackedObjectOffset;

            endPos += startingPos;
        }
        //handle the direction settings when moving back to the starting position

        else
        {
            startingPos = positionComposer.TargetOffset;
            endPos = _startingTrackedObjectOffset;
        }
        float elapsedTime = 0f;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            positionComposer.TargetOffset = panLerp;

            yield return null;
        }
        positionComposer.TargetOffset = endPos;
    }

    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineCamera cameraFromLeft, CinemachineCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        // if the current camera is the camera on the left and our trigger exit direction was on the right
        if(currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            //activate the new camera

            cameraFromRight.enabled = true;
            //deactivate the old one

            cameraFromLeft.enabled = false;
            //set the new camera as the current camera
            currentCamera = cameraFromRight;

            //update composer variable
            positionComposer = currentCamera.GetComponent<CinemachinePositionComposer>();
        }
        else if(currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            //activate the new camera

            cameraFromLeft.enabled = true;
            //deactivate the old one

            cameraFromRight.enabled = false;
            //set the new camera as the current camera
            currentCamera = cameraFromLeft;

            //update composer variable
            positionComposer = currentCamera.GetComponent<CinemachinePositionComposer>();
        }
    }

    #endregion
}
