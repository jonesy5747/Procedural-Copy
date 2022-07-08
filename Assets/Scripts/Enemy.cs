using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;
    private Transform target;
    private bool move;
    private float curDistance;
    public float minDistance;
    public float maxDistance;
    public SpriteRenderer enemy;
    public LayerMask ignore;
    public Slider healthBar;
    public GameObject ruby, diamond, food;

    private float nextSoundTime;

    public AudioClip death;
    public AudioClip passive;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        gameObject.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;

        nextSoundTime = Random.Range(5, 10);
    }

    // Update is called once per frame
    void Update()
    {
        //Distance between player and enemy.
        curDistance = Vector2.Distance(transform.position, target.position);

        //Raycast direction and enemy flip sprite.
        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 directionToRaycast = (target.position - transform.position);
        enemy.flipX = direction.x > 0;

        if (curDistance > minDistance && curDistance < maxDistance)
        {
            //Raycast to player and move.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToRaycast * 1f, maxDistance, ~ignore);
            Debug.DrawRay(transform.position, directionToRaycast, Color.green); 
            if (hit.transform.gameObject.tag == "Player") {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }

        healthBar.value = GetComponent<Health>().health / 80;
        if (Time.time >= nextSoundTime)
        {
            AudioSource.PlayClipAtPoint(passive, transform.position, 0.4f);
            nextSoundTime += Random.Range(5, 10);
        }

        if (GetComponent<Health>().health <= 0)
        {
            AudioSource.PlayClipAtPoint(death, transform.position);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerControl>().health -= 1;
        }
    }

    //Item loot drop chances.
    public void DropItems()
    {
        for (int i = 0; i < 3; i++) 
        {
            int diamondChance = Random.Range(0, 4);
            int rubyChance = Random.Range(0, 3);
            int foodChance = Random.Range(0, 3);

            if (rubyChance == 1) 
            {
                Instantiate(ruby, transform.position, Quaternion.identity);
            }
            if (diamondChance == 1) 
            {
                Instantiate(diamond, transform.position, Quaternion.identity);
            }
            if (foodChance == 1)
            {
                Instantiate(food, transform.position, Quaternion.identity);
            }
        }
    }
}
