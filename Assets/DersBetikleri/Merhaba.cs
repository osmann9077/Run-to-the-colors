using UnityEngine;
using UnityEngine.InputSystem;

public class Merhaba : MonoBehaviour
{
    int sayac = 0;

    void Start()
    {
        Debug.Log("Merhaba! Ben " + gameObject.name);
    }

    void Update()
    {
        var isaret = Pointer.current;
        if (isaret == null || !isaret.press.wasPressedThisFrame) return;

        Vector2 ekran = isaret.position.ReadValue();
        Vector2 dunya = Camera.main.ScreenToWorldPoint(ekran);

        if (GetComponent<Collider2D>().OverlapPoint(dunya))
        {
            sayac = sayac + 1;
            Debug.Log("Dokunuldu: " + sayac);

            transform.localScale = transform.localScale * 0.9f;

            if (sayac >= 5) gameObject.SetActive(false);
        }
    }
}
