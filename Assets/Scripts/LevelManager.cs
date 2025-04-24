using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private int unlockedLevelCount;


    /*Bu method oyun baþladýðýnda çalýþýr ve daha önce açýlmýþ olan en yüksek seviyeyi PlayerPrefs üzerinden okur.
    Önce tüm level butonlarý devre dýþý býrakýlýr (interactable = false).
    Daha sonra kayýtlý seviyeye kadar olan butonlar aktif hale getirilir.
    Bu sayede oyuncu sadece açtýðý seviyelere týklayabilir kilitli olanlara týklayamaz.*/
    private void Start()
    {
        unlockedLevelCount = PlayerPrefs.GetInt("UnlockedLevelCount", 1);
        
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for(int i = 0; i < unlockedLevelCount; i++)
        {
            buttons[i].interactable = true;
        }
    }


    /*Bu method parametre olarak verilen sahne index’ine göre sahneyi yükler.
    SceneManager.LoadScene(levelIndex) komutuyla Unity'deki sahne geçiþi yapýlmaktadýr.
    Butona týklanýnca hangi sahne çaðrýlýrsa o sahne açýlýr.*/
    public void LoadLevel(int levelIndex)
    {
       SceneManager.LoadScene(levelIndex);
    }
}
