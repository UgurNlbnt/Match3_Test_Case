using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class Tile : MonoBehaviour
{
    private SpriteRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }


    /*Bu method, d��ar�dan g�nderilen sprite�� tile �zerine yerle�tirmek i�in kullan�l�r.
    �ncesinde renderer bile�eni null olup olmad��� kontrol edilir.
    E�er daha �nce atanmad�ysa bile�en al�narak tekrar renderer�a atan�r.
    Sonra renderer.sprite = sprite komutuyla g�rsel de�i�tirilir.*/
    public void SetSprite(Sprite sprite)
    {
        if (renderer == null) 
            renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = sprite;
    }


    /*Bu method tile'�n �zerinde o an hangi sprite varsa onu d�nd�r�r.
    Yine g�venlik amac�yla renderer null kontrol� yap�l�r.
    E�er null ise SpriteRenderer bile�eni al�narak renderer de�i�kenine atan�r.
    Son olarak renderer.sprite de�eri return edilir ve d��ar�dan eri�ilebilir hale gelir.*/
    public Sprite GetSprite()
    {
        if (renderer == null) 
            renderer = GetComponent<SpriteRenderer>();

        return renderer.sprite;
    }
}
