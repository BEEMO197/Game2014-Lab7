using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumController : MonoBehaviour
{
    public float runForce;
    public Transform lookAheadBelowPoint;
    public Transform lookAheadPoint;
    public LayerMask platformCollisionLayer;
    public LayerMask wallCollisionLayer;
    public bool isGroundAhead = true;
    public bool rampInFront = false;
    public bool rampBelow = false;
    public Rigidbody2D rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAheadBelow();
        LookAhead();
        Move();
    }

    private void LookAheadBelow()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookAheadBelowPoint.position, platformCollisionLayer);
        if (groundHit)
        {
            isGroundAhead = true;
            if(groundHit.collider.CompareTag("Ramps"))
            {
                rampBelow = true;
            }
            else if (groundHit.collider.CompareTag("Platforms"))
            {
                rampBelow = false;
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0.0f);
            }

            if (rampInFront && rampBelow)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 30.0f * -transform.localScale.x);
            }
            else if(rampBelow)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 30.0f * transform.localScale.x);
            }

        }
        else
        {
            isGroundAhead = false;
        }
        Debug.DrawLine(transform.position, lookAheadBelowPoint.position, Color.cyan);
    }

    private void LookAhead()
    {
        var frontHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, wallCollisionLayer);

        if (frontHit)
        {
            if (frontHit.collider.CompareTag("Ramps"))
            {
                rampInFront = true;
            }
            if (frontHit.collider.CompareTag("Walls"))
            {
                FlipX();
            }
        }
        else if(!rampBelow)
        {
            rampInFront = false;
        }
        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.red);

    }

    private void Move()
    {
        if (isGroundAhead)
        {
            rigidBody2D.AddForce(-transform.right * runForce * transform.localScale.x);
        }

        else
        {
            FlipX();
        }
    }

    private void FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }
}
