using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// 2. ekran: Başla / Ayarlar / Çıkış
// Ayarlar: zorluk seçimi (Kolay / Orta / Zor). Seçim PlayerPrefs ile saklanır,
// Oyun sahnesi açılınca OyunYonetici bunu okur.
public class MenuYonetici : MonoBehaviour
{
    [SerializeField] GameObject anaPanel;       // Başla-Ayarlar-Çıkış butonları
    [SerializeField] GameObject ayarlarPaneli;  // ayarlar penceresi
    [SerializeField] TMP_Text zorlukYazisi;     // "Zorluk: Orta"

    void Start()
    {
        anaPanel.SetActive(true);
        ayarlarPaneli.SetActive(false);
        ZorlukYazisiniGuncelle();
    }

    public void Basla()
    {
        SceneManager.LoadScene("Oyun");
    }

    public void AyarlariAc()
    {
        anaPanel.SetActive(false);
        ayarlarPaneli.SetActive(true);
    }

    public void AyarlariKapat()
    {
        ayarlarPaneli.SetActive(false);
        anaPanel.SetActive(true);
    }

    public void Cikis()
    {
        Debug.Log("Oyundan çıkıldı");   // Editörde Quit çalışmaz, Console'da görürsün
        Application.Quit();
    }

    // 0 = Kolay, 1 = Orta, 2 = Zor
    public void ZorlukSec(int zorluk)
    {
        PlayerPrefs.SetInt("zorluk", zorluk);   // telefona/bilgisayara kaydet
        ZorlukYazisiniGuncelle();
    }

    void ZorlukYazisiniGuncelle()
    {
        int zorluk = PlayerPrefs.GetInt("zorluk", 1);   // kayıt yoksa Orta
        switch (zorluk)
        {
            case 0: zorlukYazisi.text = "Zorluk: Kolay"; break;
            case 1: zorlukYazisi.text = "Zorluk: Orta"; break;
            default: zorlukYazisi.text = "Zorluk: Zor"; break;
        }
    }
}
