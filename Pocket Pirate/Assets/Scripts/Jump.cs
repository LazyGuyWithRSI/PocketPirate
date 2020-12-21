using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IJump
{
    public float JumpForce = 100f; // TODO scriptable obj, obv (obviously)
    public float JumpCooldown = 3f;
    private Rigidbody rb;

    private bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void DoJump ()
    {
        if (canJump)
        {
            Debug.Log("Jumping!");
            rb.AddForce(Vector3.up * JumpForce);
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
