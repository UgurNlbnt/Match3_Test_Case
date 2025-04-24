using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEngine.U2D;
using TMPro;

public class GridManager : MonoBehaviour
{
    public int gridRows = 5;
    public int gridCols = 5;
    public GameObject tilePrefab;

    public Sprite[] fruitSprites;

    public GameObject particleEffectPrefab;
    public Transform uiCanvas;
    public RectTransform appleTargetUI;
    public RectTransform orangeTargetUI;
    public RectTransform cabbageTargetUI;
    public RectTransform coconutTargetUI;

    private GameObject[,] fruitGrid;

    public AudioSource mergeAudioSource;
    public AudioClip mergeClip;
    private int mergeCounter = 0;

    public TextMeshProUGUI yummyText;
    public AudioSource sfxSource;
    public AudioClip yummyClip;

    private GoalManager goalManager;


    void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        CreateFruitGrid();
    }

    /*Grid'i oluşturmak için çift döngüyle sırayla satır ve sütunlar dolaşılır.
    Her hücre için prefab kopyalanır, rastgele bir sprite atanır ama 3’lü eşleşme olmamasına dikkat edilir.
    Bunu sağlamak için IsMatchAt() methoduyla önceki 2 tile kontrol edilir.
    Aynı sprite’tan 3’lü bir dizi olacaksa yeni sprite çekilir, bu işlem 100 deneme ile sınırlandırılır.
    Son olarak tile, grid dizisine kaydedilir ve oyun alanı doldurulmuş olur.
    Bu yapı oyun başladığında eşleşmelerin otomatik oluşmasını engellemek için önemlidir.*/
    void CreateFruitGrid()
    {
        fruitGrid = new GameObject[gridRows, gridCols];

        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridCols; col++)
            {
                Vector2 spawnPos = GetWorldPosition(row, col);
                GameObject newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                Tile tileScript = newTile.GetComponent<Tile>();

                Sprite selectedSprite;
                int attemptCount = 0;
                do
                {
                    selectedSprite = fruitSprites[Random.Range(0, fruitSprites.Length)];
                    attemptCount++;
                }
                while (attemptCount < 100 && ((row >= 2 && IsMatchAt(row - 1, col, row - 2, col, selectedSprite)) || (col >= 2 && IsMatchAt(row, col - 1, row, col - 2, selectedSprite))));

                tileScript.SetSprite(selectedSprite);
                fruitGrid[row, col] = newTile;
            }
        }
    }

    /*bu iki method swipe yani kaydırma event’ine abone olma ve abonelikten çıkma işlemlerini yapar.
    SwipeController sınıfındaki OnSwipeDetected event’i aktif sahneye bağlanır.
    Bu sayede sahne aktifken swipe algılanır sahneden çıkıldığında ise dinleme durdurulur.*/
    private void OnEnable()
    {
        SwipeController.OnSwipeDetected += OnSwipe;
    }
    private void OnDisable()
    {
        SwipeController.OnSwipeDetected -= OnSwipe;
    }

    /*Swipe geldiğinde çağrılan bu method, HandleSwipe() fonksiyonu ile kullanıcı hareketine göre satır ya da sütun kaydırması yapar.
    Ardından ProcessAllMatches() coroutine’i başlatılarak eşleşmelerin işlenmesi sağlanır.
    Swipe sonrası kontrol ve animasyon işlemlerini tetikler.*/
    private void OnSwipe(Vector2 swipeDirection)
    {
        if (goalManager.IsCompleted) 
            return;
        HandleSwipe(swipeDirection);
        StartCoroutine(ProcessAllMatches());
    }


    /*Kullanıcının dokunduğu noktaya en yakın tile tespit edilir.
   Swipe yönü yataysa satır, dikeyse sütun kaydırması yapılır.
   Swipe yönüne göre uygun kaydırma fonksiyonu (ShiftRowRight, ShiftRowLeft, ShiftColumnUp, ShiftColumnDown) çağrılır.
   Bu sayede hangi meyve sırasının hareket edeceği belirlenir.
   Swipe pozisyonunun grid’e en yakın hücre ile eşleşmesi bu method sayesinde yapılır.*/
    private void HandleSwipe(Vector2 direction)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float closestDistance = float.MaxValue;
        int selectedRow = -1, selectedCol = -1;

        for (int row = 0; row < gridRows; row++)
            for (int col = 0; col < gridCols; col++)
            {
                float dist = Vector2.Distance(fruitGrid[row, col].transform.position, mouseWorldPos);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    selectedRow = row;
                    selectedCol = col;
                }
            }

        if (selectedRow < 0)
            return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) ShiftRowRight(selectedCol);

            else ShiftRowLeft(selectedCol);
        }
        else
        {
            if (direction.y > 0) ShiftColumnUp(selectedRow);

            else ShiftColumnDown(selectedRow);
        }
    }

    /*Bu methodlar ilgili satırı veya sütunu belirtilen yöne kaydırır.
   Tile’ların pozisyonları güncellenir ve DOMove ile animasyon uygulanır.
   Son tile başa veya sona alınarak görsel kaydırma işlemi tamamlanır.
   Her biri swipe yönüne göre grid’i fiziksel olarak yeniden düzenler.*/
    private void ShiftRowRight(int row)
    {
        GameObject lastTile = fruitGrid[gridRows - 1, row];
        lastTile.transform.position = GetWorldPosition(-1, row);
        lastTile.transform.DOMove(GetWorldPosition(0, row), 0.2f).SetEase(Ease.OutQuad);
        for (int i = gridRows - 1; i > 0; i--)
        {
            fruitGrid[i, row] = fruitGrid[i - 1, row];
            fruitGrid[i, row].transform.DOMove(GetWorldPosition(i, row), 0.2f).SetEase(Ease.OutQuad);
        }
        fruitGrid[0, row] = lastTile;
    }

    private void ShiftRowLeft(int row)
    {
        GameObject firstTile = fruitGrid[0, row];
        firstTile.transform.position = GetWorldPosition(gridRows, row);
        firstTile.transform.DOMove(GetWorldPosition(gridRows - 1, row), 0.2f).SetEase(Ease.OutQuad);
        for (int i = 0; i < gridRows - 1; i++)
        {
            fruitGrid[i, row] = fruitGrid[i + 1, row];
            fruitGrid[i, row].transform.DOMove(GetWorldPosition(i, row), 0.2f).SetEase(Ease.OutQuad);
        }
        fruitGrid[gridRows - 1, row] = firstTile;
    }

    private void ShiftColumnUp(int col)
    {
        GameObject lastTile = fruitGrid[col, gridCols - 1];
        lastTile.transform.position = GetWorldPosition(col, -1);
        lastTile.transform.DOMove(GetWorldPosition(col, 0), 0.2f).SetEase(Ease.OutQuad);
        for (int i = gridCols - 1; i > 0; i--)
        {
            fruitGrid[col, i] = fruitGrid[col, i - 1];
            fruitGrid[col, i].transform.DOMove(GetWorldPosition(col, i), 0.2f).SetEase(Ease.OutQuad);
        }
        fruitGrid[col, 0] = lastTile;
    }

    private void ShiftColumnDown(int col)
    {
        GameObject firstTile = fruitGrid[col, 0];
        firstTile.transform.position = GetWorldPosition(col, gridCols);
        firstTile.transform.DOMove(GetWorldPosition(col, gridCols - 1), 0.2f).SetEase(Ease.OutQuad);
        for (int i = 0; i < gridCols - 1; i++)
        {
            fruitGrid[col, i] = fruitGrid[col, i + 1];
            fruitGrid[col, i].transform.DOMove(GetWorldPosition(col, i), 0.2f).SetEase(Ease.OutQuad);
        }
        fruitGrid[col, gridCols - 1] = firstTile;
    }

    /*Bu method satır ve sütuna göre grid’deki tile’ın dünya pozisyonunu hesaplar.
   Offset değerleriyle grid ekranın ortasında düzgün şekilde hizalanır.
   Spawn ve animasyon işlemleri bu pozisyonlar üzerinden yapılır.*/
    private Vector2 GetWorldPosition(int row, int col)
    {
        float X = -gridCols / 2f;
        float Y = -gridRows / 2f;
        return new Vector2((row + X) * 1.45f, (col + Y) * 1.46f);
    }

    /*Bu coroutine eşleşme olduğu sürece döner ve eşleşmeleri işler.
   Her turda CheckMatches() çağrılır, eşleşme varsa CollapseGrid() ve RefillGrid() devreye girer.
   Aralara WaitForSeconds koyarak görsel akışı sağlıklı hale getirir.
   Tüm eşleşmeler bitene kadar döngü devam eder.*/
    private IEnumerator ProcessAllMatches()
    {
        if (goalManager.IsCompleted) yield break;

        bool found;
        do
        {
            found = CheckMatches();
            if (found)
            {
                CollapseGrid();
                RefillGrid();
                yield return new WaitForSeconds(0.6f);
            }
            if (goalManager.IsCompleted) yield break;
        } while (found);

        if (mergeCounter > 6)
            StartCoroutine(ShowYummy());
    }


    /*Grid’de yatay ve dikey eşleşmeleri kontrol eder.
    Her satır ve sütun için 3 veya daha fazla aynı sprite varsa bunları işaretler.
    Eşleşen gruplar için ProcessMatchHorizontal ve ProcessMatchVertical methodları çağrılır.
    Her başarılı eşleşme sonrası true döndürerek tekrar işlem yapılmasını sağlar.*/
    private bool CheckMatches()
    {
        bool anyMatches = false;
        for (int col = 0; col < gridCols; col++)
        {
            int count = 1;
            for (int row = 1; row < gridRows; row++)
            {
                if (IsSameSprite(row, col, row - 1, col)) count++;
                else
                {
                    if (count >= 3) { anyMatches = true; ProcessMatchHorizontal(row - 1, col, count); }
                    count = 1;
                }
            }
            if (count >= 3) { anyMatches = true; ProcessMatchHorizontal(gridRows - 1, col, count); }
        }

        for (int row = 0; row < gridRows; row++)
        {
            int count = 1;
            for (int col = 1; col < gridCols; col++)
            {
                if (IsSameSprite(row, col, row, col - 1)) count++;
                else
                {
                    if (count >= 3) { anyMatches = true; ProcessMatchVertical(row, col - 1, count); }
                    count = 1;
                }
            }
            if (count >= 3) { anyMatches = true; ProcessMatchVertical(row, gridCols - 1, count); }
        }
        return anyMatches;
    }


    /*İki tile’ın sprite’larının aynı olup olmadığını kontrol eder.
    Bu eşleşme tespiti için kullanılır.
    Null kontrolleriyle güvenli hale getirilmiştir.*/
    private bool IsSameSprite(int row1, int col1, int row2, int col2)
    {
        if (fruitGrid[row1, col1] == null || fruitGrid[row2, col2] == null) return false;
        return fruitGrid[row1, col1].GetComponent<Tile>().GetSprite() == fruitGrid[row2, col2].GetComponent<Tile>().GetSprite();
    }


    /*Grid oluşturulurken 3'lü eşleşmeleri engellemek için çalışır.
     Verilen sprite önceki 2 tile ile aynıysa true döner.
     Bu sayede ilk açılışta oluşan otomatik eşleşmeler engellenir.*/
    private bool IsMatchAt(int row1, int col1, int row2, int col2, Sprite sprite)
    {
        if (fruitGrid[row1, col1] == null || fruitGrid[row2, col2] == null) return false;
        Sprite s1 = fruitGrid[row1, col1].GetComponent<Tile>().GetSprite();
        Sprite s2 = fruitGrid[row2, col2].GetComponent<Tile>().GetSprite();
        return s1 == sprite && s2 == sprite;
    }


    /*Belirli bir sayıda yatay veya dikey eşleşme bulunduğunda çalışır.
  GoalManager’a kaç meyve toplandığını bildirir.
  Her tile için AnimateMatch() çağrılır ve tile sahneden silinir.
  Bu methodlar görev takibi ve eşleşen meyvelerin kaldırılmasından sorumludur.*/
    private void ProcessMatchHorizontal(int endRow, int col, int count)
    {
        string type = GetTypeFromSprite(fruitGrid[endRow, col].GetComponent<Tile>().GetSprite());
        FindObjectOfType<GoalManager>().AddToGoal(type, count);
        for (int i = 0; i < count; i++)
        {
            mergeCounter++;
            mergeAudioSource.pitch = 1f + (mergeCounter - 1) * 0.1f;
            mergeAudioSource.PlayOneShot(mergeClip);
            AnimateMatch(fruitGrid[endRow - i, col], type);
            fruitGrid[endRow - i, col] = null;
        }
    }
    private void ProcessMatchVertical(int row, int endCol, int count)
    {
        string type = GetTypeFromSprite(fruitGrid[row, endCol].GetComponent<Tile>().GetSprite());
        FindObjectOfType<GoalManager>().AddToGoal(type, count);
        for (int i = 0; i < count; i++)
        {
            mergeCounter++;
            mergeAudioSource.pitch = 1f + (mergeCounter - 1) * 0.1f;
            mergeAudioSource.PlayOneShot(mergeClip);
            AnimateMatch(fruitGrid[row, endCol - i], type);
            fruitGrid[row, endCol - i] = null;
        }
    }

    private IEnumerator ShowYummy()
    {
        yummyText.gameObject.SetActive(true);
        sfxSource.PlayOneShot(yummyClip);

       
        RectTransform rt = yummyText.rectTransform;
        rt.anchoredPosition = new Vector2(0, -Screen.height / 2f);

    
        rt.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack);

     
        yield return new WaitForSeconds(1.4f);

        rt.DOAnchorPosY(Screen.height / 2f, 0.5f).OnComplete(() =>{
              yummyText.gameObject.SetActive(false);
              mergeCounter = 0;
              mergeAudioSource.pitch = 1f;
          });
    }

    private void OnAllMatchesDone()
    {
        if (mergeCounter > 6)
            StartCoroutine(ShowYummy());
    }


    /*Eşleşen tile’a özel particle efekti oluşturur ve UI’daki görev paneline doğru bir ikon uçurur.
    Kameradan dünya konumu alınır ve geçici bir Image oluşturularak animasyon başlatılır.
    Hedef UI bileşenine doğru DOMove yapılır ve sonunda obje yok edilir.
    Bu sayede oyuncuya görsel geri bildirim verilir ve interaktiflik artar.*/
    private void AnimateMatch(GameObject tile, string type)
    {
        if (tile == null) return;
        Vector3 worldPos = tile.transform.position;
        GameObject particle = Instantiate(particleEffectPrefab, worldPos, Quaternion.identity);
        Transform targetUI = null;
        switch (type)
        {
            case "apple": targetUI = appleTargetUI; 
                break;
            case "orange": targetUI = orangeTargetUI; 
                break;
            case "cabbage": targetUI = cabbageTargetUI; 
                break;
            case "coconut": targetUI = coconutTargetUI; 
                break;
        }
        if (targetUI != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            GameObject flyObj = new GameObject("FlyIcon");
            flyObj.transform.SetParent(uiCanvas);
            Image img = flyObj.AddComponent<Image>();
            img.sprite = tile.GetComponent<SpriteRenderer>().sprite;
            img.rectTransform.sizeDelta = new Vector2(50, 50);
            flyObj.transform.position = screenPos;
            flyObj.transform.DOMove(targetUI.position, 0.5f).OnComplete(() => Destroy(flyObj));
        }
        Destroy(particle, 0.5f);
        Destroy(tile, 0.1f);
    }


    /*Boş kalan hücrelerin üzerindeki meyveler aşağıya kaydırılır.
    Her satırda null olan tile’lar kontrol edilir ve dolu tile varsa pozisyonları değiştirilir.
   DOMove ile animasyon uygulanır.
   Bu method eşleşme sonrası boşlukları doldurur.*/
    private void CollapseGrid()
    {
        for (int row = 0; row < gridRows; row++)
            for (int col = 0; col < gridCols; col++)
                if (fruitGrid[row, col] == null)
                    for (int newCol = col + 1; newCol < gridCols; newCol++)
                        if (fruitGrid[row, newCol] != null)
                        {
                            fruitGrid[row, col] = fruitGrid[row, newCol];
                            fruitGrid[row, newCol] = null;
                            fruitGrid[row, col].transform.DOMove(GetWorldPosition(row, col), 0.2f);
                            break;
                        }
    }


    /*Boş kalan hücreler için yeni tile prefab’ları oluşturulur.
   Yukarıdan spawn edilerek aşağıya doğru hareket ettirilir.
   Random sprite atanır ve grid’e yerleştirilir.
   Bu method ile grid her zaman dolu kalır.*/
    private void RefillGrid()
    {
        for (int row = 0; row < gridRows; row++)
            for (int col = 0; col < gridCols; col++)
                if (fruitGrid[row, col] == null)
                {
                    Vector2 spawnPos = GetWorldPosition(row, col + gridCols);
                    GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                    Tile tileScript = tile.GetComponent<Tile>();
                    tileScript.SetSprite(fruitSprites[Random.Range(0, fruitSprites.Length)]);
                    tile.transform.DOMove(GetWorldPosition(row, col), 0.3f);
                    fruitGrid[row, col] = tile;
                }
    }


    /*Sprite ismine göre hangi meyve türü olduğunu belirler.
   İsim kontrolü lowercase olarak yapılır.
   Tanımlı meyve adlarıyla eşleşme varsa ilgili tip döner (apple, orange, cabbage, coconut).
   Görev paneline bildirim göndermek için kullanılır.*/
    private string GetTypeFromSprite(Sprite sprite)
    {
        string spriteName = sprite.name.ToLower();
        if (spriteName.Contains("elma")) 
            return "apple";
        if (spriteName.Contains("portakal")) 
            return "orange";
        if (spriteName.Contains("lahana")) 
            return "cabbage";
        if (spriteName.Contains("hindistan")) 
            return "coconut";
        return "";
    }
}
