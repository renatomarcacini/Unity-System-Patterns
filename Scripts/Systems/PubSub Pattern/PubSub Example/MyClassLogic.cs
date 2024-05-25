using Nato.PubSub;
using UnityEngine;
using static MyClassEvent;

public class MyClassLogic : MonoBehaviour
{
    private int score;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            score++;
            MyClassScoredEvent playerScoredEvent = new MyClassScoredEvent(score);
            EventManager<MyClassScoredEvent>.Publish(playerScoredEvent);
            EventManager<MyClassWinnedEvent>.Publish(new MyClassWinnedEvent());

            EventManager.Publish(MYCLASS_SCORED, playerScoredEvent);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            MyClassDiedEvent playerDiedEvent = new MyClassDiedEvent("Enemy");
            EventManager<MyClassDiedEvent>.Publish(playerDiedEvent);
            EventManager.Publish(MYCLASS_DIED, playerDiedEvent);

        }
    }
}
