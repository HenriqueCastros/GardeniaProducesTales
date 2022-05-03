using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public Int32 CalculateHealth(Player player){
       
        Int32 result = (player.entity.resistence * 10 ) + (player.entity.level * 4 ) + 10;
        Debug.LogFormat("reuslt od calculate health: {0}", result);

        return result;
    }

    public Int32 CalculateStamina(Player player){
       
        Int32 result = (player.entity.resistence  + player.entity.willPower ) + (player.entity.level * 4 ) + 10;
        Debug.LogFormat("reuslt od calculate stamina: {0}", result);

        return result;
    }


    public Int32 CalculateMana(Player player){
       
        Int32 result = (player.entity.resistence * 8 ) + (player.entity.level * 4 ) + 10;
        Debug.LogFormat("reuslt od calculate mana: {0}", result);

        return result;
    }


    public Int32 CalculateDamage(Player player, int armorDamage){

        System.Random rand = new System.Random();
        Int32 result = (player.entity.damage * 3) + (armorDamage * 2) + rand.Next(1, 20); 
        Debug.LogFormat("reuslt od calculate damage: {0}", result);

        return result;
    }


    public Int32 CalculateDefence(Player player, int armorDefence){
       
        Int32 result = (player.entity.defense * 3 ) + (armorDefence * 2);
        Debug.LogFormat("reuslt od calculate mana: {0}", result);

        return result;
    }

    
}
