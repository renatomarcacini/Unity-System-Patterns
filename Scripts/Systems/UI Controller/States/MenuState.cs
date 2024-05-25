using Nato.StateMachine;
using UnityEngine;

public class MenuState : BaseState<ExampleStateManager>
{
    public override void OnStart(ExampleStateManager manager)
    {
        base.OnStart(manager);
        Manager.UIManager.MenuUI.Enable();
    }

    public override void OnEnd()
    {
        base.OnEnd();

        Manager.UIManager.MenuUI.Disable();
    }

    public override void OnTick()
    {
        base.OnTick();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Manager.StateMachine.TransitionTo<GameplayState>();
        }
    }
}
