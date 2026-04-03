using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Rigidbody thingyRB;
    public ConfigurableJoint thingyJoint;
    public ParticleSystem thingyParticles;
    public Image edge;
    public Transform cam;
    public Transform circlerHolder;
    public Circler[] circlers;
    public Circler finalCircler;
    public UIManager UI;

    [HideInInspector] public Transform thingyT;
    [HideInInspector] public Rigidbody rb;
    private SpriteRenderer sprite;

    public float edgeRadiusScalar;
    public float grabRadius;
    public float reelSpeed;
    public float reelRatio;
    public float thingyBaseSpeed;
    public float maxSpin;
    public float spinAccel;
    public bool canInput;

    private float spinSpeed;
    private float linearLimit;
    private bool thingyCorrected;
    private float circlerA = -1;
    private bool inEndgame;
    [HideInInspector] public bool doneEdging;

    private int circlerNumber;
    public void addCircler(Transform t)
    {
        circlers[circlerNumber++].target = t;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        thingyT = thingyRB.transform;
        linearLimit = 1;
    }

    void Update()
    {
        //Circler main
        circlerHolder.position = transform.position;

        //Camera
        cam.position = transform.position + new Vector3(0,0, cam.position.z);

        //Inputs
        Vector3 screenCenter = new Vector2(Screen.width, Screen.height) / 2;
        //Centered
        if (canInput && ((Input.mousePosition - screenCenter).magnitude < Screen.height * edgeRadiusScalar))
        {
            if (!AudioManager.Instance.spin.isPlaying) AudioManager.Instance.spin.Play();
            if (circlerA < 1) circlerA += Time.unscaledDeltaTime / 2;
            Color newColor = new Color(1,1,1, circlerA);
            foreach(Circler c in circlers) {
                c.sprite.color = newColor;
            }
            if (inEndgame) finalCircler.sprite.color = newColor;
            spinSpeed = Mathf.Clamp(spinSpeed + spinAccel * Time.deltaTime, 0, maxSpin);
            Vector3 targetPos = transform.position + (transform.right * -1 * 1) * grabRadius * .95f;
            float eReel = reelRatio * Time.timeScale;
            if (!thingyCorrected)
            {
                thingyT.position = thingyT.position * (1 - eReel) + targetPos * eReel;
            } else
            {
                thingyT.position = targetPos;
            }
            if ((thingyT.position - targetPos).magnitude < 0.05) thingyCorrected = true;
            linearLimit = Mathf.Clamp(linearLimit - reelSpeed * Time.deltaTime, grabRadius, 4);
            SoftJointLimit thingyLinear = new SoftJointLimit();
            thingyLinear.limit = linearLimit;
            thingyJoint.linearLimit = thingyLinear;
            transform.Rotate(0, 0, spinSpeed * 180 / Mathf.PI * Time.deltaTime);
        } else 
        {
            //Release
            if(spinSpeed > 0)
            {
                AudioManager.Instance.spin.Stop();
                AudioManager.Instance.PlayShootout();
                doneEdging = true;
                thingyCorrected = false;
                linearLimit = 4;
                SoftJointLimit thingyLinear = new SoftJointLimit();
                thingyLinear.limit = linearLimit;
                thingyJoint.linearLimit = thingyLinear;

                Vector3 normalizedMouse = new Vector3((Input.mousePosition.x - screenCenter.x) / Screen.width,
                                                    (Input.mousePosition.y - screenCenter.y) / Screen.height);
                thingyRB.velocity = normalizedMouse * Mathf.Max(spinSpeed, 1) * thingyBaseSpeed;
                spinSpeed = 0;
            }
            transform.rotation = Quaternion.FromToRotation(Vector3.left, thingyT.position - transform.position);

            circlerA = Mathf.Clamp(circlerA - Time.unscaledDeltaTime, -1, 1);
            Color newColor = new Color(1, 1, 1, circlerA);
            foreach (Circler c in circlers)
            {
                c.sprite.color = newColor;
            }
            if (inEndgame) finalCircler.sprite.color = newColor;
        }
        thingyT.rotation = transform.rotation;

        if (doneEdging)
        {
            edge.color = new Color(1, 1, 1, edge.color.a - Time.unscaledDeltaTime / 2);
        }

        //endgame sequence
        if(inEndgame || UI.clocksCollected == 6)
        {
            inEndgame = true;
        }
    }
}
