using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;

    private float playerScaleX;
    private Animator anim;
    private float horizontalInput;
    private int prevdirection = 1, direction = 1;

    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.PlayerGaming) return;
        if (GameManager.Instance.IsGamePaused()) return;

        anim.SetBool("grounded", IsGrounded());

        //Test();
        // CollisionCheck();

        //if (Input.GetKey(KeyCode.U))
        //{
        //    DisplayTestResult();
        //}

        prevdirection = direction;

        ////////////////////////////////////////////////////////////////
        /// Manual operation
        ////////////////////////////////////////////////////////////////

        if (!RobotData.Instance.IsTestRun) return;
        if (playerInteraction.PlayerDisabled)
        {
            Move(0);
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        Move(horizontalInput);

        if (Input.GetKey(KeyCode.K))
        {
            Jump();
        }
    }

    public void Move(float value)
    {
        //transform.Translate(new Vector3(value, 0, 0) * speed * Time.deltaTime);
        if (value > 0) value = 1;
        else if (value < 0) value = -1;

        body.velocity = new Vector2(value * speed, body.velocity.y);
        anim.SetBool("run", value != 0);

        // Flip character when move left
        if (value > 0.01f)
        {
            direction = 1;
        }
        else if (value < -0.01f)
        {
            direction = -1;
        }
        transform.localScale = new Vector3(direction * playerScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        if (raycastHit.collider != null)
        {
            return true;
        }
        return false;
    }

    public bool CanAttack()
    {
        // there is a bug where robot hitting it's own projectile when fliping direction.
        // Therefore wait for one frame time before can fire
        return prevdirection == direction;
    }


    private float prevX, prevY;

    private float maxDistance;
    private float maxHeight;
    private bool started;

    private void Test()
    {
        if (IsGrounded())
        {
            started = false;
            return;
        }
        if (!started)
        {
            prevX = transform.position.x;
            prevY = transform.position.y;
            started = true;
        }

        maxDistance = Mathf.Max(maxDistance, Mathf.Abs(prevX - transform.position.x));
        maxHeight = Mathf.Max(maxHeight, Mathf.Abs(prevY - transform.position.y));
    }

    private void DisplayTestResult()
    {
        Debug.Log("max Distance: " + maxDistance);
        Debug.Log("max Height: " + maxHeight);
    }

    private void CollisionCheck()
    {
        RaycastHit2D raycastHit1 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.left, 0.1f, groundLayer);
        RaycastHit2D raycastHit2 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.right, 0.1f, groundLayer);
        RaycastHit2D raycastHit3 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, groundLayer);

        if (raycastHit1.collider != null)
        {
            Debug.Log("Hit on left: ", raycastHit1.collider);
        }
        if (raycastHit2.collider != null)
        {
            Debug.Log("Hit on right: ", raycastHit2.collider);
        }
        if (raycastHit3.collider != null)
        {
            Debug.Log("Hit on top: ", raycastHit3.collider);
        }
    }
}
