using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    public LayerMask tileLayerMask; // sadece tile'lara dokununca swipe

    public delegate void OnSwipe(Vector2 direction);
    public static event OnSwipe OnSwipeDetected;


    /*Bu method her frame’de sürekli çalýþýr ve kullanýcýnýn dokunma (mouse týklama ya da dokunmatik) hareketlerini kontrol eder.
   dokunulan yer bir tile ise swipe baþlatýlýr ve baþlangýç pozisyonu kaydedilir.
   Dokunma býrakýldýðýnda (GetMouseButtonUp) bitiþ pozisyonu alýnýr ve baþlangýçla farký hesaplanarak kaydýrma yönü belirlenir.
   Eðer bu hareket yeterince büyükse (magnitude > 50f), swipe yönü normalize edilir ve OnSwipeDetected event’i tetiklenir.
   Bu sayede sadece belirli mesafede bir kaydýrma olduðunda sistem tepki verir minik týklamalara tepki vermez.*/
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverTile())
            {
                startTouchPosition = Input.mousePosition;
                isSwiping = true;
            }
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            endTouchPosition = Input.mousePosition;
            Vector2 swipe = endTouchPosition - startTouchPosition;

            if (swipe.magnitude > 50f)
            {
                Vector2 swipeDirection = swipe.normalized;
                OnSwipeDetected?.Invoke(swipeDirection);
            }

            isSwiping = false;
        }
    }


    /*Bu method, kullanýcýnýn dokunduðu (veya týkladýðý) noktanýn bir tile objesi olup olmadýðýný kontrol eder.
   Ekrandaki mouse pozisyonu dünya koordinatýna çevrilir (ScreenToWorldPoint) ve bu pozisyondan bir Raycast gönderilir.
   Eðer bu ýþýn tileLayerMask ile belirlenmiþ katmana sahip bir collider’a çarparsa true döner, yani geçerli bir tile’dýr.
   Bu sayede kullanýcý sadece tile’a týkladýðýnda swipe iþlemi baþlatabilir, ekranýn boþ yerine týklarsa iþlem görmez.*/
    private bool IsPointerOverTile()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, tileLayerMask);

        return hit.collider != null;
    }
}
