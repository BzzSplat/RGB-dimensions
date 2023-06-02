using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] float grabDist, loseDist, drag;
    public Transform grabbed; //public for possible reactions when picking up certain objects
    private float objDrag, objAngDrag;
    private Transform spring;
    private ColorShift shifter;
    [SerializeField] LayerMask mask;

    private void Awake()
    {
        spring = transform.GetChild(1);
        shifter = transform.GetComponentInParent<ColorShift>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(Controls.Instance.ControlCodes[0]))
        {
            if (grabbed == null)
                Grab();
            else
                Drop();
        }

        //if grabbed object gets too far from hold position, drop it
        if (grabbed != null && Vector3.Distance(spring.position, grabbed.position) > loseDist)
            Drop();
    }

    private void Grab()
    {
        if (grabbed != null) //don't grab if already holding
            return;

        //create layermask depending on allowed currently colors. There has to be a better way, right?
        mask = 1 << 6;
        if (shifter.red)
        {
            mask |= (1 << 7);
            mask |= (1 << 11);
            mask |= (1 << 13);
        }
        if (shifter.green)
        {
            mask |= (1 << 8);
            mask |= (1 << 11);
            mask |= (1 << 12);
        }
        if (shifter.blue)
        {
            mask |= (1 << 9);
            mask |= (1 << 12);
            mask |= (1 << 13);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, grabDist, mask) && hit.transform.CompareTag("Grabbable"))
        {
            Rigidbody rb = hit.transform.GetComponent<Rigidbody>();

            Debug.Log("Grabbed " + hit.transform.gameObject.name);
            spring.GetComponent<SpringJoint>().connectedBody = rb;

            grabbed = hit.transform;

            //store original drag values then replace
            objDrag = rb.drag;
            rb.drag = drag;
            objAngDrag = rb.angularDrag;
            rb.angularDrag = drag;
        }
    }

    private void Drop()
    {
        if (grabbed == null)
            return;

        Rigidbody rb = grabbed.GetComponent<Rigidbody>();
        grabbed = null;

        rb.drag = objDrag;
        rb.angularDrag = objAngDrag;
        spring.GetComponent<SpringJoint>().connectedBody = null;
    }

    //called when shifting. If player no longer collides with the grabbed object, drop it
    public void Check()
    {
        if (grabbed != null && Physics.GetIgnoreLayerCollision(10, grabbed.gameObject.layer))
            Drop();
    }
}
