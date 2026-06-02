using UnityEngine;

public class DoorTransition : MonoBehaviour, IInteractable
{   
    private bool canInteract = true;

    public void Interact()
    {
        if(canInteract)
        {
            SceneLoader.Instance.LoadScene();
        }
    }
}
