using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    bool canPlayerInteract;

    public void PlayerInteract()
    {
        if (canPlayerInteract)
            Interact();
    }

    public virtual void Interact()
    {

    }
}
