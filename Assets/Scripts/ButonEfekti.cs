using UnityEngine;
using UnityEngine.EventSystems;

// Butona basınca hafifçe küçülür, bırakınca eski boyutuna döner.
// IPointerDownHandler / IPointerUpHandler: Unity'nin arayüz sistemi
// butona basıldığında ve bırakıldığında bu metotları kendisi çağırır.
public class ButonEfekti : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float basiliBoyut = 0.9f;   // basılıyken boyut oranı
    [SerializeField] float hiz = 15f;            // boyut değişim hızı

    Vector3 hedefBoyut = Vector3.one;

    public void OnPointerDown(PointerEventData veri)
    {
        hedefBoyut = Vector3.one * basiliBoyut;
    }

    public void OnPointerUp(PointerEventData veri)
    {
        hedefBoyut = Vector3.one;
    }

    void Update()
    {
        // Lerp: mevcut boyuttan hedefe doğru yumuşakça yaklaş.
        // unscaledDeltaTime: oyun duraklatılmışken (timeScale = 0) de çalışsın
        transform.localScale = Vector3.Lerp(transform.localScale, hedefBoyut, hiz * Time.unscaledDeltaTime);
    }
}
