using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{

    public TMP_Text appleCountText;
    public TMP_Text orangeCountText;
    public TMP_Text cabbageCountText;
    public TMP_Text coconutCountText;


    public Image appleIcon;
    public Image orangeIcon;
    public Image cabbageIcon;
    public Image coconutIcon;


    public GameObject victoryPanel;
    public float panelScaleDuration = 0.5f;
    public string nextSceneName;

    private int appleCount = 0;
    private int orangeCount = 0;
    private int cabbageCount = 0;
    private int coconutCount = 0;
    [SerializeField] private bool isCompleted = false;

    public int appleGoal = 5;
    public int orangeGoal = 5;
    public int cabbageGoal = 5;
    public int coconutGoal = 5;



    /*Bu method sahne ba�lad���nda otomatik �al���r ve g�revleri s�f�rlamak i�in ResetGoals() metodunu �a��r�r.
    E�er victoryPanel atanm��sa onu sahne ba��nda gizli hale getirilir.
   Dolay�s�yla kazanma ekran�n�n animasyonla sonradan a��lmas� i�in haz�rlan�r.
   Ba�lang�� ayarlar�n� d�zenleyerek her level�in ba��n� temiz ba�lat�r.*/
    void Start()
    {
        ResetGoals();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            victoryPanel.transform.localScale = Vector3.zero;
        }
    }



    /*Bu method belirtilen meyve t�r�ne g�re hedefe ekleme yapar.
   Her meyve i�in maksimum hedef s�n�r�n� ge�meyecek �ekilde art�� sa�lan�r.
   UI �zerindeki say�lar g�ncellenir ve ard�ndan UpdateGoalColors() ile renk kontrol� yap�l�r.
   Son olarak hedeflerin tamamlan�p tamamlanmad��� CheckIfGoalsCompleted() ile kontrol edilir.
   E�er g�rev bitmi�se tamamlanma i�lemleri ba�lat�l�r.*/
    public void AddToGoal(string type, int amount)
    {
        if (isCompleted)
            return;

        switch (type)
        {
            case "apple":
                if (appleCount < appleGoal)
                    appleCount = Mathf.Min(appleCount + amount, appleGoal);
                appleCountText.text = $"{appleCount}/{appleGoal}";
                break;
            case "orange":
                if (orangeCount < orangeGoal)
                    orangeCount = Mathf.Min(orangeCount + amount, orangeGoal);
                orangeCountText.text = $"{orangeCount}/{orangeGoal}";
                break;
            case "cabbage":
                if (cabbageCount < cabbageGoal)
                    cabbageCount = Mathf.Min(cabbageCount + amount, cabbageGoal);
                cabbageCountText.text = $"{cabbageCount}/{cabbageGoal}";
                break;
            case "coconut":
                if (coconutCount < coconutGoal)
                    coconutCount = Mathf.Min(coconutCount + amount, coconutGoal);
                coconutCountText.text = $"{coconutCount}/{coconutGoal}";
                break;
        }

        UpdateGoalColors();
        CheckIfGoalsCompleted();
    }



    /*Bu method g�revdeki her meyvenin miktar� hedefe ula�t�ysa yaz� rengini ye�il yapar.
   Hedefe ula��lmam��sa k�rm�z� olarak kal�r.
   Bu sayede oyuncu hangi hedefin tamamland���n� h�zl�ca g�r�r.*/
    private void UpdateGoalColors()
    {
        appleCountText.color = appleCount >= appleGoal ? Color.green : Color.red;
        orangeCountText.color = orangeCount >= orangeGoal ? Color.green : Color.red;
        cabbageCountText.color = cabbageCount >= cabbageGoal ? Color.green : Color.red;
        coconutCountText.color = coconutCount >= coconutGoal ? Color.green : Color.red;
    }



    /*Bu method g�revleri s�f�rlayarak yeniden ba�lat�r.
   T�m meyve say�lar� s�f�ra �ekilir ve hedef metinleri g�ncellenir.
   Ayn� zamanda renkler de UpdateGoalColors() ile s�f�rlan�r.
   Her sahne a��ld���nda ya da yeniden ba�lat�ld���nda g�revlerin d�zg�n g�r�nmesini sa�lar.*/
    public void ResetGoals()
    {
        isCompleted = false;
        appleCount = orangeCount = cabbageCount = coconutCount = 0;
        appleCountText.text = $"0/{appleGoal}";
        orangeCountText.text = $"0/{orangeGoal}";
        cabbageCountText.text = $"0/{cabbageGoal}";
        coconutCountText.text = $"0/{coconutGoal}";
        UpdateGoalColors();
    }



    /*T�m meyve say�lar� hedef de�erlerine ula�t���nda g�rev tamamlanm�� say�l�r.
   Bu durumda isCompleted true yap�l�r ve ShowVictoryPanel() �a�r�l�r.
   GridManager bulunup devre d��� b�rak�larak oyuncunun daha fazla etkile�im yapmas� engellenir.(Zafer Paneli a��l�r)
   G�revlerin do�ru �ekilde sonland���n� kontrol eden kritik bir methoddur.*/
    private void CheckIfGoalsCompleted()
    {
        if (appleCount >= appleGoal && orangeCount >= orangeGoal && cabbageCount >= cabbageGoal && coconutCount >= coconutGoal)
        {
            isCompleted = true;
            ShowVictoryPanel();
            var gridM = FindObjectOfType<GridManager>();
            if (gridM != null) gridM.enabled = false;
        }
    }

    /*Bu method galibiyet panelini aktif eder ve s�f�rdan tam boyuta DOTween animasyonu ile b�y�t�r.
   Panel ho� bir �ekilde ekrana gelir (Ease.OutBack kullan�larak).
   Ayr�ca MusicManager �zerinden galibiyet m�zi�i �al�n�r.
   Oyuncuya g�revleri ba�ard���n� hem g�rsel hem i�itsel olarak bildirir.*/
    private void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        victoryPanel.transform.localScale = Vector3.zero;
        victoryPanel.transform.DOScale(Vector3.one, panelScaleDuration).SetEase(Ease.OutBack);
        MusicManager.Instance.PlayVictoryMusic();
    }


    /*Bu method "Devam" butonuna bas�ld���nda �al���r.
   �ncelikle oyun m�zi�i tekrar ba�lat�l�r.
   Ard�ndan nextSceneName bo� de�ilse, bu sahne y�klenir.
   B�ylece bir sonraki levele ge�i� yap�l�r ve oyun devam eder.*/
    public void OnNextButtonPressed()
    {
        MusicManager.Instance.PlayGameplayMusic();
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }

    public bool IsCompleted
    {
        get 
        { 
            return isCompleted; 
        }

    }
}