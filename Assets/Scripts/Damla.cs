using UnityEngine;

// Su sıçraması damlası: verilen yöne uçar, solar ve kendini yok eder.
// Kurulum: küçük daire sprite + bu betik → Prefab. OyunYonetici.SuSicrat üretir.
public class Damla : MonoBehaviour
{
    public Vector2 hiz;                       // OyunYonetici rastgele verir
    [SerializeField] float omur = 0.6f;       // kaç saniye görünsün

    SpriteRenderer sr;
    float gecen = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position += (Vector3)(hiz * Time.deltaTime);

        // Zaman geçtikçe saydamlaş (alfa 1 → 0)
        gecen = gecen + Time.deltaTime;
        Color c = sr.color;
        c.a = 1f - gecen / omur;
        sr.color = c;

        if (gecen >= omur) Destroy(gameObject);
    }
}
