using Nato.PubSub;
using UnityEngine;
using UnityEngine.UI;
using static MyClassEvent;

public class MyClassVisual : MonoBehaviour
{
    public Text scoreText;

    private void OnEnable()
    {
        EventManager<MyClassScoredEvent>.Subscribe(OnPlayerScored);
        EventManager<MyClassDiedEvent>.Subscribe(OnPlayerDied);
        EventManager<MyClassWinnedEvent>.Subscribe(OnPlayerWinned);

        EventManager.Subscribe(MYCLASS_SCORED, OnPlayerScoredMessage);
        EventManager.Subscribe(MYCLASS_DIED, OnPlayerDiedMessage);
    }



    private void OnDisable()
    {
        EventManager<MyClassScoredEvent>.Unsubscribe(OnPlayerScored);
        EventManager<MyClassDiedEvent>.Unsubscribe(OnPlayerDied);
        EventManager<MyClassWinnedEvent>.Unsubscribe(OnPlayerWinned);

        EventManager.Unsubscribe(MYCLASS_SCORED, OnPlayerScoredMessage);
        EventManager.Unsubscribe(MYCLASS_DIED, OnPlayerDiedMessage);
    }

    private void OnPlayerScored(MyClassEvent.MyClassScoredEvent @event)
    {
        Debug.Log("GENERIC: " + @event.Score);
    }

    private void OnPlayerDied(MyClassEvent.MyClassDiedEvent @event)
    {
        Debug.Log("GENERIC: " + @event.KillerName);
    }

    private void OnPlayerWinned(MyClassWinnedEvent @event)
    {
        Debug.Log("GENERIC WIN [Class null]");
    }

    private void OnPlayerScoredMessage(object obj)
    {
        MyClassScoredEvent @event = (MyClassScoredEvent)obj;
        Debug.Log("STRING: " + @event.Score);
    }

    private void OnPlayerDiedMessage(object obj)
    {
        MyClassDiedEvent @event = (MyClassDiedEvent)obj;
        Debug.Log("STRING: " + @event.KillerName);
    }
}
