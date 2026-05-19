using System;
using Unity.VisualScripting;
using UnityEngine;

//ADD: Add UI for this script, only enable this script when the UI is open, and trigger events from buttons.
public class CheatMenu : MonoBehaviour
{
    public static CheatMenu Instance { get; private set; }
    

    [Header("Broadcast on Event Channels")]
    [SerializeField] private BoolEventChannel_SO _triggerCheats;
    [SerializeField] private BoolEventChannel_SO _killPlayer;
    [SerializeField] private BoolEventChannel_SO _playerInvincible;
    [SerializeField] private VoidEventChannelSO _playerInfiniteHealth;

    private void Awake()
    {
        if(CheatMenu.Instance != null && CheatMenu.Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            CheatMenu.Instance = this;
        }
    }

    //Don't use update, use buttons from uxml
    //Use buttons to send the event for BoolEventChannel_SO
}
