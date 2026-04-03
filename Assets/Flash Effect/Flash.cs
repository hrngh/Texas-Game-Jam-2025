using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public float lifetime;
    public SpriteRenderer sprite;
    public Image image;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        if (image) image.color = Color.black;
        timer = lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
        if (sprite) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, timer/lifetime);
        if (image) image.color = new Color(image.color.r, image.color.g, image.color.b, timer / lifetime);
    }
}
