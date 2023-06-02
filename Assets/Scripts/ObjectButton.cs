using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ObjectButton : MonoBehaviour
{
    [SerializeField] Interactable triggeredObject;
    [SerializeField] bool invert, triggerOnExit;
    [SerializeField] Animator anim;
    [SerializeField] VisualEffect zapEffect;
    [SerializeField] float requiredWeight = 0;
    private float currentWeight = 0f;
    private bool activated = false; 

    private void OnCollisionEnter(Collision collision)
    {
        //check if button has already been activated and if it has the required amount of weight
        currentWeight += collision.rigidbody.mass;
        if (!activated && currentWeight >= requiredWeight)
        {
            activated = true;

            if (triggeredObject)
                triggeredObject.Interact();

            if(zapEffect != null)
                zapEffect.Play();

            if (anim != null)
                anim.SetBool("pressed", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        currentWeight -= collision.rigidbody.mass;
        if (activated && currentWeight < requiredWeight)
        {
            activated = false;

            if (triggeredObject && triggerOnExit)
                triggeredObject.Interact();

            if (zapEffect != null)
                zapEffect.Stop();

            if (anim != null)
                anim.SetBool("pressed", false);
        }
    }
}
