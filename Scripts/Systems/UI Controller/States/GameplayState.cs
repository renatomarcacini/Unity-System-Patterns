using Nato.StateMachine;
using UnityEngine;

public class GameplayState : BaseState<ExampleStateManager>
{
    private float timer = 0;

    public override void OnStart(ExampleStateManager gameManager)
    {
        base.OnStart(gameManager);

        Manager.UIManager.GameplayUI.Enable();
    }

    public override void OnEnd()
    {
        base.OnEnd();

        Manager.UIManager.GameplayUI.Disable();
    }

    public override void OnTick()
    {
        base.OnTick();

        timer += Time.deltaTime;
        Manager.UIManager.GameplayUI.SetTimer(timer);


        if (Input.GetKeyDown(KeyCode.Space))
            Manager.StateMachine.TransitionTo<MenuState>();
    }

    public void SetInitialTimer(float timer)
    {
        this.timer = timer; 
    }
}