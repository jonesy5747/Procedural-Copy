using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public float speed, jumpForce;
    private float moveInput;
    public float health;

    private Rigidbody2D rb;
    private bool facingRight = true;

    public bool isGrounded, midStairHit, groundStairHit;
    public Transform groundCheck, groundStairCheck, midStairCheck;
    public float checkRadius;
    public float stairCheckRadius;

    public GameObject player;
    public GameObject coal, ruby, diamond, rock, wood, apple;
    public GameObject tuna;
    public GameObject[] secondaryItems;

    private bool ePressed;

    public LayerMask whatIsGrounded;
    public LayerMask ignore;

    public float maxFuel;
    private float curFuel;

    public SpriteRenderer flame;

    public SpriteRenderer[] tools;
    public List<GameObject> secondaryTools;

    public Slider jetpackFuel;
    public Slider healthBar;

    float timer = 1f;

    private int selectedTool;
    private int selectedSecondary;

    private bool hoveringOverUI;
    private RaycastHit2D hit;

    private GameObject deathScreen;
    private GameObject[] mainUI;

    public Audio manager;

    private float nextSoundTimeHeartbeat;
    private float nextSoundTimeHitting;
    private float nextSoundTimeWalking;
    private float minPitchRange = 1.2f;
    private float maxPitchRange = 0.8f;

    private bool onGrass;
    private bool onRock;

    private bool deathSound = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curFuel = maxFuel;
        jetpackFuel.gameObject.SetActive(false);

        for (int i = 1; i < tools.Length; i++)
        {
            tools[i].enabled = false;
        }
        for (int i = 0; i < secondaryItems.Length; i++)
        {
            secondaryItems[i].SetActive(false);
        }

        mainUI = GameObject.FindGameObjectsWithTag("mainUI");
        deathScreen = GameObject.FindWithTag("deathScreen");
        deathScreen.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        // Raycast for the tools
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Ray worldPos = Camera.main.ScreenPointToRay(mousePos);
        Vector3 playerPos = new Vector3(player.transform.position.x - 0.5f, player.transform.position.y + 0.1f, player.transform.position.z);
        hit = Physics2D.CircleCast(player.transform.position, 1f, (worldPos.origin - player.transform.position) * -1f, -8f, ~ignore);

        if (EventSystem.current.IsPointerOverGameObject())
        {
            hoveringOverUI = true;
        }
        else
        {
            hoveringOverUI = false;
        }

        GameObject.FindWithTag("crafting").GetComponent<Crafting>().hit = hit;

        jetpackFuel.value = curFuel / maxFuel;
        healthBar.value = health / 100;

        if (!hoveringOverUI) {
            if (selectedTool == 1)
            {
                DestroyEnemy(hit);
            }

            if (selectedTool == 0)
            {
                DestroyingTiles(hit);
            }

            if (selectedTool == 2)
            {
                DestroyTree(hit);
            }

            if (selectedTool == 3)
            {
                DestroyFish(hit);
            }
        }

        //Tool selection
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && !Input.GetMouseButton(1))
        {
            selectedTool += 1;
            if (selectedTool > tools.Length - 1)
            {
                selectedTool = 0;
            }
            Equip(selectedTool);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && !Input.GetMouseButton(1))
        {
            selectedTool -= 1;
            if (selectedTool < 0)
            {
                selectedTool = tools.Length - 1;
            }
            Equip(selectedTool);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && Input.GetMouseButton(1))
        {
            selectedSecondary += 1;
            if (selectedSecondary > secondaryTools.Count - 1)
            {
                selectedSecondary = 0;
            }
            EquipSecondary(selectedSecondary);
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && Input.GetMouseButton(1))
        {
            selectedSecondary -= 1;
            if (selectedSecondary < 0)
            {
                selectedSecondary = secondaryTools.Count - 1;
            }
            EquipSecondary(selectedSecondary);
        }

        //Death
        if (health <= 0)
        {
            if (deathSound) {
                manager.sfxSource.pitch = 1;
                manager.sfxSource.volume = 0.6f;
                manager.Play(manager.playerDeath1, manager.sfxSource);
                deathSound = false;
            }
            deathScreen.SetActive(true);
            foreach (GameObject ui in mainUI)
            {
                ui.SetActive(false);
            }
            Time.timeScale = 0;
        }

        //HealthSound
        if (health <= 80 && health > 60)
        {
            if (Time.time >= nextSoundTimeHeartbeat)
            {
                manager.Play(manager.health1, manager.sfxSource);
                nextSoundTimeHeartbeat += manager.health1.length;
            }
        }

        if (health <= 60 && health > 30)
        {
            if (Time.time >= nextSoundTimeHeartbeat)
            {
                manager.Play(manager.health2, manager.sfxSource);
                nextSoundTimeHeartbeat += manager.health2.length;
            }
        }

        if (health <= 30)
        {
            if (Time.time >= nextSoundTimeHeartbeat)
            {
                manager.Play(manager.health3, manager.sfxSource);
                nextSoundTimeHeartbeat += manager.health3.length;
            }
        }

        //Jump
        if (isGrounded)
        {
            if (Input.GetKeyDown("space"))
            {
                rb.AddForce(rb.transform.up * jumpForce * 50f * Time.deltaTime);
            }
        }

        JetpackActivation();

        if (Input.GetKey(KeyCode.E))
        {
            ePressed = true;
        }
        else
            ePressed = false;

        SecondaryItemCheck();

        SecondaryEatItem();
    }

    //Equips tool based off mouse scroll value
    void Equip(int selectedTool)
    {
        for (int i = 0; i < tools.Length; i++)
        {
            if (i == selectedTool)
            {
                selectedTool = i;
                tools[i].enabled = true;
            }
            else
                tools[i].enabled = false;
        }
    }

    //Equips secondary tool based off mouse scroll value + mouse click
    void EquipSecondary(int selectedSecondary)
    {
        for (int i = 0; i < secondaryTools.Count; i++)
        {
            if (i == selectedSecondary)
            {
                selectedSecondary = i;
                secondaryTools[i].SetActive(true);
            }
            else
                secondaryTools[i].SetActive(false);
        }
    }

    //Allows jetpack movement when player has fuel. Outputs to slider.
    void JetpackActivation()
    {
        if (!isGrounded && curFuel >= 0f && Input.GetKey("space"))
        {
            curFuel -= Time.deltaTime;
            rb.AddForce(rb.transform.up * jumpForce * 2f * Time.deltaTime);
            flame.enabled = true;
            jetpackFuel.gameObject.SetActive(true);
            timer = 1f;
        }

        else
        {
            if (timer <= 0)
            {
                flame.enabled = false;
                jetpackFuel.gameObject.SetActive(false);
            }
            timer -= Time.deltaTime;
            if (curFuel < maxFuel)
            {
                curFuel += Time.deltaTime / 2;
            }
        }
    }

   //Destroys tree and spits out wood and apples. Called in update.
    void DestroyTree(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.transform.tag == "tree")
        {
            if (Input.GetMouseButton(0) && hit.distance < 1.5f)
            {
                if (Time.time >= nextSoundTimeHitting)
                {
                    manager.PlayRandom(manager.hittingSound, manager.sfxSource);
                    nextSoundTimeHitting += 0.3f;
                }
                hit.transform.gameObject.GetComponent<Health>().health -= 1;
                if (hit.transform.gameObject.GetComponent<Health>().health % 60 == 0)
                {
                    GenerateCollectableItem(wood);
                }
                int dropApple = Random.Range(0, 60);
                if (dropApple == 1)
                {
                    GenerateCollectableItem(apple);
                }
                if (hit.transform.gameObject.GetComponent<Health>().health <= 0)
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    //Destroys fish and spits out raw tuna.
    void DestroyFish(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            if (Input.GetMouseButton(0) && hit.distance < 8f)
            {
                if (hit.transform.tag == "tuna")
                {
                    GenerateCollectableItem(tuna);
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    //Deals damage to enemy. 
    void DestroyEnemy(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.transform.tag == "enemy")
        {
            if (Input.GetMouseButtonDown(0) && hit.distance < 5f)
            {
                hit.transform.gameObject.GetComponent<Health>().health -= 10;
                float randomVol = Random.Range(manager.minPitchRange, manager.maxPitchRange);
                AudioSource.PlayClipAtPoint(manager.enemyHurt, hit.transform.position, randomVol);
                if (hit.transform.gameObject.GetComponent<Health>().health <= 0)
                {
                    hit.transform.gameObject.GetComponent<Enemy>().DropItems();
                    DeathSound();
                    hit.transform.gameObject.SetActive(false);
                }
            }
        }
    }

    void DeathSound()
    {
        int ran = Random.Range(0, 2);
        if (ran == 0)
        {
            AudioSource.PlayClipAtPoint(manager.enemyDeath1, hit.transform.position);
        }
        if (ran == 1)
        {
            AudioSource.PlayClipAtPoint(manager.enemyDeath2, hit.transform.position);
        }
    }

    void FixedUpdate()
    {
        //Ground and step checks.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGrounded);
        midStairHit = Physics2D.OverlapCircle(midStairCheck.position, stairCheckRadius, whatIsGrounded);
        groundStairHit = Physics2D.OverlapCircle(groundStairCheck.position, stairCheckRadius, whatIsGrounded);

        //Movement.
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed * Time.deltaTime, rb.velocity.y);


        if (moveInput != 0 && isGrounded)
        {
            if (Time.time >= nextSoundTimeWalking)
            {
                if (onGrass)
                {
                    manager.PlayRandom(manager.walkGrassSound, manager.sfxSource);
                    nextSoundTimeWalking += manager.walkGrassSound.length + 0.1f * Time.deltaTime;
                }

                if (onRock)
                {
                    manager.PlayRandom(manager.walkRockSound, manager.sfxSource);
                    nextSoundTimeWalking += manager.walkRockSound.length + 0.1f * Time.deltaTime;
                }
            }
        }

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        //Steps up stairs.
        if (groundStairHit == true && midStairHit == false && !Input.GetKey("space"))
        {
            if (moveInput > 0)
            {
                rb.position = Vector3.Lerp(rb.position, new Vector3(rb.position.x + 15f * Time.deltaTime, rb.position.y + 20f * Time.deltaTime, 0), speed * moveInput * Time.deltaTime);
            }
            if (moveInput < 0)
            {
                rb.position = Vector3.Lerp(rb.position, new Vector3(rb.position.x + -15f * Time.deltaTime, rb.position.y + 20f * Time.deltaTime, 0), -speed * moveInput * Time.deltaTime);
            }
        }
    }

    //Character faces the right way.
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    //Allows for terrain destruction (rocks, gems etc.)
    void Destroy(RaycastHit2D hit, string objHit, GameObject objectToSpawn)
    {
        if (hit.collider != null && hit.transform.tag == objHit)
        {
            if (Input.GetMouseButton(0) && hit.distance < 1.5f)
            {
                if (Time.time >= nextSoundTimeHitting) {
                    manager.PlayRandom(manager.hittingSound, manager.sfxSource);
                    nextSoundTimeHitting += 0.3f;
                }
                GameObject.FindGameObjectWithTag(objHit).GetComponent<TilesDestructable>().hit = hit;
                if (objectToSpawn == rock)
                {
                    int rockDropChance = Random.Range(0, 10);
                    if (rockDropChance == 1)
                    {
                        if (objectToSpawn != null)
                        {
                            GenerateCollectableItem(objectToSpawn);
                        }
                    }
                }
                else
                {
                    if (objectToSpawn != null)
                    {
                        GenerateCollectableItem(objectToSpawn);
                    }
                }
            }
        }
    }

    //Needed fixed.
    //Passes through different objects to destroy.
    void DestroyingTiles(RaycastHit2D hit)
    {
        Destroy(hit, "grass", null);
        Destroy(hit, "dirt", null);
        Destroy(hit, "rock", rock);
        Destroy(hit, "coal", coal);
        Destroy(hit, "ruby", ruby);
        Destroy(hit, "diamond", diamond);
    }

    void GenerateCollectableItem(GameObject obj)
    {
        obj = Instantiate(obj, new Vector3(hit.point.x, hit.point.y, 0), Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0));
    }

    //Quick eat item from secondary item by using "C".
    void SecondaryEatItem()
    {
        if (secondaryTools[selectedSecondary].tag == "apple")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple -= 1;
                if (health < 100)
                {
                    health += 10;

                }
                else
                {
                    GetComponent<Hunger>().hungerValue += 10;
                }
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple == 0) 
                {
                    Destroy(GameObject.FindWithTag("appleButton"));
                }
            }
        }
        if (secondaryTools[selectedSecondary].tag == "tunaCooked")
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna -= 1;
                if (health < 100)
                {
                    health += 35;

                }
                else
                {
                    GetComponent<Hunger>().hungerValue += 35;
                }
                if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna == 0) 
                {
                    Destroy(GameObject.FindWithTag("cookedTunaButton"));
                }
            }
        }
    }

    //Check to see if item in inventory.
    void SecondaryItemCheck()
    {
        if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple == 0)
        {
            SecondaryItemRemoval("apple");
        }
        if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna == 0)
        {
            SecondaryItemRemoval("tunaCooked");
        }
    }
    
    //Remove item if not in inventory.
    void SecondaryItemRemoval(string itemToRemove)
    {
        for (int i = 0; i < secondaryTools.Count; i++)
        {
            if (secondaryTools[i].tag == itemToRemove)
            {
                if (secondaryTools[selectedSecondary].tag == itemToRemove)
                {
                    secondaryTools[selectedSecondary].SetActive(false);
                    secondaryTools[0].SetActive(true);
                }
                secondaryTools.Remove(secondaryTools[i].gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "grass") {
            onGrass = true;
        }

        if (other.gameObject.tag == "rock")
        {
            onRock = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "grass")
        {
            onGrass = false;
        }

        if (other.gameObject.tag == "rock")
        {
            onRock = false;
        }
    }

    //BAD - needed separate function.
    //Checking collision and adding to inventory.
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "coalCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupGemSound, manager.sfxSource);
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().coal += 1;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.tag == "rubyCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupGemSound, manager.sfxSource);
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().ruby += 1;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.tag == "diamondCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupGemSound, manager.sfxSource);
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().diamond += 1;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.tag == "rockCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupSound, manager.sfxSource);
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().rock += 1;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.tag == "woodCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupWoodSound, manager.sfxSource);
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().wood += 1;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.tag == "tunaCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupSound, manager.sfxSource);
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().tuna += 1;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.tag == "appleCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupAppleSound, manager.sfxSource);
            if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple < 1)
            {
                secondaryTools.Add(secondaryItems[0].transform.gameObject);
            }
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().apple += 1;
            Destroy(other.transform.gameObject);
        }
        if (other.gameObject.tag == "cookedTunaCollectable" && ePressed)
        {
            manager.PlayRandom(manager.pickupSound, manager.sfxSource);
            if (GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna < 1)
            {
                secondaryTools.Add(secondaryItems[1].transform.gameObject);
            }
            GameObject.FindWithTag("crafting").GetComponent<Crafting>().cookedTuna += 1;
            Destroy(other.transform.gameObject);
        }

        if (other.gameObject.tag == "water")
        {
            if (!Input.GetKey(KeyCode.S))
            {
                rb.velocity = new Vector3(0, 5, 0);
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector3(0, 5, 0);
            }
        }
    }
}
