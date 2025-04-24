using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    /*Bu method parametre olarak ald��� sahne ismini kullanarak sahne ge�i�i yapar.
   ilkolarak MusicManager singleton s�n�f�na eri�erek oyun m�zi�ini ba�lat�r (PlayGameplayMusic()).
   Bu sahne de�i�iminden sonra m�zi�in kald��� yerden veya uygun �ekilde yeniden ba�lamas�n� sa�lar.
   Ard�ndan SceneManager.LoadScene(sceneName) ile verilen sahne y�klenir.
   Dolay�s�yla kullan�c� ana men�den oyuna veya bir b�l�mden di�erine ge�i� yapabilir.*/
    public void LoadScene(string sceneName)
    {
        MusicManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene(sceneName);
    }
}
