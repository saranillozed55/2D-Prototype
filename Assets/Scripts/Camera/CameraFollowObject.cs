using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotations Stats")]
    [SerializeField] private float flipRotationTime = 0.5f;

    [SerializeField] private CinemachinePositionComposer _composer;

    private PlayerController player;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<PlayerController>();
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        LeanTween.cancel(gameObject);
        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipRotationTime).setEaseInOutSine();
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < flipRotationTime)
        {
            elapsedTime += Time.deltaTime;

            //lerp the y rotation

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, elapsedTime / flipRotationTime);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        if(player.IsFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
}
