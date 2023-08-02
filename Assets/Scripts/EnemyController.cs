using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    private Transform target;

    public float damage;

    public float hitWaitTime = 1f;
    private float hitCounter;

    public float health = 5f;

    public float knockBackTime = .5f;
    private float knockBackCounter;

    public int expToGive = 1;

    public int coinValue = 1;
    public int magnetValue = 10000;
    public float coinDropRate = .5f;
    public float xpDropRate = .5f;
    public float magnetDropRate = .5f;

    public GameObject Player;

    public bool flip;

    public Animator animator;

    public float delay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //target = FindObjectOfType<PlayerController>().transform;
        target = PlayerHealthController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.gameObject.activeSelf == true)
        {
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2f;
                }

                if (knockBackCounter <= 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed * .5f);
                }
            }

            theRB.velocity = (target.position - transform.position).normalized * moveSpeed;

            if (hitCounter > 0f)
            {
                hitCounter -= Time.deltaTime;
            }
        } else
        {
            theRB.velocity = Vector2.zero;
            Destroy(this);
        }

        Vector3 scale = transform.localScale;

        Player = GameObject.Find("Player");

        if (Player.transform.position.x < transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
        }

        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);

            hitCounter = hitWaitTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;

        if(health <= 0)
        {
            knockBackCounter = knockBackTime;
            this.CallWithDelay(DestroyGameObject,delay);
            CinemachineShake.Instance.ShakeCamera(5f, .1f);

            if(Random.value <= xpDropRate)
            {
                ExperienceLevelController.instance.SpawnExp(transform.position, expToGive);
            }

            if(Random.value <= coinDropRate)
            {
                CoinController.instance.DropCoin(transform.position, coinValue);
            }

            if(Random.value <= magnetDropRate)
            {
                PlayerStatController.instance.DropMagnet(transform.position, magnetValue);
            }

            SFXManager.instance.PlaySFXPitched(0);
        } else
        {
            SFXManager.instance.PlaySFXPitched(1);
        }

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damageToTake, bool shouldKnockback)
    {
        TakeDamage(damageToTake);
        if(shouldKnockback == true)
        {
            CinemachineShake.Instance.ShakeCamera(5f, .1f);
            knockBackCounter = knockBackTime;
            animator.SetTrigger("Hit");
        }
    }
}
