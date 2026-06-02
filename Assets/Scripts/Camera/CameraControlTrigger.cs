using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
        {
            Debug.LogError("No collider found on CameraControlTrigger object. Please add a 2D collider and set it to trigger.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            if (customInspectorObjects.panCameraOnContact)
            {
                //pan the camera based on the pan direction in the inspector
                CameraManager.Instance.IsCameraPanning = true;
                CameraManager.Instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _collider.bounds.center).normalized; // If the x value is positive, they exited to the right, if x is negative then they exited left
            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null && customInspectorObjects != null)
            {
                //swap cameras
                CameraManager.Instance.SwapCamera(customInspectorObjects.cameraOnLeft, customInspectorObjects.cameraOnRight, exitDirection);
            }
            if (customInspectorObjects.panCameraOnContact)
            {
                //pan the camera back to the original position
                CameraManager.Instance.IsCameraPanning = true;
                CameraManager.Instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }

    }

}

[System.Serializable]
public class CustomInspectorObjects 
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineCamera cameraOnLeft;
    [HideInInspector] public CinemachineCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;

}

public enum PanDirection { 
    Up,
    Down,
    Left,
    Right

}

//[CustomEditor(typeof(CameraControlTrigger))]
//public class MyScriptEditor : Editor 
//{
//    CameraControlTrigger cameraControlTrigger;

//    private void OnEnable()
//    {
//        cameraControlTrigger = (CameraControlTrigger)target;
//    }
//    override public void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        if(cameraControlTrigger.customInspectorObjects.swapCameras)
//        {
//            cameraControlTrigger.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera On Left", cameraControlTrigger.customInspectorObjects.cameraOnLeft, typeof(CinemachineCamera), true) as CinemachineCamera;
//            cameraControlTrigger.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera On Right", cameraControlTrigger.customInspectorObjects.cameraOnRight, typeof(CinemachineCamera), true) as CinemachineCamera;
//        }

//        if(cameraControlTrigger.customInspectorObjects.panCameraOnContact)
//        {
//            cameraControlTrigger.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Pan Direction", cameraControlTrigger.customInspectorObjects.panDirection);
//            cameraControlTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.customInspectorObjects.panDistance);
//            cameraControlTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.customInspectorObjects.panTime);
//        }

//        if(GUI.changed)
//        {
//            EditorUtility.SetDirty(cameraControlTrigger);
//        }
//    }

    
//}


