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



    /*Bu method sahne baþladýðýnda otomatik çalýþýr ve görevleri sýfýrlamak için ResetGoals() metodunu çaðýrýr.
    Eðer victoryPanel atanmýþsa onu sahne baþýnda gizli hale getirilir.
   Dolayýsýyla kazanma ekranýnýn animasyonla sonradan açýlmasý için hazýrlanýr.
   Baþlangýç ayarlarýný düzenleyerek her level’in baþýný temiz baþlatýr.*/
    void Start()
    {
        ResetGoals();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            victoryPanel.transform.localScale = Vector3.zero;
        }
    }



    /*Bu method belirtilen meyve türüne göre hedefe ekleme yapar.
   Her meyve için maksimum hedef sýnýrýný geçmeyecek þekilde artýþ saðlanýr.
   UI üzerindeki sayýlar güncellenir ve ardýndan UpdateGoalColors() ile renk kontrolü yapýlýr.
   Son olarak hedeflerin tamamlanýp tamamlanmadýðý CheckIfGoalsCompleted() ile kontrol edilir.
   Eðer görev bitmiþse tamamlanma iþlemleri baþlatýlýr.*/
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



    /*Bu method görevdeki her meyvenin miktarý hedefe ulaþtýysa yazý rengini yeþil yapar.
   Hedefe ulaþýlmamýþsa kýrmýzý olarak kalýr.
   Bu sayede oyuncu hangi hedefin tamamlandýðýný hýzlýca görür.*/
    private void UpdateGoalColors()
    {
        appleCountText.color = appleCount >= appleGoal ? Color.green : Color.red;
        orangeCountText.color = orangeCount >= orangeGoal ? Color.green : Color.red;
        cabbageCountText.color = cabbageCount >= cabbageGoal ? Color.green : Color.red;
        coconutCountText.color = coconutCount >= coconutGoal ? Color.green : Color.red;
    }



    /*Bu method görevleri sýfýrlayarak yeniden baþlatýr.
   Tüm meyve sayýlarý sýfýra çekilir ve hedef metinleri güncellenir.
   Ayný zamanda renkler de UpdateGoalColors() ile sýfýrlanýr.
   Her sahne açýldýðýnda ya da yeniden baþlatýldýðýnda görevlerin düzgün görünmesini saðlar.*/
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



    /*Tüm meyve sayýlarý hedef deðerlerine ulaþtýðýnda görev tamamlanmýþ sayýlýr.
   Bu durumda isCompleted true yapýlýr ve ShowVictoryPanel() çaðrýlýr.
   GridManager bulunup devre dýþý býrakýlarak oyuncunun daha fazla etkileþim yapmasý engellenir.(Zafer Paneli açýlýr)
   Görevlerin doðru þekilde sonlandýðýný kontrol eden kritik bir methoddur.*/
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

    /*Bu method galibiyet panelini aktif eder ve sýfýrdan tam boyuta DOTween animasyonu ile büyütür.
   Panel hoþ bir þekilde ekrana gelir (Ease.OutBack kullanýlarak).
   Ayrýca MusicManager üzerinden galibiyet müziði çalýnýr.
   Oyuncuya görevleri baþardýðýný hem görsel hem iþitsel olarak bildirir.*/
    private void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        victoryPanel.transform.localScale = Vector3.zero;
        victoryPanel.transform.DOScale(Vector3.one, panelScaleDuration).SetEase(Ease.OutBack);
        MusicManager.Instance.PlayVictoryMusic();
    }


    /*Bu method "Devam" butonuna basýldýðýnda çalýþýr.
   Öncelikle oyun müziði tekrar baþlatýlýr.
   Ardýndan nextSceneName boþ deðilse, bu sahne yüklenir.
   Böylece bir sonraki levele geçiþ yapýlýr ve oyun devam eder.*/
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