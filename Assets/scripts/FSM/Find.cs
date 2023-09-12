using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find : IState
{
    ChangeState _change;
    Waypoints _startingNode;
    Waypoints _goalNode;
    List<Waypoints> _path = new List<Waypoints>();
    Transform _transform;
    float _speed;
    float _minDetectWaypoint;
    public Find(ChangeState change, Transform transform, float speed,float minDetecWaypoint)
    {
        _change = change;
        _transform = transform;
        _speed = speed;
        _minDetectWaypoint = minDetecWaypoint;
    }

    public override void OnEnter()
    {
        Debug.Log("Lo estoy buscando");
        _startingNode = GetNeighbors();
        _goalNode = GameManager.instance.goal;
        _path = Pathfinding.instance.AStar(_startingNode, _goalNode);
        Pathfinding.instance.ActiveSearch(_startingNode, _goalNode);
    }
    public override void OnUpdate()
    {
        if (_path.Count > 0)
        {
            //Recorremos la colección path como recorremos los waypoints
            Vector3 target = _path[0].transform.position;
            _transform.forward = target - _transform.position;
            _transform.position += _transform.forward * _speed * Time.deltaTime;

            if (Vector3.Distance(_transform.position, target) <= 0.05f)
                _path.RemoveAt(0);
        }
        else
        {
            _change.ChangeTheState(AgentStates.Patrol);
        }
        
    }

    public override void OnExit()
    {
        Debug.Log("Lo dejé de buscar");
    }

    public Waypoints GetNeighbors()
    {

        //paso por cada uno de los nodos del array que está en gamemanager
        foreach (Waypoints node in GameManager.instance.waypoints)
        {

            //creo un vector 3 para sacar la direccion del raycast
            Vector3 dir = node.transform.position - _transform.position;
            RaycastHit hit;
            //si el raycast toca otro nodo
            if (Physics.Raycast(_transform.position, dir, out hit, 20))
            {
                var actualnode = hit.transform.gameObject.GetComponent<Waypoints>();
                if (actualnode != null)
                {
                    //si la lista no contiene el nodo que lo agregue
                    if (dir.magnitude <= _minDetectWaypoint)
                    {

                        return actualnode;
                    }


                }
            }
        }


        return null;

    }

}
