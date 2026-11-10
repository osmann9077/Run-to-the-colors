using UnityEngine;
using UnityEngine.InputSystem;

// Ekranın sol altındaki sanal joystick.
// Kurulum: Canvas içinde "Taban" (büyük daire) ve onun içinde "Topuz" (küçük daire) Image'ları.
public class SanalJoystick : MonoBehaviour
{
    [SerializeField] RectTransform taban;   // büyük daire
    [SerializeField] RectTransform topuz;   // parmakla sürüklenen küçük daire

    public Vector2 yon;     // Oyuncu.cs bunu okur: (-1..1, -1..1)
    bool tutuluyor = false;

    void Update()
    {
        // Pointer: fare ve dokunmatik ekranın ortak adı (Merhaba.cs)
        var isaret = Pointer.current;
        if (isaret == null) return;

        Vector2 ekran = isaret.position.ReadValue();

        // Sadece ekranın sol alt çeyreğine dokunulursa joystick başlar
        if (isaret.press.wasPressedThisFrame && ekran.x < Screen.width / 2f && ekran.y < Screen.height / 2f)
            tutuluyor = true;

        if (isaret.press.wasReleasedThisFrame)
            tutuluyor = false;

        if (tutuluyor)
        {
            // Tabanın ekrandaki yarıçapı (piksel)
            float yaricap = taban.rect.width / 2f * taban.lossyScale.x;

            Vector2 fark = ekran - (Vector2)taban.position;
            fark = Vector2.ClampMagnitude(fark, yaricap);   // daireden dışarı taşmasın

            topuz.position = (Vector2)taban.position + fark;
            yon = fark / yaricap;
        }
        else
        {
            topuz.position = taban.position;   // bırakınca ortaya dön
            yon = Vector2.zero;
        }
    }
}
