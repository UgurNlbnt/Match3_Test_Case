using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassLevel : MonoBehaviour
{
    private int currentLevel;


    /*Bu method ge�ilen seviyenin bilgilerini al�r ve kaydeder.
   �nce o anki sahnenin build index�ini currentLevel de�i�kenine atar.
   E�er bu level daha �nce ula��lan en y�ksek seviyeden b�y�kse PlayerPrefs ile yeni a��lan seviyeyi kay�t alt�na al�r.*/
    public void PassTheLevel()
    {
        currentLevel=SceneManager.GetActiveScene().buildIndex;

        if(currentLevel >= PlayerPrefs.GetInt("UnlockedLevelCount"))
        {
            PlayerPrefs.SetInt("UnlockedLevelCount", currentLevel + 1);
        }
    }
}
