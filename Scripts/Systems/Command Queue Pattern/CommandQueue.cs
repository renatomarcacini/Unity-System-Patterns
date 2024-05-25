using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nato.Command
{
    public class CommandQueue : MonoBehaviour
    {
        public Queue<ICommand> commandQueue = new Queue<ICommand>();

        public event Action OnCommandsExecuted;

        private Action onCommandQueueEmptyCallback;

        public void EnqueueCommand(ICommand command)
        {
            commandQueue.Enqueue(command);
        }

        public void ExecuteCommands(Action onCommandQueueEmpty = null, float delay = 0)
        {
            onCommandQueueEmptyCallback = onCommandQueueEmpty;
            StartCoroutine(ExecuteCommandsCoroutine());
        }

        private IEnumerator ExecuteCommandsCoroutine(float delay = 0)
        {
            while (commandQueue.Count > 0)
            {
                ICommand command = commandQueue.Dequeue();
                yield return StartCoroutine(command.Execute());
            }

            yield return new WaitForSeconds(delay);
            OnCommandsExecuted?.Invoke();
            onCommandQueueEmptyCallback?.Invoke();
        }
    }
}