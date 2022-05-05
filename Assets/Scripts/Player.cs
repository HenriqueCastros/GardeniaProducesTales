using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public Entity entity;

    [Header("Player UI")]  
    public Slider health;

    [Header("Player Regeneration")]
    public bool regenerateHp = true;
    public int regenerateHPValue = 1;
    public int regenerateHPTime = 1000;

    [Header("Game Manager")]

    public GameManager manager;

    // [Header("Player UI")]
    // Start is called before the first frame update
    void Start()
    {
        if(manager == null){
            Debug.LogError("instancie o gameManager para essa entidade");
            return;
        }

        entity.maxHealth = manager.CalculateHealth(this);
        entity.maxMana = manager.CalculateMana(this);
        entity.maxStamina = manager.CalculateStamina(this);

        int dmg = (int)manager.CalculateDamage(this, 7);
        int def = (int)manager.CalculateDefence(this, 4);

        entity.currentHealth = 2;

        entity.currentStamina = entity.maxStamina;

        health.value = (float)entity.currentHealth;   

        StartCoroutine(RegenerateHealth());
        
    }

    private void Update(){
        health.value = (float)entity.currentHealth/(float)entity.maxHealth;
        if(Input.GetKeyDown(KeyCode.Space)){
            entity.currentHealth -= 1;

        }
    }

    IEnumerator RegenerateHealth(){
        while(true){
            if(regenerateHp){
                if(entity.currentHealth < entity.maxHealth){
                    
                    Debug.LogFormat("Recuperando hp");
                    entity.currentHealth += regenerateHPValue;
                    yield return new WaitForSeconds(regenerateHPTime);
                    
                }else{
                    yield return null;
                }
            }
            else{
                yield return null;
            }
        }
    }
}
