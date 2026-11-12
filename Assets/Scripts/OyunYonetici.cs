using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Oyunun beyni: zemini kurar, turları yönetir, oyunun bitişine karar verir.
// KURAL: Süre bitince ekrandaki renkte olmayan kareler suya düşer.
//        Doğru renkte değilsen sen de düşersin ve 1 can gider.
//        Her dayandığın tur 1 puan. Canın biterse oyun biter; amaç rekoru kırmak.
public class OyunYonetici : MonoBehaviour
{
    [Header("Zemin")]
    [SerializeField] Kare karePrefab;
    [SerializeField] int kenarAdet = 4;        // 4x4 = 16 kare
    [SerializeField] float aralik = 1.5f;      // kareler arası mesafe

    [Header("Oyun ayarları")]
    [SerializeField] float kolaySure = 4f;     // zorluğa göre ilk tur süresi
    [SerializeField] float ortaSure = 3f;
    [SerializeField] float zorSure = 2f;
    [SerializeField] float hizlanma = 0.1f;    // her tur süre bu kadar kısalır
    [SerializeField] float enAzSure = 1f;      // süre bunun altına inmez
    [SerializeField] int can = 3;
    [SerializeField] float beklemeSuresi = 1.5f;   // kareler düştükten sonra bekleme

    [Header("Bağlantılar")]
    [SerializeField] Oyuncu oyuncu;
    [SerializeField] SpriteRenderer renkEkrani;    // arenanın üstündeki ekran
    [SerializeField] TMP_Text ekranYazisi;         // "YEŞİL!"
    [SerializeField] TMP_Text bilgiYazisi;         // skor, can, süre
    [SerializeField] TMP_Text rekorYazisi;         // en altta: "Rekor (Orta): 7"
    [SerializeField] GameObject sonucPaneli;
    [SerializeField] TMP_Text sonucYazisi;
    [SerializeField] GameObject duraklatPaneli;

    [Header("Sesler")]
    [SerializeField] AudioSource hoparlor;
    [SerializeField] AudioClip turSesi;
    [SerializeField] AudioClip dusmeSesi;
    [SerializeField] AudioClip rekorSesi;
    [SerializeField] AudioClip kaybetmeSesi;

    Kare[] kareler;
    int hedefRenk;
    int skor = 0;           // dayanılan tur sayısı
    int rekor;
    int zorluk;
    float turSuresi;
    float sayac;            // geri sayım
    bool turSuruyor = false;
    bool bekleniyor = false;
    bool oyuncuDustu = false;

    void Start()
    {
        Time.timeScale = 1f;
        sonucPaneli.SetActive(false);
        duraklatPaneli.SetActive(false);

        // Menüdeki Ayarlar'dan seçilen zorluk ilk tur süresini belirler
        zorluk = PlayerPrefs.GetInt("zorluk", 1);
        switch (zorluk)
        {
            case 0: turSuresi = kolaySure; break;
            case 1: turSuresi = ortaSure; break;
            default: turSuresi = zorSure; break;
        }

        // Her zorluğun rekoru ayrı saklanır: "rekor0", "rekor1", "rekor2"
        rekor = PlayerPrefs.GetInt("rekor" + zorluk, 0);
        rekorYazisi.text = "Rekor (" + ZorlukAdi(zorluk) + "): " + rekor;

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
        // Her kareye rastgele renk ver
        for (int i = 0; i < kareler.Length; i++)
            kareler[i].RenkVer(Random.Range(0, 4));

        // Hedef rengi zeminden seç: böylece o renk mutlaka zeminde vardır
        hedefRenk = kareler[Random.Range(0, kareler.Length)].renkNo;
        renkEkrani.color = Renk(hedefRenk);
        ekranYazisi.text = RenkAdi(hedefRenk);

        sayac = turSuresi;
        turSuruyor = true;
        SesCal(turSesi);
    }

    void Update()
    {
        if (turSuruyor)
        {
            sayac = sayac - Time.deltaTime;
            bilgiYazisi.text = "Skor: " + skor + "   Can: " + can + "   Süre: " + sayac.ToString("0.0");
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
        if (guvende)
        {
            skor = skor + 1;
        }
        else
        {
            can = can - 1;
            oyuncuDustu = true;
            oyuncu.SuyaDus();
            SesCal(dusmeSesi);
            Debug.Log("Suya düştün! Kalan can: " + can);
        }

        bilgiYazisi.text = "Skor: " + skor + "   Can: " + can;
        sayac = beklemeSuresi;
        bekleniyor = true;
    }

    void Sonraki()
    {
        bekleniyor = false;

        // KAYBETME koşulu
        if (can <= 0)
        {
            Bitir();
            return;
        }

        if (oyuncuDustu)
        {
            oyuncu.YenidenDog(Vector3.zero);
            oyuncuDustu = false;
        }

        // Her tur biraz daha az süre (ama enAzSure'nin altına inmesin)
        turSuresi = Mathf.Max(enAzSure, turSuresi - hizlanma);
        YeniTur();
    }

    void Bitir()
    {
        oyuncu.aktif = false;
        sonucPaneli.SetActive(true);

        if (skor > rekor)
        {
            rekor = skor;
            PlayerPrefs.SetInt("rekor" + zorluk, rekor);   // yeni rekoru kaydet
            sonucYazisi.text = "YENİ REKOR!\nSkor: " + skor;
            SesCal(rekorSesi);
        }
        else
        {
            sonucYazisi.text = "SUYA DÜŞTÜN!\nSkor: " + skor + "\nRekor: " + rekor;
            SesCal(kaybetmeSesi);
        }
        rekorYazisi.text = "Rekor (" + ZorlukAdi(zorluk) + "): " + rekor;
        Debug.Log(sonucYazisi.text);
    }

    // --- Butonlar ---
    public void Duraklat()
    {
        if (sonucPaneli.activeSelf) return;   // oyun bittiyse duraklatma
        Time.timeScale = 0f;                  // zaman durur: deltaTime 0 olur
        duraklatPaneli.SetActive(true);
    }

    public void DevamEt()
    {
        Time.timeScale = 1f;
        duraklatPaneli.SetActive(false);
    }

    public void TekrarOyna()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Oyun");
    }

    public void MenuyeDon()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // Ses ayarı açıksa sesi çal
    void SesCal(AudioClip klip)
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) hoparlor.PlayOneShot(klip);
    }

    public static string ZorlukAdi(int zorluk)
    {
        switch (zorluk)
        {
            case 0: return "Kolay";
            case 1: return "Orta";
            default: return "Zor";
        }
    }

    // switch: renk numarasına göre renk seç (Sekme/TopUretici'deki gibi)
    public static Color Renk(int no)
    {
        switch (no)
        {
            case 0: return new Color(0.9f, 0.2f, 0.2f);    // kırmızı
            case 1: return new Color(0.25f, 0.8f, 0.3f);   // yeşil
            case 2: return new Color(0.95f, 0.85f, 0.2f);  // sarı
            default: return new Color(0.6f, 0.3f, 0.85f);  // mor
        }
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
