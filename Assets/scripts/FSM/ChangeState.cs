using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeState 
{
    IState _currentState;
    Dictionary<AgentStates, IState> _allStates = new Dictionary<AgentStates, IState>();

    
    public void AddState(AgentStates key, IState state)
    {
        if (_allStates.ContainsKey(key))
            _allStates[key] = state;

        state.fsm = this;

        _allStates.Add(key, state);
        if (_currentState == null) ChangeTheState(key);
    }

    public void ChangeTheState(AgentStates state)
    {
        if (!_allStates.ContainsKey(state)) return;

        if (_currentState != null) _currentState.OnExit();
        _currentState = _allStates[state];
        _currentState.OnEnter();
    }

    public void Update()
    {
        _currentState.OnUpdate();
    }

    

}
