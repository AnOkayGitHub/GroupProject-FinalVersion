using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject destroyPS;
    [SerializeField] private GameObject propDestroyPS;
    [SerializeField] private Transform destroySpawn;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Animator animator;
    private float lifetime;
    private float currentTime = 0;
    private bool destroy = false;
    private bool hasDestroyed = false;
    private float damage;
    private bool isFriendly = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        animator.Play("Decay", -1, 0f);
    }

    private void Update()
    {
        if(currentTime < lifetime)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            if(!destroy)
            {
                DestroySelf();
            }
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "ProjDestroy")
        {
            if (!hasDestroyed)
            {
                
                DestroySelf();
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!hasDestroyed)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Prop"))
            {
                DestroySelf();
                Destroy(collision.gameObject, 1f);
                collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                GameObject ps = (GameObject)Instantiate(propDestroyPS);
                ps.transform.position = collision.transform.position;
                ParticleSystem.MainModule settings = ps.GetComponent<ParticleSystem>().main;
                settings.startColor = new ParticleSystem.MinMaxGradient(collision.gameObject.GetComponent<SpriteRenderer>().color);
                collision.gameObject.GetComponent<Prop>().SpawnLoot();
                collision.gameObject.GetComponent<AudioSource>().pitch = Random.Range(1f, 1.2f);
                collision.gameObject.GetComponent<AudioSource>().Play();
                Destroy(ps, 1f);
            }
            
            if (isFriendly)
            {
                if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    DestroySelf();
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                    enemy.Hurt(damage);
                    PlayerController p = World.player.gameObject.GetComponent<PlayerController>();

                    if(p.GetItems()[4])
                    {
                        p.SetCurrentHealth(p.GetCurrentHealth() + 1f);
                        p.UpdateHealth();
                    }
                }

                if (collision.gameObject.layer == LayerMask.NameToLayer("NPC"))
                {
                    DestroySelf();
                }
            } 
            else 
            {
                if (collision.gameObject.tag == "Player")
                {
                    DestroySelf();
                    PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                    player.Hurt(damage);
                }
            }
        }
        
    }
    
    private void DestroySelf()
    {
        Destroy(gameObject, 0.5f);
        if(!hasDestroyed)
        {
            hasDestroyed = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            audioSource.pitch = Random.Range(0.8f, 1.4f);
            audioSource.Play();
            GameObject ps = (GameObject)Instantiate(destroyPS);
            ps.transform.position = destroySpawn.position;
            Destroy(ps, 1f);
        }
    }

    public void SetLifetime(float l)
    {
        lifetime = l;
    }

    public void SetFriendly(bool status)
    {
        isFriendly = status;
    }
   
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}