using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    private int unlockedLevelCount;


    /*Bu method oyun ba�lad���nda �al���r ve daha �nce a��lm�� olan en y�ksek seviyeyi PlayerPrefs �zerinden okur.
    �nce t�m level butonlar� devre d��� b�rak�l�r (interactable = false).
    Daha sonra kay�tl� seviyeye kadar olan butonlar aktif hale getirilir.
    Bu sayede oyuncu sadece a�t��� seviyelere t�klayabilir kilitli olanlara t�klayamaz.*/
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


    /*Bu method parametre olarak verilen sahne index�ine g�re sahneyi y�kler.
    SceneManager.LoadScene(levelIndex) komutuyla Unity'deki sahne ge�i�i yap�lmaktad�r.
    Butona t�klan�nca hangi sahne �a�r�l�rsa o sahne a��l�r.*/
    public void LoadLevel(int levelIndex)
    {
       SceneManager.LoadScene(levelIndex);
    }
}
