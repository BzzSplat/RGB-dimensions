using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;

public class Stabilizer : Interactable
{
    public bool setRed, red, setGreen, green, setBlue, blue, powered;
    [SerializeField] VisualEffect zapEffect;
    [SerializeField] AudioClip zapSound;
    bool shouldZap, canShift = true;
    [SerializeField] float cooldown;

    private void Awake()
    {
        //switch the power twice to make sure things are properly enabled or disabled
        Interact();
        Interact();
    }

    public override void Interact()
    {
        if (powered)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<AudioSource>().Stop();
            GetComponent<Volume>().enabled = false;
            powered = !powered;
        }
        else
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<AudioSource>().Play();
            GetComponent<Volume>().enabled = true;
            powered = !powered;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!canShift)
            return;

        // get and check for the player's ColorShift script
        ColorShift shifter = other.GetComponent<ColorShift>();
        if (shifter == null)
            return;

        // if you want to set the color and the color is not set to what you want change it
        if (setRed && (red != shifter.red))
        {
            shifter.toggleRed();
            shouldZap = true;
        }

        if (setGreen && (green != shifter.green))
        {
            shifter.toggleGreen();
            shouldZap = true;
        }

        if (setBlue && (blue != shifter.blue))
        {
            shifter.toggleBlue();
            shouldZap = true;
        }

        //zap the player
        if (shouldZap)
        {
            if(zapSound)
                GetComponent<AudioSource>().PlayOneShot(zapSound);
            zapEffect.Play();
            shouldZap = false;

            StartCoroutine("cool");
        }
    }

    private IEnumerator cool ()
    {
        canShift = false;
        yield return new WaitForSeconds(cooldown);
        canShift = true;
    }
}
