using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public GameObject star;
    public Transform cam;
    public float minZ;
    public float maxZ;
    public float xScale;
    public float yScale;
    public float starCount;
    void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            Star s = Instantiate(star).GetComponent<Star>();
            float z = Random.Range(minZ, maxZ);
            float normZ = (z - minZ) / (maxZ - minZ);
            s.transform.position = new Vector3(Random.Range(-xScale, xScale), Random.Range(-yScale, yScale), z);
            s.cam = cam;
            s.xScale = xScale;
            s.yScale = yScale;
            s.transform.localScale = Vector3.one * (0.25f - normZ / 10);
            s.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 1 - normZ / 2);
        }
    }
}
