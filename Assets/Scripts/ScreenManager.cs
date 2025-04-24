using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public Transform topAnchor;
    public Transform bottomAnchor;
    public Transform leftAnchor;
    public Transform rightAnchor;



    /*Bu method oyun baþladýðýnda ana kamerayý (Camera.main) referans alarak ekranýn üst, alt, sol ve sað kenarlarýný dünya koordinatlarýnda hesaplar.
    ViewportToWorldPoint() fonksiyonu, kamera ekranýndaki 0-1 arasý viewport deðerlerini gerçek dünya koordinatlarýna çevirir.
    (0.5f, 1f) deðeri ekranýn tam ortasýnda en üst noktayý verir, (0f, 0.5f) ise ekranýn sol orta noktasýdýr.
    Hesaplanan bu noktalar sýrasýyla topAnchor, bottomAnchor, leftAnchor ve rightAnchor objelerinin pozisyonlarýna atanýr.
    Bu sayede ekranda sabitlenmiþ UI ya da nesne yerleþimi yapmak kolaylaþýr.
    Bu özelliði farklý çözünürlüklü cihazlarda ekran sýnýrlarýný dinamik olarak belirlemek için çok iþe yarayan bir yöntem olarak kullanýlýr.*/
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
