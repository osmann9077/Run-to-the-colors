using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// 2. ekran: ana menü (Oyna / Ayarlar / Renk)
// Ayarlar: ses açık-kapalı, zorluk (Kolay / Orta / Zor)
// Renk: canlılık (Soluk / Normal / Canlı), renk körü modu (karelerde şekil)
// Seçimler PlayerPrefs ile kaydedilir; Oyun sahnesi açılınca oradan okunur.
public class MenuYonetici : MonoBehaviour
{
    [Header("Paneller")]
    [SerializeField] GameObject anaPanel;
    [SerializeField] GameObject ayarlarPaneli;
    [SerializeField] GameObject renkPaneli;

    [Header("Yazılar")]
    [SerializeField] TMP_Text sesYazisi;        // "Ses: Açık"
    [SerializeField] TMP_Text zorlukYazisi;     // "Zorluk: Orta"
    [SerializeField] TMP_Text canlilikYazisi;   // "Canlılık: Normal"
    [SerializeField] TMP_Text renkKoruYazisi;   // "Renk körü modu: Kapalı"

    [Header("Renk önizlemesi")]
    [SerializeField] Image[] ornekKareler;      // 4 küçük kare: ayarın etkisini gösterir
    [SerializeField] Image[] ornekSekiller;     // karelerin üstündeki şekiller

    [Header("Ses")]
    [SerializeField] AudioSource hoparlor;
    [SerializeField] AudioClip tikSesi;

    void Start()
    {
        PaneliAc(anaPanel);
        YazilariGuncelle();
    }

    // --- Ana menü butonları ---
    public void Oyna()
    {
        Tikla();
        SceneManager.LoadScene("Oyun");
    }

    public void AyarlariAc()
    {
        Tikla();
        PaneliAc(ayarlarPaneli);
    }

    public void RenkAc()
    {
        Tikla();
        PaneliAc(renkPaneli);
    }

    public void GeriDon()
    {
        Tikla();
        PaneliAc(anaPanel);
    }

    // --- Ayarlar ---
    public void SesDegistir()
    {
        int ses = PlayerPrefs.GetInt("ses", 1);
        if (ses == 1) PlayerPrefs.SetInt("ses", 0);
        else PlayerPrefs.SetInt("ses", 1);
        Tikla();
        YazilariGuncelle();
    }

    // 0 = Kolay, 1 = Orta, 2 = Zor
    public void ZorlukSec(int zorluk)
    {
        PlayerPrefs.SetInt("zorluk", zorluk);
        Tikla();
        YazilariGuncelle();
    }

    // --- Renk ---
    // 0 = Soluk, 1 = Normal, 2 = Canlı
    public void CanlilikSec(int canlilik)
    {
        PlayerPrefs.SetInt("canlilik", canlilik);
        Tikla();
        YazilariGuncelle();
    }

    public void RenkKoruDegistir()
    {
        int acik = PlayerPrefs.GetInt("renkKoru", 0);
        if (acik == 1) PlayerPrefs.SetInt("renkKoru", 0);
        else PlayerPrefs.SetInt("renkKoru", 1);
        Tikla();
        YazilariGuncelle();
    }

    // --- Yardımcılar ---

    // Sadece istenen paneli göster, diğerlerini gizle
    void PaneliAc(GameObject panel)
    {
        anaPanel.SetActive(panel == anaPanel);
        ayarlarPaneli.SetActive(panel == ayarlarPaneli);
        renkPaneli.SetActive(panel == renkPaneli);
    }

    void YazilariGuncelle()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) sesYazisi.text = "Ses: Açık";
        else sesYazisi.text = "Ses: Kapalı";

        switch (PlayerPrefs.GetInt("zorluk", 1))
        {
            case 0: zorlukYazisi.text = "Zorluk: Kolay"; break;
            case 1: zorlukYazisi.text = "Zorluk: Orta"; break;
            default: zorlukYazisi.text = "Zorluk: Zor"; break;
        }

        switch (PlayerPrefs.GetInt("canlilik", 1))
        {
            case 0: canlilikYazisi.text = "Canlılık: Soluk"; break;
            case 1: canlilikYazisi.text = "Canlılık: Normal"; break;
            default: canlilikYazisi.text = "Canlılık: Canlı"; break;
        }

        bool renkKoru = PlayerPrefs.GetInt("renkKoru", 0) == 1;
        if (renkKoru) renkKoruYazisi.text = "Renk körü modu: Açık";
        else renkKoruYazisi.text = "Renk körü modu: Kapalı";

        // Önizleme: 4 rengi şu anki ayarla boya, şekilleri göster/gizle
        for (int i = 0; i < ornekKareler.Length; i++)
        {
            ornekKareler[i].color = OyunYonetici.Renk(i);
            ornekSekiller[i].enabled = renkKoru;
        }
    }

    void Tikla()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) hoparlor.PlayOneShot(tikSesi);
    }
}
