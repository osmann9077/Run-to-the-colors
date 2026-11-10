using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Oyunun beyni: zemini kurar, turları yönetir, kazanma/kaybetmeye karar verir.
// KURAL: Süre bitince ekrandaki renkte olmayan kareler suya düşer.
//        Doğru renkte değilsen sen de düşersin ve 1 can gider.
//        toplamTur kadar dayanırsan kazanırsın, canın biterse kaybedersin.
public class OyunYonetici : MonoBehaviour
{
    [Header("Zemin")]
    [SerializeField] Kare karePrefab;
    [SerializeField] int kenarAdet = 4;        // 4x4 = 16 kare
    [SerializeField] float aralik = 1.5f;      // kareler arası mesafe

    [Header("Oyun ayarları")]
    [SerializeField] float turSuresi = 3f;     // ilk turda kaç saniye var
    [SerializeField] float hizlanma = 0.2f;    // her tur süre bu kadar kısalır
    [SerializeField] int toplamTur = 10;
    [SerializeField] int can = 3;
    [SerializeField] float beklemeSuresi = 1.5f;   // kareler düştükten sonra bekleme

    [Header("Bağlantılar")]
    [SerializeField] Oyuncu oyuncu;
    [SerializeField] SpriteRenderer renkEkrani;    // arenanın üstündeki ekran
    [SerializeField] TMP_Text ekranYazisi;         // "YEŞİL!"
    [SerializeField] TMP_Text bilgiYazisi;         // tur, can, süre
    [SerializeField] GameObject sonucPaneli;
    [SerializeField] TMP_Text sonucYazisi;
    [SerializeField] SpriteRenderer ekranSekli;    // renk körü modu: ekrandaki şekil
    [SerializeField] Sprite[] sekiller;            // 0 daire, 1 üçgen, 2 kare, 3 yıldız

    [Header("Sesler")]
    [SerializeField] AudioSource hoparlor;
    [SerializeField] AudioClip turSesi;
    [SerializeField] AudioClip dusmeSesi;
    [SerializeField] AudioClip kazanmaSesi;
    [SerializeField] AudioClip kaybetmeSesi;

    Kare[] kareler;
    int hedefRenk;
    int tur = 0;
    float sayac;            // geri sayım
    bool turSuruyor = false;
    bool bekleniyor = false;
    bool oyuncuDustu = false;

    void Start()
    {
        sonucPaneli.SetActive(false);

        // Menüdeki Ayarlar'dan seçilen zorluk: süreyi çarparak değiştirir
        int zorluk = PlayerPrefs.GetInt("zorluk", 1);
        if (zorluk == 0) turSuresi = turSuresi * 1.4f;   // Kolay: daha çok süre
        if (zorluk == 2) turSuresi = turSuresi * 0.7f;   // Zor: daha az süre

        // DÖNGÜ: zemindeki kareleri üret (TopUretici.cs'deki for + Instantiate)
        kareler = new Kare[kenarAdet * kenarAdet];
        float bas = -(kenarAdet - 1) * aralik / 2f;   // ilk karenin konumu (ortalamak için)
        for (int i = 0; i < kareler.Length; i++)
        {
            int sutun = i % kenarAdet;
            int satir = i / kenarAdet;
            Vector3 konum = new Vector3(bas + sutun * aralik, bas + satir * aralik, 0f);
            kareler[i] = Instantiate(karePrefab, konum, Quaternion.identity);
        }

        YeniTur();
    }

    void YeniTur()
    {
        tur = tur + 1;

        // Her kareye rastgele renk ver
        for (int i = 0; i < kareler.Length; i++)
            kareler[i].RenkVer(Random.Range(0, 4));

        // Hedef rengi zeminden seç: böylece o renk mutlaka zeminde vardır
        hedefRenk = kareler[Random.Range(0, kareler.Length)].renkNo;
        renkEkrani.color = Renk(hedefRenk);
        ekranYazisi.text = RenkAdi(hedefRenk);
        ekranSekli.sprite = sekiller[hedefRenk];
        ekranSekli.enabled = PlayerPrefs.GetInt("renkKoru", 0) == 1;
        SesCal(turSesi);

        sayac = turSuresi;
        turSuruyor = true;
    }

