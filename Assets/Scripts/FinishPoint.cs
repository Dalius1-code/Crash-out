using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPoint : MonoBehaviour
{

    [SerializeField] GameObject endMenu;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnlockNewLevel();
            Debug.Log( PlayerPrefs.GetInt("unlockedlevel"));
            
            endMenu.SetActive(true);
        }
    }

    void UnlockNewLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int unlockedLevel = PlayerPrefs.GetInt("unlockedlevel", 1);

        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("unlockedlevel", currentLevel + 1);
            PlayerPrefs.Save();
            Debug.Log("Lygis atrakintas! Naujas unlockedlevel: " + (currentLevel + 1));
        }
    }
}
