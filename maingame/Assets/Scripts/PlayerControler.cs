using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (PlayerControler))]
public class PlayerControler : EntityController
{
    public Animator playerAnimator;
    
    float input_x = 0;

    float input_y = 0;

    bool isWalking = false;
    
    private GameObject gardeniaButton;
    private GameObject callButton;

    [Header("Player UI")]
    public Slider health;

    [Header("Player Regeneration")]
    public bool regenerateHp = true;
    public int regenerateHPValue = 5;
    public int regenerateHPTime = 2;
    
    [Header("Game Manager")]
    public GameManager manager;

    // public bool allowMoviment = true;
    Rigidbody2D rb2D;
    GameObject attackObj;

    Vector2 moviment = Vector2.zero;

    bool CheckCloseToTag(string tag, float minimumDistance)
    {
        GameObject[] goWithTag = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < goWithTag.Length; ++i)
        {
            if (
                Vector3
                    .Distance(transform.position,
                    goWithTag[i].transform.position) <=
                minimumDistance
            ) return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        isWalking = false;
        rb2D = GetComponent<Rigidbody2D>();
        attackObj = gameObject.transform.Find("AttackArea").gameObject;
        gardeniaButton = GameObject.Find("GardeniaButton");
        callButton = GameObject.Find("CallButton");
        
        if (manager == null)
        {
            Debug.LogError("instancie o gameManager para essa entidade");
            return;
        }

        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);

        int dmg = (int) manager.CalculateDamage(entity, 7);
        int def = (int) manager.CalculateDefence(entity, 4);

        entity.currentHealth = 2;

        entity.currentStamina = entity.maxStamina;

        health.value = (float) entity.currentHealth;

        StartCoroutine(RegenerateHealth());
    }
    
    // Update is called once per frame
    void Update()
    {
        gardeniaButton
            .SetActive(CheckCloseToTag("gardenia", 30) &&
            !GameObject.Find("PauseMenu"));

        if (gardeniaButton.activeSelf)
            callButton.SetActive(false);
        else
            callButton.SetActive(true);

        health.value = (float) entity.currentHealth / (float) entity.maxHealth;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            entity.currentHealth -= 1;
        }

        if (!allowMoviment) return;

        input_x = Input.GetAxisRaw("Horizontal");
        input_y = Input.GetAxisRaw("Vertical");
        isWalking = (input_x != 0 || input_y != 0);
        moviment = new Vector2(input_x, input_y);

        if (isWalking)
        {
            playerAnimator.SetFloat("input_x", input_x);
            playerAnimator.SetFloat("input_y", input_y);
        }
        
        if (input_x > 0) {
            attackObj.transform.localPosition  = new Vector3(1, 0, 0);
        } else if  (input_x < 0) {
            attackObj.transform.localPosition  = new Vector3(-1, 0, 0);
        }
        
        playerAnimator.SetBool("isWalking", isWalking);

        if (Input.GetButtonDown("Fire1")) playerAnimator.SetTrigger("attack");
    }
    
    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            if (regenerateHp)
            {
                if (entity.currentHealth < entity.maxHealth)
                {
                    Debug.LogFormat("Recuperando hp");
                    entity.currentHealth += regenerateHPValue;
                    yield return new WaitForSeconds(regenerateHPTime);
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private void FixedUpdate()
    {
        rb2D
            .MovePosition(rb2D.position +
            moviment * entity.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("buffvida"))
        {
            Destroy(collision.gameObject);
            entity.maxHealth += 20;
            entity.currentHealth += 20;
            print("MaxLife was buffed.");
        }
        else if (collision.gameObject.CompareTag("buffdefesa"))
        {
            Destroy(collision.gameObject);
            entity.defense += 1;
            print("Defense was buffed.");
        }
        else if (collision.gameObject.CompareTag("buffataque"))
        {
            Destroy(collision.gameObject);
            entity.damage += 1;
            print("Attack was buffed.");
        }
    }
}
