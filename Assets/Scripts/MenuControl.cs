using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public GameObject menuPanel;


    /*Bu method menü panelini görünür hale getirmek için kullanýlýr.
   SetActive(true) komutu ile menuPanel nesnesi aktif edilir.
   Böylece kullanýcýya menü ekraný açýlmýþ olarak gösterilir.*/
    public void OpenMenu()
    {
        menuPanel.SetActive(true);
    }


    /*Bu method menü panelini gizlemek için kullanýlýr.
    SetActive(false) komutu sayesinde menuPanel nesnesi devre dýþý býrakýlýr.
    Bu da menünün kapatýldýðýný gösterir.*/
    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }
}
