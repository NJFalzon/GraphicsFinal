using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour
{
    [SerializeField] KeyCode forward;
    [SerializeField] KeyCode forwardAlternate;
    [Space]
    [SerializeField] KeyCode backward;
    [SerializeField] KeyCode backwardAlternate;
    [Space]
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode leftAlternate;
    [Space]
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode rightAlternate;
    [Space]
    [SerializeField] KeyCode jump;
    [SerializeField] KeyCode jumpAlternate;
    [Space]
    [SerializeField] float force;

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Jump();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Jump()
    {
        if (Input.GetKeyDown(jump) || Input.GetKeyDown(jumpAlternate))
        {
            rb.AddForce(transform.up * force * 10, ForceMode.Impulse);
        }
    }

    void MoveForward()
    {
        if (Input.GetKey(forward) || Input.GetKey(forwardAlternate))
        {
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
        }
    }

    void MoveBackward()
    {
        if (Input.GetKey(backward) || Input.GetKey(backwardAlternate))
        {
            rb.AddForce(-transform.forward * force, ForceMode.Impulse);
        }
    }

    void MoveRight()
    {
        if (Input.GetKey(right) || Input.GetKey(rightAlternate))
        {
            rb.AddForce(transform.right * force, ForceMode.Impulse);
        }
    }

    void MoveLeft()
    {
        if (Input.GetKey(left) || Input.GetKey(leftAlternate))
        {
            rb.AddForce(-transform.right * force, ForceMode.Impulse);
        }
    }


    void Look()
    {
        float x = Input.GetAxis("Mouse X") * 2;
        float y = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(-y, x, 0));

        float z = transform.eulerAngles.z;
        transform.Rotate(0, 0, -z);
    }

    void Move()
    {
        MoveForward();
        MoveBackward();
        MoveLeft();
        MoveRight();
        LimitVelocity();
    }

    void LimitVelocity()
    {
        if(rb.velocity.magnitude > 20)
        {
            rb.velocity = rb.velocity.normalized*20;
        }
    }
}
