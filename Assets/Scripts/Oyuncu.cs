using UnityEngine;
using UnityEngine.InputSystem;

// Kağıt adam: WASD ya da joystick ile arenada koşar. (Hareket.cs'den geliştirildi)
public class Oyuncu : MonoBehaviour
{
    [SerializeField] float hiz = 4f;          // birim / saniye
    [SerializeField] float sinir = 2.9f;      // arenanın yarı genişliği
    [SerializeField] SanalJoystick joystick;      // telefondaki sol alt joystick

    public bool aktif = true;                 // false ise hareket etmez
    Vector3 ilkBoyut;
    bool dusuyor = false;
    float adim = 0f;      // koşma animasyonunun zamanlayıcısı

    void Awake()
    {
        ilkBoyut = transform.localScale;
    }

    void Update()
    {
        // Suya düşüyorsa küçülerek kaybolsun
        if (dusuyor)
        {
            transform.localScale -= ilkBoyut * 2f * Time.deltaTime;
            if (transform.localScale.x < 0f) transform.localScale = Vector3.zero;
            return;
        }

        if (!aktif) return;

        // 1) Klavye: W A S D
        Vector2 yon = Vector2.zero;
        var klavye = Keyboard.current;
        if (klavye != null)
        {
            if (klavye.wKey.isPressed) yon.y = yon.y + 1;
            if (klavye.sKey.isPressed) yon.y = yon.y - 1;
            if (klavye.dKey.isPressed) yon.x = yon.x + 1;
            if (klavye.aKey.isPressed) yon.x = yon.x - 1;
        }

        // 2) Joystick kullanılıyorsa onun yönü geçerli
        if (joystick != null && joystick.yon != Vector2.zero) yon = joystick.yon;

        // Çapraz giderken daha hızlı gitmesin
        if (yon.magnitude > 1f) yon = yon.normalized;

        // Hareket (Time.deltaTime: her bilgisayarda aynı hız)
        Vector3 p = transform.position + (Vector3)(yon * hiz * Time.deltaTime);

        // Arenanın dışına çıkamasın
        p.x = Mathf.Clamp(p.x, -sinir, sinir);
        p.y = Mathf.Clamp(p.y, -sinir, sinir);
        transform.position = p;

        // Koşarken canlansın: gittiği yöne eğilsin, adım attıkça hafifçe uzayıp kısalsın
        if (yon != Vector2.zero)
        {
            adim = adim + Time.deltaTime * 18f;
            transform.rotation = Quaternion.Euler(0f, 0f, -yon.x * 12f);
            float esneme = 1f + Mathf.Sin(adim) * 0.06f;
            transform.localScale = new Vector3(ilkBoyut.x, ilkBoyut.y * esneme, 1f);
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.localScale = ilkBoyut;
        }
    }

    public void SuyaDus()
    {
        transform.rotation = Quaternion.identity;
        transform.localScale = ilkBoyut;
        dusuyor = true;
        aktif = false;
    }

    // Suya düştükten sonra arenanın ortasında yeniden doğar
    public void YenidenDog(Vector3 konum)
    {
        dusuyor = false;
        transform.localScale = ilkBoyut;
        transform.position = konum;
        aktif = true;
    }
}
