using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControler : MonoBehaviour
{
    public float camSpeed;
    public Transform cam;
    public Transform text;
    public AudioSource hum;
    public Unstable unstables;
    public Image blocker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cam.Translate(Vector3.left * camSpeed*Time.deltaTime);
        Vector3 normMouse = new Vector3(Input.mousePosition.x / Screen.width - 0.5f, Input.mousePosition.y / Screen.height - 0.5f);
        //if hovering
        if (Mathf.Abs(normMouse.x) < 390f / 1920f / 1.5f && Mathf.Abs(normMouse.y + 367f / 1080f) < 75f / 1080f / 1.5f)
        {
            unstables.instability = Mathf.Clamp(unstables.instability + Time.unscaledDeltaTime / 10, 1.1f, 1.4f);
            if (unstables.instability >= 1.4f)
            {
                SceneManager.LoadScene("Main");
            }
        }
        else
        {
            unstables.instability = Mathf.Clamp(unstables.instability - Time.unscaledDeltaTime / 5, 1.1f, 1.4f);
        }
        hum.volume = (1 + (1.1f - unstables.instability) / .3f) / 2f;
        blocker.color = new Color(0, 0, 0, (unstables.instability - 1.1f) / .3f);
    }
}
