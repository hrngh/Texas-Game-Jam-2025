using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    public float range;
    public float gravConstant;
    private Player player;
    public AudioSource hum;
    private float humPitch;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        humPitch = hum.pitch;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hum.pitch = humPitch * Time.timeScale;
        Vector3 fromThingy = transform.position - player.thingyT.position;
        float thingyDistancce = Mathf.Max(fromThingy.magnitude, 0.5f);
        if(thingyDistancce < range)
        {
            player.thingyRB.AddForce(fromThingy.normalized * gravConstant/Mathf.Pow(thingyDistancce, 2));
        }
        Vector3 fromPlayer = transform.position - player.transform.position;
        float playerDistance = Mathf.Max(fromPlayer.magnitude, 0.5f);
        if (playerDistance < range)
        {
            player.rb.AddForce(fromPlayer.normalized * gravConstant / Mathf.Pow(playerDistance, 2));
        }
        if(playerDistance < 8f)
        {
            Time.timeScale = playerDistance / 20f + 0.6f;
            AudioManager.Instance.music.pitch = Time.timeScale;
        }
        else if (playerDistance < 10f)
        {
            Time.timeScale = 1;
            AudioManager.Instance.music.pitch = Time.timeScale;
        }
    }
}
