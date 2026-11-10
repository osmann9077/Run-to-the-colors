using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// 1. ekran: oyunun adı birkaç saniye görünür, sonra Menü açılır.
public class Splash : MonoBehaviour
{
    [SerializeField] float bekleme = 2.5f;   // kaç saniye görünsün
    float gecen = 0f;

    void Update()
    {
        gecen = gecen + Time.deltaTime;

        // Süre dolduysa ya da ekrana dokunulduysa menüye geç
        var isaret = Pointer.current;
        bool dokunuldu = isaret != null && isaret.press.wasPressedThisFrame;

        if (gecen >= bekleme || dokunuldu)
            SceneManager.LoadScene("Menu");
    }
}
