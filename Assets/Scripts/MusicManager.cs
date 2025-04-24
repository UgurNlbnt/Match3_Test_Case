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


    /*Bu method oyun sýrasýnda çalacak olan normal müziði baþlatýr.
    AudioSource bileþenine gameplayMusic atanýr.
    Önceden durdurulduysa kaldýðý yerden devam etmesi için audioSource.time = savedTime ile zaman geri verilir.
    Son olarak müzik baþlatýlýr ve loop (tekrar) açýk olduðu için sürekli çalmaya devam eder.*/
    public void PlayGameplayMusic()
    {
        audioSource.clip = gameplayMusic;
        audioSource.time = savedTime; 
        audioSource.Play();
    }


   /*Bu method oyun kazanýldýðýnda galibiyet müziðini çalmak için kullanýlýr.
   Önceki müziðin kaldýðý zamaný savedTime deðiþkenine kaydeder.
   Sonra victoryMusic atanýr sýfýrdan baþlamasý için audioSource.time = 0 yapýlýr ve tekrar etmemesi için loop false yapýlýr.
   Son olarak galibiyet müziði çalýnýr.*/
    public void PlayVictoryMusic()
    {
        savedTime = audioSource.time; 
        audioSource.clip = victoryMusic;
        audioSource.time = 0;
        audioSource.loop = false;
        audioSource.Play();
    }
}
