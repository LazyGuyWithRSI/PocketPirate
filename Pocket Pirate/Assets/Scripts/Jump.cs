using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IJump
{
    public float JumpForce = 100f; // TODO scriptable obj, obv (obviously)
    public float JumpCooldown = 3f;
    public float VerticalDrag = 0.95f;
    public float AirDrag = 1f;
    private bool inAir = false;

    private Rigidbody rb;

    private bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!inAir && transform.position.y > 0.5f)
        {
            inAir = true;
        }
        else if (inAir && transform.position.y < 0.5f)
        {
            inAir = false;
        }

        Vector3 vel = rb.velocity;

        if (inAir)
            vel.y *= AirDrag;
        else
            vel.y *= VerticalDrag;

        rb.velocity = vel;
    }

    public void DoJump ()
    {
        if (canJump && !inAir)
        {
            Debug.Log("Jumping!");
            rb.AddForce(Vector3.up * JumpForce);
            inAir = true;
            StartCoroutine(DoCooldown());
        }
    }

    private IEnumerator DoCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(JumpCooldown);
        canJump = true;
    }
}

public interface IJump
{
    void DoJump ();
}
