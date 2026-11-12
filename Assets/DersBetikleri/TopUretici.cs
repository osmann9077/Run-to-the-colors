using UnityEngine;

public class TopUretici : MonoBehaviour
{
    [SerializeField] Sekme topPrefab;
    [SerializeField] int adet = 50;

    void Start()
    {
        for (int i = 0; i < adet; i++)
        {
            Sekme t = Instantiate(topPrefab, Vector3.zero, Quaternion.identity);
            t.hiz = Random.insideUnitCircle.normalized * Random.Range(2f, 5f);

            var sr = t.GetComponent<SpriteRenderer>();
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
