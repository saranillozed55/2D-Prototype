using UnityEngine;

public class PlayerButtons : MonoBehaviour
{
    //[SerializeField] private PlayerInputHandler inputHandler;
    //[SerializeField] private MenuEvents menuEvents;

    ////DELETE THIS OR UPDATE IT BECAUSE SWITCHING ACTION MAPS
    //private void OnEnable()
    //{
    //    if(inputHandler != null)
    //    {
    //        inputHandler.OnMenu += PausePerformed;
    //    }
    //}

    //private void OnDisable()
    //{
    //    if (inputHandler != null)
    //    {
    //        inputHandler.OnMenu -= PausePerformed;
    //    }
    //}

    //private void PausePerformed()
    //{
    //    if(GameManager.Instance.currentState == GameState.Playing)
    //    {
    //        GameManager.Instance.UpdateGameState(GameState.Paused);
    //        menuEvents.document.enabled = true;
    //        menuEvents.InitializeMenu();
    //    }
    //    else if(GameManager.Instance.currentState == GameState.Paused)
    //    {
    //        GameManager.Instance.UpdateGameState(GameState.Playing);
    //        menuEvents.document.enabled = false;
    //    }
    //}
}
