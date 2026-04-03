using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Transform cam;
    public float xScale;
    public float yScale;

    void Update()
    {
        if(transform.position.x - cam.position.x > xScale)
        {
            transform.position = new Vector3(transform.position.x - xScale*2, cam.position.y + Random.Range(-yScale, yScale), transform.position.z);
        } 
        else if(cam.position.x - transform.position.x > xScale)
        {
            transform.position = new Vector3(transform.position.x + xScale * 2, cam.position.y + Random.Range(-yScale, yScale), transform.position.z);
        } 
        else if(transform.position.y - cam.position.y > yScale)
        {
            transform.position = new Vector3(cam.position.x + Random.Range(-xScale, xScale), transform.position.y - yScale * 2, transform.position.z);
        }
        else if (cam.position.y - transform.position.y > yScale)
        {
            transform.position = new Vector3(cam.position.x + Random.Range(-xScale, xScale), transform.position.y + yScale * 2, transform.position.z);
        }
    }
}
