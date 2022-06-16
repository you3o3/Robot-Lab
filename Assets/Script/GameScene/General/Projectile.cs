using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 1;
    private bool hit;
    private int direction;
    private float lifeTime;
    private BoxCollider2D boxCollider;
    private int playerLayer, enemyLayer;

    private bool isPlayerProjectile;

    private AudioPlayer audioPlayer;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        audioPlayer = GetComponent<AudioPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        transform.Translate(speed * direction * Time.deltaTime, 0, 0);

        lifeTime += Time.deltaTime;
        if (lifeTime > 5) gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        audioPlayer.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;

        if (collision.gameObject.layer == playerLayer && isPlayerProjectile)
        {
            Debug.LogError("player hit by own projectile");
        }

        if (collision.gameObject.layer == playerLayer && !isPlayerProjectile)
        {
            collision.GetComponent<PlayerInteraction>().PlayerDie();
        }
        else if (collision.gameObject.layer == enemyLayer)
        {
            collision.GetComponent<EnemyInteraction>().EnemyDie();
        }

        gameObject.SetActive(false);
    }

    public void FireThisProjectile(int direction, float speed, bool isPlayerProjectile = false)
    {
        if (!(direction == -1 || direction == 1)) Debug.LogError("direction is not -1 or 1");
        this.direction = direction;
        this.speed = speed;

        gameObject.SetActive(true);
        lifeTime = 0;
        hit = false;
        boxCollider.enabled = true;
        this.isPlayerProjectile = isPlayerProjectile;

        float x = Mathf.Abs(transform.localScale.x) * direction;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

}
