using UnityEngine;
using UnityEngine.UI;

public class MenuUI : BaseUI
{
    [SerializeField] private Text stateText;

    public override void Enable()
    {
        base.Enable();

        stateText.text = "MENU";
    }

    public override void Disable()
    {
        base.Disable();
    }
}
