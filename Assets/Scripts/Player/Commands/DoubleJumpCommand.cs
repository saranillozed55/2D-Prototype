using UnityEngine;

public class DoubleJumpCommand : ICommand
{
    private PlayerController playerController;
    public DoubleJumpCommand(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void Execute()
    {
        playerController.QueueJump();
    }
}
