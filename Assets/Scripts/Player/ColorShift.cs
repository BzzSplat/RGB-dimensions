using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorShift : MonoBehaviour
{
    [SerializeField] bool unlockedRed, unlockedGreen, unlockedBlue, blackLocked;
    public bool red = true, green = true, blue = true, cooldown = true;
    [SerializeField] MaterialStorage mats;
    Animator anim;
    [SerializeField] float shiftDelay, cooldownTime;
    [SerializeField] Image iconR, iconG, iconB;
    AudioSource soundsSource;
    [SerializeField] AudioClip shiftSound;

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        soundsSource = GetComponent<AudioSource>();

        if (!unlockedRed)
            iconR.color = Color.black;
        if (!unlockedGreen)
            iconG.color = Color.black;
        if (!unlockedBlue)
            iconB.color = Color.black;
    }

    private void Update() //what a mess
    {
        //toggle red
        if (Input.GetKeyDown(KeyCode.Alpha1) && unlockedRed && cooldown)
        {
            //stop player from being in no dimension unless they have unlocked that ability
            if (!((!green && !blue) && blackLocked))
            {
                //quick shift check
                if(Input.GetKey(Controls.Instance.ControlCodes[4]) && green)
                    StartCoroutine("togGreen");
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && blue)
                    StartCoroutine("togBlue");
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && !red)
                    StartCoroutine("togRed");

                if (!Input.GetKey(Controls.Instance.ControlCodes[4]))
                    StartCoroutine("togRed");

            }
        }

        //toggle green
        if (Input.GetKeyDown(KeyCode.Alpha2) && unlockedGreen && cooldown)
        {
            if (!((!blue && !red) && blackLocked))
            {
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && red)
                    StartCoroutine("togRed");
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && blue)
                    StartCoroutine("togBlue");
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && !green)
                    StartCoroutine("togGreen");

                if (!Input.GetKey(Controls.Instance.ControlCodes[4]))
                    StartCoroutine("togGreen");
            }
        }

        //toggle blue
        if (Input.GetKeyDown(KeyCode.Alpha3) && unlockedBlue && cooldown)
        {
            if (!((!red && !green) && blackLocked))
            {
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && green)
                    StartCoroutine("togGreen");
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && red)
                    StartCoroutine("togRed");
                if (Input.GetKey(Controls.Instance.ControlCodes[4]) && !blue)
                    StartCoroutine("togBlue");

                if (!Input.GetKey(Controls.Instance.ControlCodes[4]))
                    StartCoroutine("togBlue");
            }
        }
    }

    private IEnumerator togRed()
    {
        anim.Play("ColorShift");
        soundsSource.PlayOneShot(shiftSound);
        StartCoroutine("CoolDown");

        yield return new WaitForSeconds(shiftDelay);
        red = !red;
        mats.shiftColor("_Red", red);
        Physics.IgnoreLayerCollision(10, 7, !red); //!red because I check to see if red is on, it checks to see if red is off
        checkOthers();

        if (red)
            iconR.color = Color.red;
        else
            iconR.color = new Color(0.3f, 0, 0);
    }
    private IEnumerator togGreen()
    {
        anim.Play("ColorShift");
        soundsSource.PlayOneShot(shiftSound);
        StartCoroutine("CoolDown");

        yield return new WaitForSeconds(shiftDelay);
        green = !green;
        mats.shiftColor("_Green", green);
        Physics.IgnoreLayerCollision(10, 8, !green);
        checkOthers();

        if (green)
            iconG.color = Color.green;
        else
            iconG.color = new Color(0, 0.3f, 0);
    }
    private IEnumerator togBlue()
    {
        anim.Play("ColorShift");
        soundsSource.PlayOneShot(shiftSound);
        StartCoroutine("CoolDown");

        yield return new WaitForSeconds(shiftDelay);
        blue = !blue;
        mats.shiftColor("_Blue", blue);
        Physics.IgnoreLayerCollision(10, 9, !blue);
        checkOthers();

        if (blue)
            iconB.color = Color.blue;
        else
            iconB.color = new Color(0, 0, 0.3f);
    }
    private void checkOthers()
    {
        //if no color is on don't collide with white
        Physics.IgnoreLayerCollision(10, 6, !(red || green || blue));

        //check collision for between colors
        Physics.IgnoreLayerCollision(10, 11, !red && !green); //yellow
        Physics.IgnoreLayerCollision(10, 12, !green && !blue); //teal
        Physics.IgnoreLayerCollision(10, 13, !blue && !red); //purple

        //check objects being held
        transform.GetChild(0).GetComponent<PlayerGrab>().Check();
    }

    IEnumerator CoolDown()
    {
        cooldown = false;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = true;
    }

    public void toggleRed()
    {
        StartCoroutine("togRed");
    }
    public void toggleGreen()
    {
        StartCoroutine("togGreen");
    }
    public void toggleBlue()
    {
        StartCoroutine("togBlue");
    }
}
