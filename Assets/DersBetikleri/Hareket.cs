using UnityEngine;

public class Hareket : MonoBehaviour
{
    [SerializeField] float hiz = 10f;
    [SerializeField] bool aktif = true;
    [SerializeField] int can = 3;
    [SerializeField] string ad = "Pierrot";
    [SerializeField] float sagSinir = 8f;

    void Start()
    {
        Debug.Log(ad + " sahneye çıktı. Can: " + can);
    }

    void Update()
    {
        if (!aktif) return;

        transform.position += Vector3.left * hiz * Time.deltaTime;

        if (transform.position.x < -sagSinir)
            transform.position = new Vector3(sagSinir, transform.position.y, 0f);
    }

    public void HasarAl()
    {
        can = can - 1;
        Debug.Log(ad + " hasar aldı. Kalan can: " + can);
    }
}
