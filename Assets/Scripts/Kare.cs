using UnityEngine;

// Zemindeki tek bir renkli kare.
// Kurulum: Square sprite + BoxCollider2D + bu betik → Prefab yap.
public class Kare : MonoBehaviour
{
    public int renkNo;                // 0 kırmızı, 1 yeşil, 2 sarı, 3 mor
    [SerializeField] float dusmeHizi = 2f;   // küçülme hızı (saniyede)

    SpriteRenderer sr;
    Vector3 ilkBoyut;
    bool dusuyor = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ilkBoyut = transform.localScale;
    }

    // Kareyi yeniden ortaya çıkarır ve rengini verir
    public void RenkVer(int no)
    {
        renkNo = no;
        sr.color = OyunYonetici.Renk(no);
        transform.localScale = ilkBoyut;
        dusuyor = false;
        gameObject.SetActive(true);
    }

    public void Dus()
    {
        dusuyor = true;
    }

    // Verilen nokta bu karenin üstünde mi? (Merhaba.cs'deki OverlapPoint)
    public bool UstundeMi(Vector2 nokta)
    {
        return GetComponent<Collider2D>().OverlapPoint(nokta);
    }

    void Update()
    {
        if (!dusuyor) return;

        // Suya batıyormuş gibi küçül
        transform.localScale -= ilkBoyut * dusmeHizi * Time.deltaTime;
        if (transform.localScale.x <= 0f)
        {
            dusuyor = false;
            gameObject.SetActive(false);
        }
    }
}
