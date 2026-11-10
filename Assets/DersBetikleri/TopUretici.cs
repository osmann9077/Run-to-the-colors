using UnityEngine;

// HAFTA 3: KOŞULLAR VE DÖNGÜLER (Sekme.cs'den ayrı dosyaya taşındı)
public class TopUretici : MonoBehaviour
{
    [SerializeField] Sekme topPrefab;
    [SerializeField] int adet = 50;   // DENEY 1: 10 → 50

    void Start()
    {
        // DÖNGÜ: aynı işi 'adet' kez yap
        for (int i = 0; i < adet; i++)
        {
            Sekme t = Instantiate(topPrefab, Vector3.zero, Quaternion.identity);
            t.hiz = Random.insideUnitCircle.normalized * Random.Range(2f, 5f);

            // switch: duruma göre seçim
            var sr = t.GetComponent<SpriteRenderer>();
            // DENEY 2: dördüncü renk eklendi (i % 3 → i % 4)
            switch (i % 4)
            {
                case 0: sr.color = Color.red; break;
                case 1: sr.color = Color.blue; break;
                case 2: sr.color = Color.green; break;
                default: sr.color = Color.yellow; break;
            }
        }
    }
}
