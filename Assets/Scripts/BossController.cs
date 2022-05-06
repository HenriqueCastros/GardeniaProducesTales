using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class BossController : MonoBehaviour
{
    [Header("Controller")]
    public Entity entity = new Entity();
    public GameManager manager;

    [Header("Patrol")]
    public Transform[] waypointList;
    public float arrivalDistance = 0.5f;
    public float waitTime = 0;
    
    //Privates
    Transform targetWapoint;
    int currenntWaypoint = 0;
    float lastDistanceToTarget = 0f;
    float currentWaitTime = 0f;

    [Header("Experience Reword")]
    public int rewardExperience = 10;
    public int lootGoldMin = 3;
    public int lootGoldMax = 10;
    
    [Header("Respawn")]
    public GameObject prefab;
    public bool respawn = true;
    public float respawnTime = 5f;

    Rigidbody2D rb2D; 
    Animator animator;
    // Vector2 vector2 = Vector2.zero;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);
                
        entity.currentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;

        currentWaitTime = waitTime;

        if(waypointList.Length > 0 ){
            targetWapoint = waypointList[currenntWaypoint];
            lastDistanceToTarget = Vector2.Distance(transform.position, targetWapoint.position);            
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>

    void Update()
    {
        if(entity.dead){
            return;
        }

        if(entity.currentHealth <= 0){
            
            entity.currentHealth = 0;
            Dead();
            
        }

        if(!entity.inCombat){
            if(waypointList.Length > 0 ){
                Patrulhar();
            }else{
                animator.SetBool("isWalking", false);
            }



        }else{
            if(entity.attackTimer > 0){
                entity.attackTimer -= Time.deltaTime;
            }
            
            if(entity.attackTimer < 0){
                entity.attackTimer = 0;
            }

            if(entity.target != null && entity.inCombat){
                if(!entity.combatCoroutine){
                    StartCoroutine(Attack());
                    
                }
                else{
                    entity.combatCoroutine = false;
                    StopCoroutine(Attack());
                }
            }

        }
    }

    /// <summary>
    /// Sent each frame where another object is within a trigger collider
    /// attached to this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerStay2D(Collider2D colider)
    {
        if(colider.tag == "Player" && !entity.dead){
            entity.inCombat = true;
            entity.target = colider.gameObject;
        }
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D colider)
    {
                
        if(colider.tag == "Player"){
            entity.inCombat = false;
            entity.target = null;
            
        }
    }

    void Patrulhar(){

        if(entity.dead){
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, targetWapoint.position);
        
        if(distanceToTarget <= arrivalDistance || distanceToTarget >= lastDistanceToTarget ){

            animator.SetBool("isWalking", false);


            if(currentWaitTime <= 0){

                currenntWaypoint++;

                if(currenntWaypoint >= waypointList.Length ){
                    currenntWaypoint = 0;
                }

                targetWapoint = waypointList[currenntWaypoint];
            
                lastDistanceToTarget = Vector2.Distance(transform.position, targetWapoint.position);

                currentWaitTime = waitTime;
                
            }
            else{
                currentWaitTime -= Time.deltaTime;
            }
        }else{
            animator.SetBool("isWalking",true);
            lastDistanceToTarget = distanceToTarget;
        }

        Vector2 direction = (targetWapoint.position - transform.position).normalized; 
        animator.SetFloat("input_x", direction.x);
        animator.SetFloat("input_y", direction.y);

        rb2D.MovePosition(rb2D.position + direction * (entity.speed * Time.fixedDeltaTime) );
    }

    IEnumerator Attack(){
        entity.combatCoroutine = true;

        while(true){
            yield return new WaitForSeconds(entity.cooldown);

            if(entity.target != null && !entity.target.GetComponent<Player>().entity.dead){
                animator.SetTrigger("knifeAtack");

                float distance = Vector2.Distance(entity.target.transform.position, transform.position);

                Debug.Log("distance: " + distance);
                Debug.Log("entity.attackDistance: " + entity.attackDistance);

                if(distance <= entity.attackDistance){

                    int dmg = manager.CalculateDamage(entity, entity.damage);
                    int def = manager.CalculateDefence(entity.target.GetComponent<Player>().entity, entity.target.GetComponent<Player>().entity.defense);

                    int resultDmg = dmg - def;

                    if(resultDmg < 0){
                        resultDmg = 0;
                    }

                    Debug.Log("PLAYER APANHOU: "  + resultDmg);
                    entity.target.GetComponent<Player>().entity.currentHealth -= resultDmg;
                }
            }
        }
    }

    IEnumerator Respawn(){

        yield return new WaitForSeconds(respawnTime);

        GameObject newMonster = Instantiate(prefab, transform.position, transform.rotation, null);
        newMonster.name = prefab.name;
        newMonster.GetComponent<BossController>().entity.dead = false;

        Destroy(this.gameObject);
    }
    void Dead(){
        
        entity.dead = true;
        entity.inCombat = false;
        entity.target = null;
        animator.SetBool("isWalking", false);
        //manager.GainExp(rewardExperience);

        Debug.Log("Inimigo morreu" + entity.name);

        animator.SetBool("isDead", true);
        StopAllCoroutines();
        StartCoroutine(Respawn());

    }

}
