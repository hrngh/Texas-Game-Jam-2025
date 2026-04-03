using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BBHController : MonoBehaviour
{
    public Player player;
    public float killRadius;
    public UIManager UI;

    private float explodeScale;
    public float explodeRate;

    public Transform[] unstable;
    public ParticleSystem particles;
    public GameObject brokenBits;
    public Image endText;
    public Image endBlocker;
    public Unstable endTextU;
    public Transform ropeT;
    public GameObject[] obstacles;
    public SpriteRenderer playerBlocker;
    public SpriteRenderer sprite;
    public float instability;
    public float deathTimer;

    private bool dead;
    private bool inEndgame;
    private bool touched;
    private bool winning;
    private bool freed;

    private float distanceLerp;
    public AnimationCurve suckDistance;
    public float suckTime;

    void Start()
    {

    }

    void Update()
    {
        float playerDist = (player.transform.position - transform.position).magnitude;
        if (!touched && Time.timeScale >= 0.5f)
        {
            foreach (Transform t in unstable)
            {
                t.localScale = Vector3.one * Random.Range(1, instability);
            }
        }

        if ((inEndgame || UI.clocksCollected == 6) && UI.timer > 0)
        {
            //IN ENDGAME
            if (!inEndgame)
            {
                instability = 1.2f;
                var emissionModule = particles.emission.rateOverTime;
                emissionModule.constant *= 2;
                var e2 = particles.emission;
                e2.rateOverTime = emissionModule;
                var lifespanModule = particles.main;
                lifespanModule.startLifetime = 1;
                var spaceModule = particles.shape.radius;
                spaceModule /= 3;
                var r2 = particles.shape;
                r2.radius = spaceModule;
                sprite.sortingOrder = -2;
                sprite.color = Color.white;
                foreach (Transform t in unstable)
                {
                    t.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.3f);
                }
                transform.localScale = Vector3.one * 5;
                killRadius /= 3;
                inEndgame = true;
            }
            if(!touched && playerDist < killRadius + 1)
            {
                touched = true;
                sprite.sortingOrder = 5;
                foreach (Transform t in unstable)
                {
                    t.localScale = Vector3.zero;
                }
                player.canInput = false;
                distanceLerp = suckTime;
                particles.Stop();
            }
            if (touched && !winning)
            {
                player.rb.velocity *= (1 - Time.unscaledDeltaTime);
                player.thingyRB.velocity *= (1 - Time.unscaledDeltaTime);
                distanceLerp -= Time.unscaledDeltaTime;
                if (distanceLerp > 0)
                {
                    Time.timeScale = Mathf.Max(distanceLerp / suckTime, 0.2f);
                    AudioManager.Instance.music.pitch = Time.timeScale;
                    float deathScalar = suckDistance.Evaluate((suckTime - distanceLerp) / suckTime) * (killRadius * 5);
                    transform.position = player.thingyT.position + (transform.position - player.thingyT.position).normalized * deathScalar;
                    transform.localScale = Vector3.one * 5 * Mathf.Max(1-(suckTime-distanceLerp), deathScalar / 3f);
                }
                else
                {
                    Time.timeScale = .6f;
                    AudioManager.Instance.music.volume = 0;
                    distanceLerp = 0;
                    UI.timerText.gameObject.SetActive(false);
                    brokenBits.SetActive(true);
                    winning = true;
                    sprite.color = Color.black;
                    UI.bars[6].SetActive(true);
                    sprite.sortingOrder = -2;
                    obstacles[0].SetActive(false);
                    obstacles[1].SetActive(false);
                    AudioManager.Instance.PlayExplosion();
                }
            }
            if (winning && !freed)
            {
                AudioManager.Instance.music.volume -= Time.unscaledDeltaTime;
                AudioManager.Instance.hum.volume = Mathf.Clamp01(AudioManager.Instance.hum.volume + Time.unscaledDeltaTime);
                distanceLerp += Time.unscaledDeltaTime;
                transform.localScale = Vector3.one * explodeRate * distanceLerp;
                Vector3 normMouse = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f);
                //if hovering
                if (distanceLerp >= 10 && Mathf.Abs(normMouse.x - 535f / 1920f) < 270f / 1920f / 1.5f && Mathf.Abs(normMouse.y - 295f / 1080f) < 50f / 1080f / 1.5f)
                {
                    endTextU.instability = Mathf.Clamp(endTextU.instability + Time.unscaledDeltaTime / 10, 1.2f, 1.5f);
                    AudioManager.Instance.hum.volume = 1 + (1.2f - endTextU.instability) / .3f;
                    endText.color = new Color(1, 1, 1, 1 + (1.2f - endTextU.instability) / .3f);
                    endBlocker.color = new Color(0,0,0, (endTextU.instability - 1.2f) / .3f);
                    ropeT.transform.localScale = new Vector3(ropeT.localScale.x, 1+(1.2f-endTextU.instability)/.3f, 1);
                    if (endTextU.instability >= 1.5f)
                    {
                        SoftJointLimit thingyLinear = new SoftJointLimit();
                        thingyLinear.limit = 100;
                        player.thingyJoint.linearLimit = thingyLinear;
                        brokenBits.SetActive(false);
                        playerBlocker.color = new Color(0,0,0, -1);
                        player.thingyRB.velocity = (player.thingyT.position - player.transform.position).normalized;
                        freed = true;
                    }
                } else
                {
                    endText.color = new Color(1,1,1, distanceLerp/2-5);
                    //endTextU.instability = Mathf.Clamp(endTextU.instability - Time.unscaledDeltaTime / 5, 1.2f, 1.5f);
                }
            }
            if (freed)
            {
                playerBlocker.color = new Color(0,0,0, playerBlocker.color.a + Time.unscaledDeltaTime / 2);
                if (playerBlocker.color.a >= 3.5) Application.Quit();
            }
            return;
        }

        if (UI.timer != 0)
        {
            if (playerDist <= killRadius)
            {
                UI.murder = true;
                player.thingyRB.velocity *= 1 - Time.unscaledDeltaTime * 5;
                player.rb.velocity *= 1 - Time.unscaledDeltaTime * 5;
                if (UI.timer >= 0) Time.timeScale = Mathf.Clamp01(playerDist + 1 - killRadius) / 2;
            }
            else
            {
                UI.murder = false;
            }
            if (playerDist <= killRadius + 2)
            {
                AudioManager.Instance.music.pitch = Time.timeScale;
                Time.timeScale = 0.5f + Mathf.Clamp(playerDist - killRadius, 0, 2) / 4;
            }
        }

        if(UI.timer == 0)
        {
            if (!dead)
            {
                dead = true;
                particles.Stop();
                explodeScale = playerDist / 0.3f - explodeRate;
                AudioManager.Instance.PlayExplosion();
            }
            AudioManager.Instance.music.volume -= Time.deltaTime * 2;
            explodeScale += explodeRate * Time.deltaTime * Mathf.Max(playerDist/20f, 1);
            transform.localScale = Vector3.one * Mathf.Max(explodeScale, 15);
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                SceneManager.LoadScene("Main");
            }
        }


    }
}
