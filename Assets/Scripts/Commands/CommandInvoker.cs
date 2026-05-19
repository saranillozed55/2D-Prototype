using UnityEngine;

public class CommandInvoker
{
    public static void ExecuteCommand(ICommand command)
    {
        command.Execute();
    }
}
