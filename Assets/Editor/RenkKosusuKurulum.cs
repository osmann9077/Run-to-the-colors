using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using TMPro;

// SADECE EDİTÖR ARACI (oyunun parçası değil).
// Üst menü: "Renk Koşusu > Her Şeyi Kur" → sahneleri, prefabları ve ayarları otomatik kurar.
// Kaynak: Claude (yapay zekâ) ile yazıldı. Kurulum bittikten sonra silinebilir.
public static class RenkKosusuKurulum
{
    const string SAHNE = "Assets/Scenes/";
    static readonly Color SU = new Color(0.16f, 0.5f, 0.83f);
    static readonly Vector2 ORTA = new Vector2(0.5f, 0.5f);
    static Sprite kare, daire, adam;

    [MenuItem("Renk Koşusu/Her Şeyi Kur")]
    static void HerSeyiKur()
    {
        if (Resources.Load<TMP_Settings>("TMP Settings") == null)
        {
            EditorUtility.DisplayDialog("Önce TextMeshPro",
                "Önce Window > TextMeshPro > Import TMP Essential Resources ile içe aktar, sonra bu menüyü tekrar çalıştır.", "Tamam");
            return;
        }
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        System.IO.Directory.CreateDirectory("Assets/Scenes");
        System.IO.Directory.CreateDirectory("Assets/Prefabs");
        AssetDatabase.Refresh();

        kare = SpriteHazirla("Assets/Sprites/Kare.png", 64, false);
        daire = SpriteHazirla("Assets/Sprites/Daire.png", 256, false);
        adam = SpriteHazirla("Assets/Sprites/KagitAdam.png", 256, true);

        SplashKur();
        MenuKur();
        OyunKur();
        DeneyMerhabaKur();
        DeneyHareketKur();
        DeneySekmeKur();

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(SAHNE + "Splash.unity", true),
            new EditorBuildSettingsScene(SAHNE + "Menu.unity", true),
            new EditorBuildSettingsScene(SAHNE + "Oyun.unity", true),
        };
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;

