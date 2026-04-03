using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public bool cheating;
    [HideInInspector] public int clocksCollected;
    public Transform[] unstable;
    public float instabilityPer;
    public float timeSlowPer;
    [HideInInspector] public bool murder;
    public float murderScalar;

    public Image mainUI;

    public TextMeshProUGUI timerText;
    public float timer;

    public GameObject[] bars;
    public Image[] barSprites;
    public Transform[] subUnstable;
    public float subInstability;
    private float deathScalar;
    private bool dead;

    public Player player;

    void Start()
    {
        if (cheating) clocksCollected = 6;
    }

    void Update()
    {
        if (Time.timeScale >= 0.5f)
        {
            foreach (Transform t in unstable)
            {
                t.localScale = Vector3.one * Random.Range(1, 1 + clocksCollected * instabilityPer);
            }
            foreach (Transform t in subUnstable)
            {
                if (t.gameObject.activeInHierarchy)
                {
                    t.localScale = Vector3.one * Random.Range(1, subInstability);
                }
            }
        }

        if (!murder && player.doneEdging) timer = Mathf.Max(timer-Time.deltaTime * (1 - timeSlowPer * clocksCollected), 0);
        if(murder) timer = Mathf.Max(timer - Time.unscaledDeltaTime * murderScalar, 0);
        timerText.text = ((long)timer / 60).ToString("D2") + ":" + ((long)timer % 60).ToString("D2") + "." + ((long)(timer % 1 * 10000)).ToString("D4");
        if (timer == 0)
        {
            if (!dead) {
                Time.timeScale = 1;
                player.canInput = false;
                dead = true;
                foreach (Transform t in subUnstable)
                {
                    t.gameObject.SetActive(false);
                }
                foreach (Transform t in unstable)
                {
                    t.gameObject.SetActive(false);
                }
                deathScalar = -5;
            }
            deathScalar += Time.deltaTime * 5;
            Color deadC = new Color(1, 1, 1, 1 - deathScalar);
            foreach (Image s in barSprites)
            {
                s.color = deadC;
            }
            mainUI.color = deadC;
            timerText.color = deadC;
        }
    }
}
