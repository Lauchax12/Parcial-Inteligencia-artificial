using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    //creo la lista de vecinos
    public List<Waypoints> _neighbors = new List<Waypoints>();

    public int cost = 1;


    //inicializo la lista y chequeo vecinos
    private void Start()
    {
        _neighbors = GetNeighbors();
    }

    //Creo las variables para que se agreguen los vecinos
    public List<Waypoints> GetNeighbors()
    {
        List<Waypoints> nodes = new List<Waypoints>();
        //paso por cada uno de los nodos del array que está en gamemanager
        foreach (Waypoints node in GameManager.instance.waypoints)
        {
            if (node == this)
            {
                continue;
            }
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
                    if (!nodes.Contains(actualnode))
                    {
                        nodes.Add(actualnode);
                    }


                }
            }
        }
        return nodes;



    }
}
