using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Splash : MonoBehaviour
{
    [SerializeField] float bekleme = 2.5f;
    float gecen = 0f;

    void Update()
    {
        gecen = gecen + Time.deltaTime;

        var isaret = Pointer.current;
        bool dokunuldu = isaret != null && isaret.press.wasPressedThisFrame;

        if (gecen >= bekleme || dokunuldu)
            SceneManager.LoadScene("Menu");
    }
}
