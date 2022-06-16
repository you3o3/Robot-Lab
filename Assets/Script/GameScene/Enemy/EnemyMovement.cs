using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header ("Patrol points")]
    [SerializeField] private Transform leftEnd;
    [SerializeField] private Transform rightEnd;
    private float leftX, rightX;

    [Header("Movements")]
    [SerializeField] private float speed;
    [SerializeField] private float idleDuration;
    private float idleTimer;
    private bool isMoveLeft, isIdle;

    [Header("Animation")]
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isMoveLeft = false;
        isIdle = false;
        idleTimer = 0;
        leftX = leftEnd.position.x;
        rightX = rightEnd.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.PlayerGaming) return;
        if (isIdle)
        {
            if (idleTimer > idleDuration)
            {
                isIdle = false;
            }
            else
            {
                idleTimer += Time.deltaTime;
                return;
            }
        }

        if (isMoveLeft)
            Move(-1);
        else
            Move(1);

        if ((isMoveLeft && transform.position.x <= leftX) ||
            (!isMoveLeft && transform.position.x >= rightX))
            ChangeDirection();
    }

    private void ChangeDirection()
    {
        anim.SetBool("moving", false);
        isMoveLeft = !isMoveLeft;
        isIdle = true;
    }

    private void Move(int direction)
    {
        if (!(direction == -1 || direction == 1)) Debug.LogError("direction is not -1 or 1");

        idleTimer = 0;
        isIdle = false;
        anim.SetBool("moving", true);

        float x = Mathf.Abs(transform.localScale.x) * direction;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);

        transform.position = new Vector3(transform.position.x + Time.deltaTime * direction * speed
            , transform.position.y, transform.position.z);
    }

}
