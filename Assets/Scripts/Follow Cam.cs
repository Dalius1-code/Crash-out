using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    public float speed = 10f;

    [Range(0f, 1f)]
    public float smoothness = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var targetposition = Vector3.MoveTowards(transform.position, target.position + offset, speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetposition, 1 - smoothness);


    }
}