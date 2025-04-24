using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public GameObject menuPanel;


    /*Bu method men� panelini g�r�n�r hale getirmek i�in kullan�l�r.
   SetActive(true) komutu ile menuPanel nesnesi aktif edilir.
   B�ylece kullan�c�ya men� ekran� a��lm�� olarak g�sterilir.*/
    public void OpenMenu()
    {
        menuPanel.SetActive(true);
    }


    /*Bu method men� panelini gizlemek i�in kullan�l�r.
    SetActive(false) komutu sayesinde menuPanel nesnesi devre d��� b�rak�l�r.
    Bu da men�n�n kapat�ld���n� g�sterir.*/
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
}
