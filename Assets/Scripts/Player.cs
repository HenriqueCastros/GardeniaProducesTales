using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Entity entity;
    private GameObject gardeniaButton;

    [Header("Player Regeneration")]
    public bool regenerateHp = true;

    public int regenerateHPValue = 5;

    public int regenerateHPTime = 2;

    [Header("Game Manager")]
    public GameManager manager;

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
    
    // [Header("Player UI")]
    // Start is called before the first frame update
    void Start()
    {
        gardeniaButton = GameObject.Find("GardeniaButton");
        
        if (manager == null)
        {
            Debug.LogError("instancie o gameManager para essa entidade");
            return;
        }

        entity.maxHealth = manager.CalculateHealth(this);
        entity.maxMana = manager.CalculateMana(this);
        entity.maxStamina = manager.CalculateStamina(this);

        int dmg = (int) manager.CalculateDamage(this, 7);
        int def = (int) manager.CalculateDefence(this, 4);

        entity.currentMana = 1;
        entity.currentHealth = entity.maxHealth;
        entity.currentStamina = entity.maxStamina;

        StartCoroutine(RegenerateHealth());
    }

    void Update()
    {
        gardeniaButton.SetActive(CheckCloseToTag("gardenia", 30) && !GameObject.Find("PauseMenu"));
        
        // if (CheckCloseToTag("gardenia", 30)) {
        //     Debug.Log(gardeniaButton);
        // }
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
}