        AssetDatabase.SaveAssets();
        EditorSceneManager.OpenScene(SAHNE + "Splash.unity");
        EditorUtility.DisplayDialog("Hazır!",
            "Sahneler kuruldu. Splash sahnesi açık, Play'e bas.\nGame penceresinde çözünürlüğü 1080x1920 Portrait yap.", "Tamam");
    }

    // ---------------- SAHNELER ----------------

    static void SplashKur()
    {
        var s = YeniSahne(5f, SU);
        var c = CanvasKur();
        Resim("KagitAdam", c, adam, Color.white, ORTA, new Vector2(0, 100), new Vector2(500, 500));
        Yazi("Baslik", c, "RENK KOŞUSU", 130, ORTA, new Vector2(0, 550), new Vector2(1000, 200), Color.white);
        Yazi("AltYazi", c, "dokun ve başla", 55, ORTA, new Vector2(0, -350), new Vector2(900, 100), Color.white);
        new GameObject("Splash").AddComponent<Splash>();
        EditorSceneManager.SaveScene(s, SAHNE + "Splash.unity");
    }

    static void MenuKur()
    {
        var s = YeniSahne(5f, SU);
        var c = CanvasKur();

        var ana = Panel("AnaPanel", c, new Color(0, 0, 0, 0));
        Yazi("Baslik", ana.transform, "RENK KOŞUSU", 120, ORTA, new Vector2(0, 650), new Vector2(1000, 200), Color.white);
        Resim("KagitAdam", ana.transform, adam, Color.white, ORTA, new Vector2(0, 330), new Vector2(320, 320));
        var basla = Buton("Basla", ana.transform, "Başla", new Vector2(0, 0));
        var ayarlar = Buton("Ayarlar", ana.transform, "Ayarlar", new Vector2(0, -200));
        var cikis = Buton("Cikis", ana.transform, "Çıkış", new Vector2(0, -400));

        var ayarP = Panel("AyarlarPaneli", c, new Color(0, 0, 0, 0.6f));
        Yazi("Baslik", ayarP.transform, "AYARLAR", 110, ORTA, new Vector2(0, 650), new Vector2(1000, 200), Color.white);
        var zorlukYazi = Yazi("ZorlukYazisi", ayarP.transform, "Zorluk: Orta", 70, ORTA, new Vector2(0, 380), new Vector2(1000, 150), Color.white);
        var kolay = Buton("Kolay", ayarP.transform, "Kolay", new Vector2(0, 180));
        var orta = Buton("Orta", ayarP.transform, "Orta", new Vector2(0, 0));
        var zor = Buton("Zor", ayarP.transform, "Zor", new Vector2(0, -180));
        var geri = Buton("Geri", ayarP.transform, "Geri", new Vector2(0, -480));

        var m = new GameObject("MenuYonetici").AddComponent<MenuYonetici>();
        Bagla(m, "anaPanel", ana);
        Bagla(m, "ayarlarPaneli", ayarP);
        Bagla(m, "zorlukYazisi", zorlukYazi);

        UnityEventTools.AddPersistentListener(basla.onClick, m.Basla);
        UnityEventTools.AddPersistentListener(ayarlar.onClick, m.AyarlariAc);
        UnityEventTools.AddPersistentListener(cikis.onClick, m.Cikis);
        UnityEventTools.AddPersistentListener(geri.onClick, m.AyarlariKapat);
        UnityEventTools.AddIntPersistentListener(kolay.onClick, m.ZorlukSec, 0);
        UnityEventTools.AddIntPersistentListener(orta.onClick, m.ZorlukSec, 1);
        UnityEventTools.AddIntPersistentListener(zor.onClick, m.ZorlukSec, 2);

        EditorSceneManager.SaveScene(s, SAHNE + "Menu.unity");
    }

    static void OyunKur()
    {
        var s = YeniSahne(7f, SU);   // arka plan mavi = su

        // Kare prefabı
        var kg = new GameObject("Kare");
        kg.AddComponent<SpriteRenderer>().sprite = kare;
        kg.AddComponent<BoxCollider2D>().size = Vector2.one;
        kg.AddComponent<Kare>();
        kg.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
        var karePrefab = PrefabUtility.SaveAsPrefabAsset(kg, "Assets/Prefabs/Kare.prefab");
        Object.DestroyImmediate(kg);

        // Oyuncu (kağıt adam)
        var og = new GameObject("Oyuncu");
        var osr = og.AddComponent<SpriteRenderer>();
        osr.sprite = adam;
        osr.sortingOrder = 5;
        og.transform.localScale = new Vector3(1.6f, 1.6f, 1f);
        var oyuncu = og.AddComponent<Oyuncu>();

        // Arenadaki renk ekranı
        var cerceve = new GameObject("EkranCercevesi");
        var csr = cerceve.AddComponent<SpriteRenderer>();
        csr.sprite = kare;
        csr.color = new Color(0.15f, 0.15f, 0.15f);
        csr.sortingOrder = -1;
        cerceve.transform.position = new Vector3(0, 4.7f, 0);
        cerceve.transform.localScale = new Vector3(5.9f, 2.2f, 1f);

        var eg = new GameObject("RenkEkrani");
        var esr = eg.AddComponent<SpriteRenderer>();
        esr.sprite = kare;
        esr.color = Color.green;
        eg.transform.position = new Vector3(0, 4.7f, 0);
        eg.transform.localScale = new Vector3(5.5f, 1.8f, 1f);

        var tg = new GameObject("EkranYazisi");
        var tmp = tg.AddComponent<TextMeshPro>();
        tg.transform.position = new Vector3(0, 4.7f, 0);
        tmp.rectTransform.sizeDelta = new Vector2(5.3f, 1.6f);
        tmp.text = "YEŞİL!";
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 3;
        tmp.fontSizeMax = 10;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.black;
        tg.GetComponent<Renderer>().sortingOrder = 2;

        // Arayüz
        var c = CanvasKur();
        var bilgi = Yazi("BilgiYazisi", c, "Tur 1/10   Can: 3", 55, new Vector2(0.5f, 1f), new Vector2(0, -90), new Vector2(1000, 120), Color.white);

        var taban = Resim("JoystickTaban", c, daire, new Color(1, 1, 1, 0.35f), Vector2.zero, new Vector2(230, 230), new Vector2(320, 320));
        var topuz = Resim("JoystickTopuz", taban.transform, daire, new Color(1, 1, 1, 0.8f), ORTA, Vector2.zero, new Vector2(140, 140));
        var joy = taban.gameObject.AddComponent<SanalJoystick>();
        Bagla(joy, "taban", taban.rectTransform);
        Bagla(joy, "topuz", topuz.rectTransform);

        var sonuc = Panel("SonucPaneli", c, new Color(0, 0, 0, 0.7f));
        var sonucYazi = Yazi("SonucYazisi", sonuc.transform, "KAZANDIN!", 100, ORTA, new Vector2(0, 250), new Vector2(1000, 400), Color.white);
        var tekrar = Buton("Tekrar", sonuc.transform, "Tekrar Oyna", new Vector2(0, -100));
        var menu = Buton("Menu", sonuc.transform, "Menü", new Vector2(0, -300));

        // Oyun yöneticisi ve bağlantılar
        var y = new GameObject("OyunYonetici").AddComponent<OyunYonetici>();
        Bagla(y, "karePrefab", karePrefab.GetComponent<Kare>());
        Bagla(y, "oyuncu", oyuncu);
        Bagla(y, "renkEkrani", esr);
        Bagla(y, "ekranYazisi", tmp);
        Bagla(y, "bilgiYazisi", bilgi);
        Bagla(y, "sonucPaneli", sonuc);
        Bagla(y, "sonucYazisi", sonucYazi);
        Bagla(oyuncu, "joystick", joy);

        UnityEventTools.AddPersistentListener(tekrar.onClick, y.TekrarOyna);
        UnityEventTools.AddPersistentListener(menu.onClick, y.MenuyeDon);

        EditorSceneManager.SaveScene(s, SAHNE + "Oyun.unity");
    }

    // A ve B aşaması için ders betiklerinin deney sahneleri
    static void DeneyMerhabaKur()
    {
        var s = YeniSahne(5f, new Color(0.2f, 0.2f, 0.25f));
        var g = new GameObject("Kare");
        g.AddComponent<SpriteRenderer>().sprite = kare;
        g.AddComponent<BoxCollider2D>().size = Vector2.one;
        g.transform.localScale = new Vector3(2f, 2f, 1f);
        g.AddComponent<Merhaba>();
        EditorSceneManager.SaveScene(s, SAHNE + "Deney_Merhaba.unity");
    }

    static void DeneyHareketKur()
    {
        var s = YeniSahne(5f, new Color(0.2f, 0.2f, 0.25f));
        var g = new GameObject("Pierrot");
        g.AddComponent<SpriteRenderer>().sprite = kare;
        g.AddComponent<Hareket>();
        EditorSceneManager.SaveScene(s, SAHNE + "Deney_Hareket.unity");
    }

    static void DeneySekmeKur()
    {
        var s = YeniSahne(5f, new Color(0.2f, 0.2f, 0.25f));
        var tg = new GameObject("Top");
        tg.AddComponent<SpriteRenderer>().sprite = daire;
        tg.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        tg.AddComponent<Sekme>();
        var topPrefab = PrefabUtility.SaveAsPrefabAsset(tg, "Assets/Prefabs/Top.prefab");
        Object.DestroyImmediate(tg);

        var u = new GameObject("TopUretici").AddComponent<TopUretici>();
        Bagla(u, "topPrefab", topPrefab.GetComponent<Sekme>());
        EditorSceneManager.SaveScene(s, SAHNE + "Deney_Sekme.unity");
    }

    // ---------------- YARDIMCILAR ----------------

    static Sprite SpriteHazirla(string yol, int ppu, bool altPivot)
    {
        var imp = (TextureImporter)AssetImporter.GetAtPath(yol);
        imp.textureType = TextureImporterType.Sprite;
        imp.spriteImportMode = SpriteImportMode.Single;
        imp.spritePixelsPerUnit = ppu;
        var ayar = new TextureImporterSettings();
        imp.ReadTextureSettings(ayar);
        ayar.spriteAlignment = altPivot ? (int)SpriteAlignment.BottomCenter : (int)SpriteAlignment.Center;
        imp.SetTextureSettings(ayar);
        imp.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(yol);
    }

    static Scene YeniSahne(float boyut, Color arka)
    {
        var s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var kg = new GameObject("Main Camera");
        kg.tag = "MainCamera";
        var k = kg.AddComponent<Camera>();
        k.orthographic = true;
        k.orthographicSize = boyut;
        k.clearFlags = CameraClearFlags.SolidColor;
        k.backgroundColor = arka;
        kg.AddComponent<AudioListener>();
        kg.transform.position = new Vector3(0, 0, -10);
        return s;
    }

    static Transform CanvasKur()
    {
        var go = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        go.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        var sc = go.GetComponent<CanvasScaler>();
        sc.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        sc.referenceResolution = new Vector2(1080, 1920);
        sc.matchWidthOrHeight = 0.5f;
        new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
        return go.transform;
    }

    static RectTransform UIObje(string ad, Transform ebeveyn, Vector2 anchor, Vector2 konum, Vector2 boyut)
    {
        var go = new GameObject(ad, typeof(RectTransform));
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(ebeveyn, false);
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = ORTA;
        rt.anchoredPosition = konum;
        rt.sizeDelta = boyut;
        return rt;
    }

    static TextMeshProUGUI Yazi(string ad, Transform eb, string metin, float punto, Vector2 anchor, Vector2 konum, Vector2 boyut, Color renk)
    {
        var t = UIObje(ad, eb, anchor, konum, boyut).gameObject.AddComponent<TextMeshProUGUI>();
        t.text = metin;
        t.fontSize = punto;
        t.alignment = TextAlignmentOptions.Center;
        t.color = renk;
        t.raycastTarget = false;
        return t;
    }

    static Image Resim(string ad, Transform eb, Sprite sp, Color renk, Vector2 anchor, Vector2 konum, Vector2 boyut)
    {
        var img = UIObje(ad, eb, anchor, konum, boyut).gameObject.AddComponent<Image>();
        img.sprite = sp;
        img.color = renk;
        img.preserveAspect = true;
        return img;
    }

    static Button Buton(string ad, Transform eb, string metin, Vector2 konum)
    {
        var img = Resim(ad, eb, kare, new Color(1f, 0.95f, 0.85f), ORTA, konum, new Vector2(600, 140));
        img.preserveAspect = false;
        var b = img.gameObject.AddComponent<Button>();
        Yazi("Yazi", img.transform, metin, 60, ORTA, Vector2.zero, new Vector2(600, 140), Color.black);
        return b;
    }

    static GameObject Panel(string ad, Transform eb, Color renk)
    {
        var rt = UIObje(ad, eb, ORTA, Vector2.zero, Vector2.zero);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.sizeDelta = Vector2.zero;
        rt.gameObject.AddComponent<Image>().color = renk;
        return rt.gameObject;
    }

    // [SerializeField] private alanlara Inspector'daki gibi değer sürükler
    static void Bagla(Object hedef, string alan, Object deger)
    {
        var so = new SerializedObject(hedef);
        var p = so.FindProperty(alan);
        if (p == null)
        {
            Debug.LogError(hedef.name + " içinde '" + alan + "' alanı bulunamadı");
            return;
        }
        p.objectReferenceValue = deger;
        so.ApplyModifiedPropertiesWithoutUndo();
    }
}
