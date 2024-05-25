using Nato.Command;
using System.Collections;
using UnityEngine;

public class ExampleCommand : ICommand
{
    int value = 0;
    public ExampleCommand(int value)
    {
        this.value = value;
    }

    public IEnumerator Execute()
    {
        yield return new WaitForSeconds(1);
        value++;
        Debug.Log("Value: " + value);

        yield return new WaitForSeconds(1);
        value++;
        Debug.Log("Value: " + value);

        yield return new WaitForSeconds(1);
        value++;
        Debug.Log("Value: " + value);
    }

    /* FOR DOTWEEN
    private IEnumerator Test(Transform target)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOMove(Vector3.zero, 0.5f));
        sequence.Append(target.DOShakeRotation(0.5f));
        yield return sequence.WaitForCompletion();
    }
    */
}
