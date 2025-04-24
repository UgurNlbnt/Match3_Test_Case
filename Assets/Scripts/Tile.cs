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


    /*Bu method, dýþarýdan gönderilen sprite’ý tile üzerine yerleþtirmek için kullanýlýr.
    Öncesinde renderer bileþeni null olup olmadýðý kontrol edilir.
    Eðer daha önce atanmadýysa bileþen alýnarak tekrar renderer’a atanýr.
    Sonra renderer.sprite = sprite komutuyla görsel deðiþtirilir.*/
    public void SetSprite(Sprite sprite)
    {
        if (renderer == null) 
            renderer = GetComponent<SpriteRenderer>();

        renderer.sprite = sprite;
    }


    /*Bu method tile'ýn üzerinde o an hangi sprite varsa onu döndürür.
    Yine güvenlik amacýyla renderer null kontrolü yapýlýr.
    Eðer null ise SpriteRenderer bileþeni alýnarak renderer deðiþkenine atanýr.
    Son olarak renderer.sprite deðeri return edilir ve dýþarýdan eriþilebilir hale gelir.*/
    public Sprite GetSprite()
    {
        if (renderer == null) 
            renderer = GetComponent<SpriteRenderer>();

        return renderer.sprite;
    }
}
