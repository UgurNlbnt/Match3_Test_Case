using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassLevel : MonoBehaviour
{
    private int currentLevel;


    /*Bu method geçilen seviyenin bilgilerini alýr ve kaydeder.
   Önce o anki sahnenin build index’ini currentLevel deðiþkenine atar.
   Eðer bu level daha önce ulaþýlan en yüksek seviyeden büyükse PlayerPrefs ile yeni açýlan seviyeyi kayýt altýna alýr.*/
    public void PassTheLevel()
    {
        currentLevel=SceneManager.GetActiveScene().buildIndex;

        if(currentLevel >= PlayerPrefs.GetInt("UnlockedLevelCount"))
        {
            PlayerPrefs.SetInt("UnlockedLevelCount", currentLevel + 1);
        }
    }
}
