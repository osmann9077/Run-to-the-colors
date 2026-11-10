using UnityEngine;

// HAFTA 2: DEĞİŞKENLER VE INSPECTOR
public class Hareket : MonoBehaviour
{
    [SerializeField] float hiz = 10f;       // birim / saniye (float: kesirli sayı)
    [SerializeField] bool aktif = true;     // hareket açık mı?
    [SerializeField] int can = 3;           // int: tam sayı
    [SerializeField] string ad = "Pierrot"; // string: metin
    [SerializeField] float sagSinir = 8f;

    void Start()
    {
        Debug.Log(ad + " sahneye çıktı. Can: " + can);
    }

    void Update()
    {
        if (!aktif) return;

        // Time.deltaTime: son kareden bu yana geçen saniye.
        // 60 FPS'te de 30 FPS'te de saniyede 'hiz' birim yol alınır.
        transform.position += Vector3.right * hiz * Time.deltaTime;

        if (transform.position.x > sagSinir)
            transform.position = new Vector3(-sagSinir, transform.position.y, 0f);
    }

    public void HasarAl()
    {
        can = can - 1;
        Debug.Log(ad + " hasar aldı. Kalan can: " + can);
    }
}
