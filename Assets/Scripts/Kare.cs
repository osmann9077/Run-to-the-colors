using UnityEngine;

public class Kare : MonoBehaviour
{
    public int renkNo;
    [SerializeField] float dusmeHizi = 2f;

    SpriteRenderer sr;
    Vector3 ilkBoyut;
    bool dusuyor = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ilkBoyut = transform.localScale;
    }

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

    public bool UstundeMi(Vector2 nokta)
    {
        return GetComponent<Collider2D>().OverlapPoint(nokta);
    }

    void Update()
    {
        if (!dusuyor) return;

        transform.localScale -= ilkBoyut * dusmeHizi * Time.deltaTime;
        if (transform.localScale.x <= 0f)
        {
            dusuyor = false;
            gameObject.SetActive(false);
        }
    }
}