    void Update()
    {
        if (turSuruyor)
        {
            sayac = sayac - Time.deltaTime;
            bilgiYazisi.text = "Tur " + tur + "/" + toplamTur + "   Can: " + can + "   Süre: " + sayac.ToString("0.0");
            if (sayac <= 0f) TurBitti();
        }
        else if (bekleniyor)
        {
            sayac = sayac - Time.deltaTime;
            if (sayac <= 0f) Sonraki();
        }
    }

    void TurBitti()
    {
        turSuruyor = false;

        // Hedef renkte olmayan kareler suya düşer
        for (int i = 0; i < kareler.Length; i++)
        {
            if (kareler[i].renkNo != hedefRenk) kareler[i].Dus();
        }

        // Oyuncu doğru renkte bir karenin üstünde mi?
        bool guvende = false;
        for (int i = 0; i < kareler.Length; i++)
        {
            if (kareler[i].renkNo == hedefRenk && kareler[i].UstundeMi(oyuncu.transform.position))
                guvende = true;
        }

        // KURAL
        if (!guvende)
        {
            can = can - 1;
            oyuncuDustu = true;
            oyuncu.SuyaDus();
            SesCal(dusmeSesi);
            Debug.Log("Suya düştün! Kalan can: " + can);
        }

        bilgiYazisi.text = "Tur " + tur + "/" + toplamTur + "   Can: " + can;
        sayac = beklemeSuresi;
        bekleniyor = true;
    }

    void Sonraki()
    {
        bekleniyor = false;

        // KAYBETME / KAZANMA koşulları
        if (can <= 0)
        {
            Bitir(false);
            return;
        }
        if (tur >= toplamTur)
        {
            Bitir(true);
            return;
        }

        if (oyuncuDustu)
        {
            oyuncu.YenidenDog(Vector3.zero);
            oyuncuDustu = false;
        }

        // Her tur biraz daha az süre (ama 1 saniyenin altına inmesin)
        turSuresi = Mathf.Max(1f, turSuresi - hizlanma);
        YeniTur();
    }

    void Bitir(bool kazandi)
    {
        oyuncu.aktif = false;
        sonucPaneli.SetActive(true);
        if (kazandi)
        {
            sonucYazisi.text = "KAZANDIN!\n" + toplamTur + " tur dayandın";
            SesCal(kazanmaSesi);
        }
        else
        {
            sonucYazisi.text = "SUYA DÜŞTÜN!\n" + tur + ". turda elendin";
            SesCal(kaybetmeSesi);
        }
        Debug.Log(sonucYazisi.text);
    }

    // --- Butonlar (Sonuç panelindeki) ---
    public void TekrarOyna()
    {
        SceneManager.LoadScene("Oyun");
    }

    public void MenuyeDon()
    {
        SceneManager.LoadScene("Menu");
    }

    // Ses ayarı açıksa sesi çal
    void SesCal(AudioClip klip)
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) hoparlor.PlayOneShot(klip);
    }

    // switch: renk numarasına göre renk seç (Sekme/TopUretici'deki gibi)
    public static Color Renk(int no)
    {
        Color renk;
        switch (no)
        {
            case 0: renk = new Color(0.9f, 0.2f, 0.2f); break;    // kırmızı
            case 1: renk = new Color(0.25f, 0.8f, 0.3f); break;   // yeşil
            case 2: renk = new Color(0.95f, 0.85f, 0.2f); break;  // sarı
            default: renk = new Color(0.6f, 0.3f, 0.85f); break;  // mor
        }

        // Menüdeki Renk ayarı: 0 soluk, 1 normal, 2 canlı
        // Rengi HSV'ye çevir: H = renk tonu, S = doygunluk (canlılık), V = parlaklık
        float h, s, v;
        Color.RGBToHSV(renk, out h, out s, out v);
        int canlilik = PlayerPrefs.GetInt("canlilik", 1);
        if (canlilik == 0) s = s * 0.45f;     // soluk: doygunluğu azalt
        if (canlilik == 2)                    // canlı: tam doygun ve parlak
        {
            s = 1f;
            v = 1f;
        }
        return Color.HSVToRGB(h, s, v);
    }

    public static string RenkAdi(int no)
    {
        switch (no)
        {
            case 0: return "KIRMIZI!";
            case 1: return "YEŞİL!";
            case 2: return "SARI!";
            default: return "MOR!";
        }
    }
}
