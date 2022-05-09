using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    void SetPlayerMoviment(bool active)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        for (int i = 0; i < players.Length; ++i)
        {
            players[i].GetComponent<PlayerControler>().allowMoviment = active;
        }
    }
    
    public void PauseGame()
    {
        SetPlayerMoviment(false);
        Time.timeScale--;
    }
    
    public void ResumeGame()
    {
        SetPlayerMoviment(true);
        Time.timeScale++;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
