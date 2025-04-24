using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;
    private float savedTime = 0f;

    public AudioClip gameplayMusic;
    public AudioClip victoryMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        PlayGameplayMusic(); 
    }


    /*Bu method oyun s�ras�nda �alacak olan normal m�zi�i ba�lat�r.
    AudioSource bile�enine gameplayMusic atan�r.
    �nceden durdurulduysa kald��� yerden devam etmesi i�in audioSource.time = savedTime ile zaman geri verilir.
    Son olarak m�zik ba�lat�l�r ve loop (tekrar) a��k oldu�u i�in s�rekli �almaya devam eder.*/
    public void PlayGameplayMusic()
    {
        audioSource.clip = gameplayMusic;
        audioSource.time = savedTime; 
        audioSource.Play();
    }


   /*Bu method oyun kazan�ld���nda galibiyet m�zi�ini �almak i�in kullan�l�r.
   �nceki m�zi�in kald��� zaman� savedTime de�i�kenine kaydeder.
   Sonra victoryMusic atan�r s�f�rdan ba�lamas� i�in audioSource.time = 0 yap�l�r ve tekrar etmemesi i�in loop false yap�l�r.
   Son olarak galibiyet m�zi�i �al�n�r.*/
    public void PlayVictoryMusic()
    {
        savedTime = audioSource.time; 
        audioSource.clip = victoryMusic;
        audioSource.time = 0;
        audioSource.loop = false;
        audioSource.Play();
    }
}
