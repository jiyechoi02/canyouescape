using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public GameObject[] waypoints;
    int current = 0;
    float rotSpeed;
    public float speed;
    float WPradius = 1;

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(waypoints[current].transform.position, transform.position) < WPradius)
        {
            current++;
            if(current >= waypoints.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
        //Debug.Log(waypoints[current]);
        if (waypoints[current] == waypoints[0])
        {
            transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
        }
        if (waypoints[current] == waypoints[1])
        {
            transform.rotation = Quaternion.AngleAxis(270, Vector3.up);
        }
    }
}
