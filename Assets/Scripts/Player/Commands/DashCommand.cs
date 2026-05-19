using UnityEngine;
/*
 * Don't use this for player
 */
public class DashCommand : ICommand
{
    private PlayerController playerController;

    public DashCommand(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void Execute()
    {
        // Implement dash logic here
        playerController.Dash();
    }
}   
