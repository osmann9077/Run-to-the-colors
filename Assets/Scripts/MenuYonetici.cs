using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuYonetici : MonoBehaviour
{
    [SerializeField] GameObject anaPanel;
    [SerializeField] GameObject ayarlarPaneli;
    [SerializeField] TMP_Text sesYazisi;
    [SerializeField] TMP_Text zorlukYazisi;
    [SerializeField] TMP_Text rekorYazisi;
    [SerializeField] AudioSource hoparlor;
    [SerializeField] AudioClip tikSesi;

    void Start()
    {
        GeriDon();
        YazilariGuncelle();
    }

    public void Oyna()
    {
        Tikla();
        SceneManager.LoadScene("Oyun");
    }

    public void AyarlariAc()
    {
        Tikla();
        anaPanel.SetActive(false);
        ayarlarPaneli.SetActive(true);
    }

    public void GeriDon()
    {
        Tikla();
        anaPanel.SetActive(true);
        ayarlarPaneli.SetActive(false);
    }

    public void SesDegistir()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) PlayerPrefs.SetInt("ses", 0);
        else PlayerPrefs.SetInt("ses", 1);
        Tikla();
        YazilariGuncelle();
    }

    public void ZorlukSec(int zorluk)
    {
        PlayerPrefs.SetInt("zorluk", zorluk);
        Tikla();
        YazilariGuncelle();
    }

    void YazilariGuncelle()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) sesYazisi.text = "Ses: Açık";
        else sesYazisi.text = "Ses: Kapalı";

        zorlukYazisi.text = "Zorluk: " + OyunYonetici.ZorlukAdi(PlayerPrefs.GetInt("zorluk", 1));

        rekorYazisi.text = "Rekor   Kolay: " + PlayerPrefs.GetInt("rekor0", 0)
                         + "   Orta: " + PlayerPrefs.GetInt("rekor1", 0)
                         + "   Zor: " + PlayerPrefs.GetInt("rekor2", 0);
    }

    void Tikla()
    {
        if (PlayerPrefs.GetInt("ses", 1) == 1) hoparlor.PlayOneShot(tikSesi);
    }
}
