using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    private void Awake()
    {
        instance = this;
    }


    public List<Waypoints> AStar(Waypoints start, Waypoints goal)
    {
        if (start == null || goal == null) return new List<Waypoints>();

        PriorityQueue<Waypoints> frontier = new PriorityQueue<Waypoints>();
        frontier.Enqueue(start, 0);

        Dictionary<Waypoints, Waypoints> cameFrom = new Dictionary<Waypoints, Waypoints>();
        cameFrom.Add(start, null);

        Dictionary<Waypoints, int> costSoFar = new Dictionary<Waypoints, int>();
        costSoFar.Add(start, 0);

        Waypoints current = null;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break; //Llegamos al final, terminamos de recorrer

            foreach (var next in current.GetNeighbors())
            {
                //if (next.blocked) continue;

                int newCost = costSoFar[current];

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, newCost + Heuristic(next, goal));
                    costSoFar.Add(next, newCost);
                    cameFrom.Add(next, current);
                }
                else if (newCost < costSoFar[current])
                {
                    frontier.Enqueue(next, newCost + Heuristic(next, goal));
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }
            }
        }

        if (current != goal) return new List<Waypoints>();

        //Generamos el camino
        List<Waypoints> path = new List<Waypoints>();
        while (current != start)
        {
            path.Add(current);
            current = cameFrom[current];
        }

        //path.Add(start.transform.position); //opcional
        path.Reverse(); //opcional

        return path;
    }

    float Heuristic(Waypoints start, Waypoints end)
    {
        
        return Vector3.Distance(start.transform.position, end.transform.position); //Escenario chico esto es lo mejor
    }

    public IEnumerator AStarRoutine(Waypoints start, Waypoints goal)
    {
        PriorityQueue<Waypoints> frontier = new PriorityQueue<Waypoints>();
        frontier.Enqueue(start, 0);

        Dictionary<Waypoints, Waypoints> cameFrom = new Dictionary<Waypoints, Waypoints>();
        cameFrom.Add(start, null);

        Dictionary<Waypoints, int> costSoFar = new Dictionary<Waypoints, int>();
        costSoFar.Add(start, 0);

        Waypoints current = null;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                List<Waypoints> path = new List<Waypoints>();
                while (current != start)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

               

                break; //Llegamos al final, terminamos de recorrer
            }


            yield return new WaitForSeconds(0.001f);

            foreach (var next in current.GetNeighbors())
            {
                

                int newCost = costSoFar[current] + 1;

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, newCost + Heuristic(next, goal));
                    costSoFar.Add(next, newCost);
                    cameFrom.Add(next, current);
                   
                }
                else if (newCost < costSoFar[current])
                {
                    frontier.Enqueue(next, newCost + Heuristic(next, goal));
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                    
                }

                yield return new WaitForSeconds(0.001f);
            }
        }

    }

    //activa la corrutina
    public void ActiveSearch(Waypoints start,Waypoints goal)
    {
       StartCoroutine(AStarRoutine(start, goal));
    }

}
