using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] Player player;
    [SerializeField] LayerMask _wallMask;
    
    [SerializeField] float _viewAngle;
    [SerializeField] float _viewRadius;


    public Waypoints[] waypointspatrol;
    int _actualWaypoint;
    public float speed;
    public float rotation_speed;
    public float rotation_modifier;
    [Range(0.5f, 3)]
    public float minDetectWaypoint;
    public float minDetectnode;
    [Header("pathfinding")]
    public Waypoints startingNode;
    public Waypoints _goalNode;
    public List<Waypoints> _path = new List<Waypoints>();

    bool searching=false;

    [Header("fsm")]
    ChangeState _change;

    private void Start()
    {
        startingNode = GetNeighbors();
        GameManager.instance.AddEnemy(this);
        _change = new ChangeState();
        _change.AddState(AgentStates.Patrol, new Patrol(_change,transform,speed, waypointspatrol,_actualWaypoint,minDetectWaypoint,startingNode,player, _viewRadius, _viewAngle, _wallMask));
        _change.AddState(AgentStates.Chase, new Chase(_change, player, transform, speed, _viewAngle, _viewRadius, _wallMask,this));
        _change.AddState(AgentStates.Find, new Find(_change, transform, speed, minDetectnode));
        _change.AddState(AgentStates.Return, new Return(_change, transform, speed, minDetectnode,startingNode));
        _change.ChangeTheState(AgentStates.Patrol);
    }
   

    void Update()
    {
        _change.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _change.ChangeTheState(AgentStates.Find);
        }

        Debug.DrawLine(transform.position, player.transform.position);
        
    }
    //Fov
    #region
    bool InFieldOfView(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        if (dir.sqrMagnitude > _viewRadius * _viewRadius) return false; 
        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;
        if (!InLineOfSight(transform.position, targetPos)) return false;

        return true;
    }

    bool InLineOfSight(Vector3 posA, Vector3 posB)
    {
        Vector3 dir = posB - posA;
        return !Physics.Raycast(posA, dir, dir.magnitude, _wallMask);
    }

    

    Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    #endregion


    
   
    public void ComeBack()
    {
        _change.ChangeTheState(AgentStates.Return);
    }

    public void Search()
    {

        _change.ChangeTheState(AgentStates.Find);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Vector3 dirA = DirFromAngle(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 dirB = DirFromAngle(-_viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + dirA.normalized * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + dirB.normalized * _viewRadius);
    }

    public Waypoints GetNeighbors()
    {

        //paso por cada uno de los nodos del array que está en gamemanager
        foreach (Waypoints node in GameManager.instance.waypoints)
        {

            //creo un vector 3 para sacar la direccion del raycast
            Vector3 dir = node.transform.position - transform.position;
            RaycastHit hit;
            //si el raycast toca otro nodo
            if (Physics.Raycast(transform.position, dir, out hit, 20))
            {
                var actualnode = hit.transform.gameObject.GetComponent<Waypoints>();
                if (actualnode != null)
                {
                    //si la lista no contiene el nodo que lo agregue
                    if (dir.magnitude <= minDetectnode)
                    {

                        return actualnode;
                    }


                }
            }
        }


        return null;

    }
}

public enum AgentStates
{
    Find,
    Chase,
    Patrol,
    Return
}
