using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.angularVelocity = Vector3.forward * Random.Range(-0.45f, 0.45f);
        float sizeScalar = Random.Range(2f, 16f);
        transform.localScale = Vector3.one * sizeScalar;
        rb.mass = 0.35f*Mathf.Pow(sizeScalar,2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
