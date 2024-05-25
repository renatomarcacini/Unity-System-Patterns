using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : BaseUI
{
    [SerializeField] private Text stateText;

    public override void Enable()
    {
        base.Enable();

        stateText.text = "GAMEPLAY";
    }

    public override void Disable()
    {
        base.Disable();
    }

    public void SetTimer(float time)
    {
        int timeMs = Mathf.CeilToInt(time) * 1000;
        TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, timeMs);
        stateText.text = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }
}
