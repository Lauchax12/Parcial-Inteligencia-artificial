using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Waypoints[] waypoints;
    public LayerMask maskWall;

    public List<Enemy> enemys = new List<Enemy>();
    


    public Waypoints goal;

    private void Awake()
    {
        instance = this;
    }

    public void AddEnemy(Enemy b)
    {
        if (!enemys.Contains(b))
            enemys.Add(b);
    }

    public void SearchMode(Enemy exclude)
    {
        foreach(Enemy boid in enemys)
        {
            if (boid == exclude) continue;
            boid.Search();
        }
    }
    public void ReturnMode(Enemy exclude)
    {
        foreach (Enemy boid in enemys)
        {
            if (boid == exclude) continue;
            boid.ComeBack();
        }
    }
    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, maskWall);
    }

    
}
