using Nato.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleStateManager : MonoBehaviour
{
    public static ExampleStateManager Instance { get; private set; }

    public UIManager UIManager;
    public StateMachine<ExampleStateManager> StateMachine { get; private set; }

    private void Awake()
    {
        Instance = this;
        StateMachine = new(this);
    }

    private void Start()
    {
        StateMachine.TransitionTo<MenuState>();
    }

    private void Update()
    {
        StateMachine.OnTick();
    }
}
