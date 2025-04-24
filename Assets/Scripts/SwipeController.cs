using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;

    public LayerMask tileLayerMask; // sadece tile'lara dokununca swipe

    public delegate void OnSwipe(Vector2 direction);
    public static event OnSwipe OnSwipeDetected;


    /*Bu method her frame�de s�rekli �al���r ve kullan�c�n�n dokunma (mouse t�klama ya da dokunmatik) hareketlerini kontrol eder.
   dokunulan yer bir tile ise swipe ba�lat�l�r ve ba�lang�� pozisyonu kaydedilir.
   Dokunma b�rak�ld���nda (GetMouseButtonUp) biti� pozisyonu al�n�r ve ba�lang��la fark� hesaplanarak kayd�rma y�n� belirlenir.
   E�er bu hareket yeterince b�y�kse (magnitude > 50f), swipe y�n� normalize edilir ve OnSwipeDetected event�i tetiklenir.
   Bu sayede sadece belirli mesafede bir kayd�rma oldu�unda sistem tepki verir minik t�klamalara tepki vermez.*/
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


    /*Bu method, kullan�c�n�n dokundu�u (veya t�klad���) noktan�n bir tile objesi olup olmad���n� kontrol eder.
   Ekrandaki mouse pozisyonu d�nya koordinat�na �evrilir (ScreenToWorldPoint) ve bu pozisyondan bir Raycast g�nderilir.
   E�er bu ���n tileLayerMask ile belirlenmi� katmana sahip bir collider�a �arparsa true d�ner, yani ge�erli bir tile�d�r.
   Bu sayede kullan�c� sadece tile�a t�klad���nda swipe i�lemi ba�latabilir, ekran�n bo� yerine t�klarsa i�lem g�rmez.*/
    private bool IsPointerOverTile()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, tileLayerMask);

        return hit.collider != null;
    }
}
