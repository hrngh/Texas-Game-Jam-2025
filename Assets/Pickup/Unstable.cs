using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unstable : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Image image;
    public SpriteRenderer[] unstable;
    public Transform[] unstable2;
    public Image[] unstable3;
    public float instability;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale >= 0.5f)
        {
            if (sprite)
            {
                Color newColor = new Color(1, 1, 1, sprite.color.a / 3);
                foreach (SpriteRenderer t in unstable)
                {
                    t.transform.localScale = Vector3.one * Random.Range(1, instability);
                    t.color = newColor;
                }
            } 
            else if (image)
            {
                Color newColor = new Color(1, 1, 1, image.color.a / 3);
                foreach (Image t in unstable3)
                {
                    t.transform.localScale = Vector3.one * Random.Range(1, instability);
                    t.color = newColor;
                }
            }
            else
            {
                foreach (Transform t in unstable2)
                {
                    t.localScale = Vector3.one * Random.Range(1, instability);
                }
            }
        }
    }
}
