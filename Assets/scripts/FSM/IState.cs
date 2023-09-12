using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState
{
    public ChangeState fsm;
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
