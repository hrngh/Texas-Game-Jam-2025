using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Transform[] unstable;
    public float instability;
    public ParticleSystem particles;
    public AnimationCurve suckDistance;
    public float suckDistanceScalar;
    public float suckTime;
    public GameObject flash;
    public AudioSource hum;
    public float moveSpeed;
    private float humPitch;

    private Transform thingyT;
    private Player player;
    private UIManager UI;
    private Vector3 spinCenter;
    private float spinDir;

    private float distanceLerp;
    private bool dead;
    void Start()
    {
        thingyT = GameObject.Find("Thingy").transform;
        UI = GameObject.Find("UI").GetComponent<UIManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        player.addCircler(transform);
        humPitch = hum.pitch;
        float spinCenterFactor = Random.Range(0f, 1f);
        spinDir = Random.Range(0,2)*2-1;
        spinCenter = (1 - spinCenterFactor) * transform.position + spinCenterFactor * new Vector3(0, -15f);
    }

    // Update is called once per frame
    void Update()
    {
        hum.pitch = humPitch * Time.timeScale;
        if (Time.timeScale >= 0.5f)
        {
            foreach (Transform t in unstable)
            {
                t.localScale = Vector3.one * Random.Range(1, instability);
            }
        }

        if (dead)
        {
            distanceLerp -= Time.unscaledDeltaTime;
            if (distanceLerp > 0)
            {
                Time.timeScale = Mathf.Max(distanceLerp / suckTime, 0.2f);
                AudioManager.Instance.music.pitch = Time.timeScale;
                transform.position = thingyT.position + (transform.position - thingyT.position).normalized * suckDistance.Evaluate((suckTime - distanceLerp) / suckTime) * suckDistanceScalar;
            } else
            {
                Time.timeScale = 1;
                AudioManager.Instance.music.pitch = 1;
                Instantiate(flash, thingyT);
                UI.bars[UI.clocksCollected].SetActive(true);
                UI.clocksCollected++;
                AudioManager.Instance.PlayPickup();
                var e1 = player.thingyParticles.emission.rateOverTime;
                var e2 = player.thingyParticles.emission;
                e1.constant += 7;
                e2.rateOverTime = e1;
                Destroy(gameObject);
            }
        } else
        {
            Vector3 toSpin = Vector3.Cross(spinCenter - transform.position, Vector3.forward).normalized * moveSpeed * spinDir;
            transform.Translate(toSpin * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!dead && other.tag == "Player")
        {
            particles.Stop();
            distanceLerp = suckTime;
            dead = true;
        }
    }
}
