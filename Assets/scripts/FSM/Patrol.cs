using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : IState
{
    Waypoints[] _patrolpoints;
    float _speed;
    ChangeState _change;
    int _actualWaypoint;
    float _minDetectWaypoint;
    Transform _transform;
    Waypoints _startingNode;
    Player _player;
    bool _FOV;
    float _viewradius;
    float _viewangle;
    LayerMask _wallMask;
    public Patrol(ChangeState change, Transform transform, float speed, Waypoints[] patrolpoints, int actualWaypoint,  float minDetectWaypoint,Waypoints startingNode,Player player,float viewradius,float viewangle,LayerMask wallmask)
    {
        _change = change;
        _transform = transform;
        _speed = speed;
        _startingNode = startingNode;
        _patrolpoints = patrolpoints;
        _actualWaypoint = actualWaypoint;
        _player = player;
        _minDetectWaypoint = minDetectWaypoint;
        _viewangle = viewangle;
        _viewradius = viewradius;
        _wallMask = wallmask;
    }


    public override void OnEnter()
    {
        Debug.Log("toy patrullando");
    }
    public override void OnUpdate()
    {
        if (InFieldOfView(_player.transform.position))
        {
            _change.ChangeTheState(AgentStates.Chase);
        }

        var dir = _patrolpoints[_actualWaypoint].transform.position - _transform.position;
        _transform.position += dir.normalized * _speed * Time.deltaTime;
        _startingNode = _patrolpoints[_actualWaypoint];
        if (dir.magnitude <= _minDetectWaypoint)
        {
         _actualWaypoint++;

         if (_actualWaypoint >= _patrolpoints.Length)
         _actualWaypoint = 0;
        }

        Quaternion rotation = Quaternion.LookRotation(dir);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, rotation, _speed * Time.deltaTime);
    }

    public override void OnExit()
    {
        Debug.Log("deje de patrullar");
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
        return !Physics.Raycast(posA, dir, dir.magnitude, _wallMask);
    }
}
