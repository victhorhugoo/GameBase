using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VictorGame.StateMachine;

public class FSMExemplo : MonoBehaviour
{
    public enum ExampleEnum
    {
        STATE_1,
        STATE_2,
        STATE_3
    }

    public StateMachine<ExampleEnum> stateMachine;

    private void Start()
    {
        stateMachine = new StateMachine<ExampleEnum>();
        stateMachine.Init();
        stateMachine.RegisterStates(ExampleEnum.STATE_1, new StateBase());
        stateMachine.RegisterStates(ExampleEnum.STATE_2, new StateBase());
        stateMachine.RegisterStates(ExampleEnum.STATE_3, new StateBase());

    }
}
