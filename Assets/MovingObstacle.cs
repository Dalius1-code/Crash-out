using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed; 
    private Vector3 targetPos; 

    public GameObject ways; 
    private Transform[] wayPoints; 
    private int pointIndex; 
    private int pointCount; 
    private int direction; 

    private void Awake()
    {

        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i);
        }
    }

    private void Start()
    {

        pointCount = wayPoints.Length;

        if (pointCount > 0)
        {
            pointIndex = 0;
            direction = 1; 
            targetPos = wayPoints[pointIndex].position;
        }
        else
        {
            Debug.LogError("No waypoints assigned under 'ways'. Ensure 'ways' has child objects.");
        }
    }

    private void Update()
    {
        if (pointCount > 0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                NextPoint();
            }
        }
    }

    private void NextPoint()
    {

        if (pointIndex == pointCount - 1)
        {
            direction = -1;
        }
        else if (pointIndex == 0)
        {
            direction = 1;
        }

 
        pointIndex += direction;
        targetPos = wayPoints[pointIndex].position;
    }
}