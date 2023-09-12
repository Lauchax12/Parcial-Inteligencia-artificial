using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    ChangeState _change;
    Player _player;
    Transform _transform;
    float _speed;
    float _viewradius;
    float _viewangle;
    LayerMask _wallmask;
    Enemy _enemy;
    public Chase(ChangeState change,Player player,Transform transform,float speed,float viewangle,float viewradius,LayerMask wallmask,Enemy enemy)
    {
        _change = change;
        _player = player;
        _speed = speed;
        _transform = transform;
        _viewradius = viewradius;
        _viewangle = viewangle;
        _wallmask = wallmask;
        _enemy = enemy;
    }
    
    public override void OnEnter()
    {
        Debug.Log("lo empeze a perseguir");
        GameManager.instance.SearchMode(_enemy);
    }
    public override void OnUpdate()
    {
        if (!InFieldOfView(_player.transform.position))
        {
            _change.ChangeTheState(AgentStates.Patrol);
        }
        var dir = _player.transform.position - _transform.position;
        _transform.position += dir.normalized * _speed * Time.deltaTime;
        Quaternion rotation = Quaternion.LookRotation(dir);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, _speed * Time.deltaTime);
    }
    public override void OnExit()
    {
        GameManager.instance.ReturnMode(_enemy);
    }

    bool InFieldOfView(Vector3 targetPos)
    {
        Vector3 dir = targetPos - _transform.position;
        if (dir.sqrMagnitude > _viewradius * _viewradius) return false;
        if (Vector3.Angle(_transform.forward, dir) > _viewangle / 2) return false;
        if (!InLineOfSight(_transform.position, targetPos)) return false;

        return true;
    }

    bool InLineOfSight(Vector3 posA, Vector3 posB)
    {
        Vector3 dir = posB - posA;
        return !Physics.Raycast(posA, dir, dir.magnitude, _wallmask);
    }
}
