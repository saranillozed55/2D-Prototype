using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Player Input Handler")]
    [SerializeField] private PlayerInputHandler inputHandler;

    private IInteractable currentInteractable;

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnInteract += TryInteract;
        }
    }

    private void OnDisable()
    {
        if(inputHandler != null)
        {
            inputHandler.OnInteract -= TryInteract;
        }
    }

    private void TryInteract()
    {
        if(currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            //Debug.Log("Interactable in range");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable = null;
            //Debug.Log("Left Interctable Range");
        }
    }

}
