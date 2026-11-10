using UnityEngine;

// HAFTA 3: KOŞULLAR VE DÖNGÜLER
// Top: Sprite + bu betik. Üretici: boş GameObject + TopUretici.
public class Sekme : MonoBehaviour
{
    public Vector2 hiz = new Vector2(3f, 2f);
    Vector2 sinir;

    void Start()
    {
        // Kameranın gördüğü alanın yarı genişliği / yüksekliği (dünya birimi)
        Camera k = Camera.main;
        sinir = new Vector2(k.orthographicSize * k.aspect, k.orthographicSize);
    }

    void Update()
    {
        Vector3 p = transform.position + (Vector3)(hiz * Time.deltaTime);

        // KOŞUL: kenara değdiyse yönü ters çevir
        if (p.x > sinir.x || p.x < -sinir.x) hiz.x = -hiz.x;
        if (p.y > sinir.y || p.y < -sinir.y) hiz.y = -hiz.y;

        transform.position = p;
    }
}
