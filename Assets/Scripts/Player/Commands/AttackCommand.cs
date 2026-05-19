using UnityEngine;
/*
 * Don't use this for player
 */
public class AttackCommand : ICommand
{
    private PlayerController playerController;

    public AttackCommand(PlayerController playerController)
    {
        this.playerController = playerController;
    }
    public void Execute()
    {

    }
}
