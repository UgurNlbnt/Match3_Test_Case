using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Transform topAnchor;
    public Transform bottomAnchor;
    public Transform leftAnchor;
    public Transform rightAnchor;



    /*Bu method oyun ba�lad���nda ana kameray� (Camera.main) referans alarak ekran�n �st, alt, sol ve sa� kenarlar�n� d�nya koordinatlar�nda hesaplar.
    ViewportToWorldPoint() fonksiyonu, kamera ekran�ndaki 0-1 aras� viewport de�erlerini ger�ek d�nya koordinatlar�na �evirir.
    (0.5f, 1f) de�eri ekran�n tam ortas�nda en �st noktay� verir, (0f, 0.5f) ise ekran�n sol orta noktas�d�r.
    Hesaplanan bu noktalar s�ras�yla topAnchor, bottomAnchor, leftAnchor ve rightAnchor objelerinin pozisyonlar�na atan�r.
    Bu sayede ekranda sabitlenmi� UI ya da nesne yerle�imi yapmak kolayla��r.
    Bu �zelli�i farkl� ��z�n�rl�kl� cihazlarda ekran s�n�rlar�n� dinamik olarak belirlemek i�in �ok i�e yarayan bir y�ntem olarak kullan�l�r.*/
    void Start()
    {
        Camera cam = Camera.main;

        Vector3 top = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane));
        Vector3 bottom = cam.ViewportToWorldPoint(new Vector3(0.5f, 0f, cam.nearClipPlane));
        Vector3 left = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, cam.nearClipPlane));
        Vector3 right = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, cam.nearClipPlane));

        topAnchor.position = new Vector3(top.x, top.y, 0);
        bottomAnchor.position = new Vector3(bottom.x, bottom.y, 0);
        leftAnchor.position = new Vector3(left.x, left.y, 0);
        rightAnchor.position = new Vector3(right.x, right.y, 0);
    }
}
