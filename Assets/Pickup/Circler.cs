using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circler : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Transform target;

    void Update()
    {
        if(target) transform.localRotation = Quaternion.FromToRotation(Vector3.right, target.position - transform.parent.position);
        if(!target) transform.localScale = Vector3.zero;
    }
}
