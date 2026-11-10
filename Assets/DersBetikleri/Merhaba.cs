using UnityEngine;
using UnityEngine.InputSystem;

// HAFTA 1: İLK BETİK
// Kurulum: Sahneye bir Square (2D Sprite) ekle, üzerine BoxCollider2D ve bu betiği koy.
public class Merhaba : MonoBehaviour
{
    int sayac = 0; // bu nesneye kaç kez dokunuldu

    // Start: nesne sahnede ilk kareden hemen önce bir kez çalışır
    void Start()
    {
        Debug.Log("Merhaba! Ben " + gameObject.name);
    }

    // Update: her karede bir kez çalışır
    void Update()
    {
        // Pointer: fare, dokunmatik ekran ve kalemin ortak adı
        var isaret = Pointer.current;
        if (isaret == null || !isaret.press.wasPressedThisFrame) return;

        Vector2 ekran = isaret.position.ReadValue();
        Vector2 dunya = Camera.main.ScreenToWorldPoint(ekran);

        // Dokunulan nokta bu nesnenin collider'ının içinde mi?
        if (GetComponent<Collider2D>().OverlapPoint(dunya))
        {
            sayac = sayac + 1;
            Debug.Log("Dokunuldu: " + sayac);

            // DENEY 1: sayaç 5'e ulaşınca kare gizlenir
            if (sayac >= 5) gameObject.SetActive(false);
        }
    }
}
