using UnityEngine;
using UnityEngine.InputSystem;

public class SanalJoystick : MonoBehaviour
{
    [SerializeField] RectTransform taban;
    [SerializeField] RectTransform topuz;

    public Vector2 yon;
    bool tutuluyor = false;

    void Update()
    {
        var isaret = Pointer.current;
        if (isaret == null) return;

        Vector2 ekran = isaret.position.ReadValue();

        if (isaret.press.wasPressedThisFrame && ekran.x < Screen.width / 2f && ekran.y < Screen.height / 2f)
            tutuluyor = true;

        if (isaret.press.wasReleasedThisFrame)
            tutuluyor = false;

        if (tutuluyor)
        {
            float yaricap = taban.rect.width / 2f * taban.lossyScale.x;

            Vector2 fark = ekran - (Vector2)taban.position;
            fark = Vector2.ClampMagnitude(fark, yaricap);

            topuz.position = (Vector2)taban.position + fark;
            yon = fark / yaricap;
        }
        else
        {
            topuz.position = taban.position;
            yon = Vector2.zero;
        }
    }
}
