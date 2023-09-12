using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float _minDetectWaypoint;
    public Transform startposition;

    // Start is called before the first frame update
    void Start()
    {

        transform.position = startposition.position;

    }

    // Update is called once per frame
    void Update()
    {
        var H = Input.GetAxis("Horizontal");
        var V = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(H * Time.deltaTime * speed, 0, V * Time.deltaTime * speed);
        transform.position += move;



        //GetNeighbors

        
        _neighbor = GetNeighbors();

        GameManager.instance.goal = _neighbor;
    }

    //sacar el waypoint cercano para tenerlo como goal
    public Waypoints _neighbor;

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
                    if (dir.magnitude<=_minDetectWaypoint)
                    {
                        
                        return actualnode;
                    }


                }
            }
        }


        return null;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _minDetectWaypoint);
    }
}
