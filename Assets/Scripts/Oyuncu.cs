using UnityEngine;
using UnityEngine.InputSystem;

public class Oyuncu : MonoBehaviour
{
    [SerializeField] float hiz = 4f;
    [SerializeField] float sinir = 2.9f;
    [SerializeField] SanalJoystick joystick;

    public bool aktif = true;
    Vector3 ilkBoyut;
    bool dusuyor = false;

    void Awake()
    {
        ilkBoyut = transform.localScale;
    }

    void Update()
    {
        if (dusuyor)
        {
            transform.localScale -= ilkBoyut * 2f * Time.deltaTime;
            if (transform.localScale.x < 0f) transform.localScale = Vector3.zero;
            return;
        }

        if (!aktif) return;

        Vector2 yon = Vector2.zero;
        var klavye = Keyboard.current;
        if (klavye != null)
        {
            if (klavye.wKey.isPressed) yon.y = yon.y + 1;
            if (klavye.sKey.isPressed) yon.y = yon.y - 1;
            if (klavye.dKey.isPressed) yon.x = yon.x + 1;
            if (klavye.aKey.isPressed) yon.x = yon.x - 1;
        }

        if (joystick != null && joystick.yon != Vector2.zero) yon = joystick.yon;

        if (yon.magnitude > 1f) yon = yon.normalized;

        Vector3 p = transform.position + (Vector3)(yon * hiz * Time.deltaTime);

        p.x = Mathf.Clamp(p.x, -sinir, sinir);
        p.y = Mathf.Clamp(p.y, -sinir, sinir);
        transform.position = p;
    }

    public void SuyaDus()
    {
        dusuyor = true;
        aktif = false;
    }

    public void YenidenDog(Vector3 konum)
    {
        dusuyor = false;
        transform.localScale = ilkBoyut;
        transform.position = konum;
        aktif = true;
    }
}
