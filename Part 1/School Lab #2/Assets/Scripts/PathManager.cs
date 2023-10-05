using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{


    [HideInInspector]
    [SerializeField] public List<Waypoint> path;
    int currentPointIndex = 0;
    public List<GameObject> prefabPoints;

    public GameObject Prefab;


    public List<Waypoint> GetPath()
    {
        if (path == null)
        {
            path = new List<Waypoint>();
        }
        return path;
    }

    public void CreateAddPoint()
    {
        Waypoint go = new Waypoint();
        path.Add(go);
    }

    public Waypoint GetNextTarget()
    {
        int nextPointIndex = (currentPointIndex + 1) % (path.Count);
        currentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }
    private void Start()
    {
        prefabPoints = new List<GameObject>();
        foreach (Waypoint p in path)
        {
            GameObject go = Instantiate(Prefab);
            go.transform.position = p.GetPos();
            prefabPoints.Add(go);
        }

    }
    public void Update()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Waypoint p = path[i];
            GameObject g = prefabPoints[i];
            g.transform.position = p.GetPos();
        }

    }
}
