using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Transform from;
    public Transform to;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = from.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, to.position-from.position);
        transform.localScale = new Vector3((to.position-from.position).magnitude * 24,transform.localScale.y,1);
    }
}
