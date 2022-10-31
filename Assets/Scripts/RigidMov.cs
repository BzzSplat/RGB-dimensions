using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidMov : MonoBehaviour
{
    Rigidbody rb;
    public float Speed; //normal 12

    float x;
    float z;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();  
    }

    void FixedUpdate()
    {
        if(x!=0 && z != 0)
        {
            x /= 2;
            z /= 2;
        }

        Vector3 move = transform.right * x + transform.forward * z; //creates direction local to the player's rotation
        move *= Speed;
        move.y = rb.velocity.y;

        rb.velocity = move;

    }

    private void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }
}
