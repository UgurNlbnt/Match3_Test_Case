using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip soundClip;

    /*Ýçinde Resources.FindObjectsOfTypeAll<Button>() ile sahnedeki tüm Button component’lerini bulur.
   Her butona bir listener eklenir ve butona týklanýnca Sound() methodu çaðrýlýr.
   Ve sahnede hangi butona týklanýrsa týklansýn, ayný ses efekti çalýnýr.*/
    public void Awake()
    {
        foreach (Button obje in Resources.FindObjectsOfTypeAll<Button>())
        {
            obje.onClick.AddListener(() => Sound());
        }
    }

    /*Bu method, AudioSource üzerinden belirlenmiþ olan AudioClip’i bir kez oynatýr (PlayOneShot).
    Tekrarlý çalmadan, sadece týklama anýnda bir kere ses çýkar.
    Hem kýsa hem pratik bir method, sadece týk sesini oynatýr ve geçer.*/
    public void Sound()
    {
        sound.PlayOneShot(soundClip);
    }
}

