using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public AudioSource sound;
    public AudioClip soundClip;

    /*��inde Resources.FindObjectsOfTypeAll<Button>() ile sahnedeki t�m Button component�lerini bulur.
   Her butona bir listener eklenir ve butona t�klan�nca Sound() methodu �a�r�l�r.
   Ve sahnede hangi butona t�klan�rsa t�klans�n, ayn� ses efekti �al�n�r.*/
    public void Awake()
    {
        foreach (Button obje in Resources.FindObjectsOfTypeAll<Button>())
        {
            obje.onClick.AddListener(() => Sound());
        }
    }

    /*Bu method, AudioSource �zerinden belirlenmi� olan AudioClip�i bir kez oynat�r (PlayOneShot).
    Tekrarl� �almadan, sadece t�klama an�nda bir kere ses ��kar.
    Hem k�sa hem pratik bir method, sadece t�k sesini oynat�r ve ge�er.*/
    public void Sound()
    {
        sound.PlayOneShot(soundClip);
    }
}

