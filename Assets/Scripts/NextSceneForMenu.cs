using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    /*Bu method parametre olarak aldýðý sahne ismini kullanarak sahne geçiþi yapar.
   ilkolarak MusicManager singleton sýnýfýna eriþerek oyun müziðini baþlatýr (PlayGameplayMusic()).
   Bu sahne deðiþiminden sonra müziðin kaldýðý yerden veya uygun þekilde yeniden baþlamasýný saðlar.
   Ardýndan SceneManager.LoadScene(sceneName) ile verilen sahne yüklenir.
   Dolayýsýyla kullanýcý ana menüden oyuna veya bir bölümden diðerine geçiþ yapabilir.*/
    public void LoadScene(string sceneName)
    {
        MusicManager.Instance.PlayGameplayMusic();
        SceneManager.LoadScene(sceneName);
    }
}
