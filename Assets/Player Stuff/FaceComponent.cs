using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceComponent : MonoBehaviour
{
    public float scalar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 normMouse = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f);
        transform.localPosition = Vector3.zero;
        transform.position += normMouse * scalar * Time.timeScale;
    }
}
