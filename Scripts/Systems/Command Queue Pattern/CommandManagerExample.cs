using Nato.Command;
using System;
using UnityEngine;

[RequireComponent(typeof(CommandQueue))]
public class CommandManagerExample : MonoBehaviour
{
    private bool isCommandQueueExecuting = false;
    CommandQueue commandQueue;

    private void OnEnable()
    {
        commandQueue = gameObject.GetComponent<CommandQueue>();
        commandQueue.OnCommandsExecuted += CommandQueue_OnCommandsExecuted;
    }

    private void OnDestroy()
    {
        commandQueue.OnCommandsExecuted -= CommandQueue_OnCommandsExecuted;
    }

    private void CommandQueue_OnCommandsExecuted()
    {
        Debug.Log("All Commands Executed");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ExecuteCommandsAsync(new ExampleCommand(value: 0), () =>
            {
                Debug.Log("End of commands");
            });
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            EnqueueAndExecuteCommands(new ExampleCommand(value: 0), () =>
            {
                Debug.Log("End of commands");
            });
        }
    }

    public void ExecuteCommandsAsync(ICommand command, Action callback=null)
    {
        commandQueue.EnqueueCommand(command);
        commandQueue.ExecuteCommands(callback);
    }

    public void EnqueueAndExecuteCommands(ICommand command, Action callback=null)
    {
        commandQueue.EnqueueCommand(command);
        if (!isCommandQueueExecuting)
        {
            if (commandQueue.commandQueue.Count > 0)
            {
                isCommandQueueExecuting = true;
                commandQueue.ExecuteCommands(() =>
                {

                    isCommandQueueExecuting = false;
                    callback?.Invoke();
                });
            }
        }
    }
}
