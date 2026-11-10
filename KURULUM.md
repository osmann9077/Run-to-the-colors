# RENK KOŞUSU – Unity Kurulum Rehberi

**Oyunun kuralı (tek cümle):** Ekranda hangi renk çıkarsa süre bitmeden o renkteki kareye koş. Diğer kareler suya düşer, yanlış yerdeysen sen de düşersin.
**Kazanma/kaybetme:** `if (tur >= toplamTur)` kazandın, `if (can <= 0)` kaybettin.

> Unity'de **6 betik** var. Her birini Assets/Scripts klasörüne koy.
> Not: Kodlar Unity'de denenmeden yazıldı. Kırmızı hata çıkarsa ekran görüntüsünü at, düzeltelim.

---

## 0) Proje + Git (ilk iş)
1. Unity Hub → **New project** → **Universal 2D** → adı `RenkKosusu`.
2. Zip'teki `.gitignore` dosyasını proje klasörünün **köküne** koy (Assets klasörünün yanına).
3. Zip'teki `Assets/Scripts` ve `Assets/Sprites` klasörlerini projenin `Assets` klasörüne kopyala.
4. Proje klasöründe terminal aç:
   ```
   git init
   git add .
   git commit -m "Proje kuruldu, .gitignore eklendi"
   ```
   Sonra GitHub'da boş bir repo aç ve verdiği `git remote add origin ...` ve `git push -u origin main` komutlarını çalıştır.
5. **Edit → Project Settings → Player → Resolution and Presentation → Default Orientation: Portrait** (telefon dik).
6. Game penceresinin üstündeki çözünürlükten **1080x1920 Portrait** seç.

## 1) Kağıt adam görseli
`Assets/Sprites/KagitAdam.png`'e tıkla, Inspector'da:
- Texture Type: **Sprite (2D and UI)**
- Pixels Per Unit: **256**
- Pivot: **Bottom** (ayakları zemini kontrol eden nokta olsun diye)
- **Apply**

## 2) Üç sahne
`Assets/Scenes` içinde 3 sahne oluştur (sağ tık → Create → Scene): **Splash**, **Menu**, **Oyun**.
İsimler kodda yazdığı gibi olmalı (büyük/küçük harf dahil).
**File → Build Profiles → Scene List**: üçünü de sürükle, sırası **Splash, Menu, Oyun** olsun.

> Her Canvas'ta: Canvas Scaler → **Scale With Screen Size**, Reference **1080 x 1920**, Match **0.5**.
> İlk TextMeshPro ekleyişinde çıkan pencerede **Import TMP Essentials** de.
> EventSystem'de sarı uyarı çıkarsa **Replace with InputSystemUIInputModule** butonuna bas.

---

## 3) Splash sahnesi
1. Hierarchy → sağ tık → **UI → Canvas**.
2. Canvas içine **UI → Image**: Source Image = KagitAdam, boyut 400x400, ortada.
3. Canvas içine **UI → Text - TextMeshPro**: "RENK KOŞUSU", font 120, ortalı, resmin üstüne.
4. Altına küçük bir yazı: "dokun ve başla" (font 50).
5. **Create Empty** → adı `Splash` → **Splash.cs** ekle.
6. Kamerayı seç → Background istediğin bir renk.

## 4) Menu sahnesi
1. **UI → Canvas**. İçine **UI → Panel** → adı `AnaPanel`.
2. AnaPanel içine:
   - Başlık yazısı (TextMeshPro): "RENK KOŞUSU"
   - 3 buton (**UI → Button - TextMeshPro**): `Başla`, `Ayarlar`, `Çıkış`
3. Canvas içine ikinci bir **Panel** → adı `AyarlarPaneli`. İçine:
   - Başlık: "AYARLAR" (içeriğini sonra konuşacağız)
   - Buton: `Geri`
4. **Create Empty** → adı `Menu` → **Menu.cs** ekle → Inspector'da `Ana Panel` ve `Ayarlar Paneli` kutularına panelleri sürükle.
5. Her butonun **On Click ()** kısmında **+** → `Menu` objesini sürükle → fonksiyon seç:
   - Başla → `Menu.Basla`
   - Ayarlar → `Menu.AyarlariAc`
   - Çıkış → `Menu.Cikis`
   - Geri → `Menu.AyarlariKapat`

## 5) Oyun sahnesi
### Kamera ve su
- Main Camera: **Size = 7**, Background = **mavi** (örn. `#2A7FD4`) → bu **su**.

### Kare prefabı
1. **2D Object → Sprites → Square** → adı `Kare`, Scale **(1.3, 1.3, 1)**.
2. **Add Component → Box Collider 2D**, **Add Component → Kare**.
3. `Assets/Prefabs` klasörü aç, Kare'yi oraya sürükle (prefab oldu), sahnedekini sil.

### Oyuncu
1. KagitAdam.png'yi sahneye sürükle → adı `Oyuncu`, Position **(0, 0, 0)**, Scale **(0.9, 0.9, 1)**.
2. Sprite Renderer → **Order in Layer = 5** (karelerin üstünde görünsün).
3. **Add Component → Oyuncu**.

### Renk ekranı (arenadaki büyük ekran)
1. **2D Object → Sprites → Square** → adı `RenkEkrani`, Position **(0, 4.8, 0)**, Scale **(5, 1.6, 1)**.
2. **3D Object → Text - TextMeshPro** (dikkat: UI değil) → adı `EkranYazisi`, Position **(0, 4.8, 0)**, yazı "YEŞİL!", Font Size **8**, ortalı, renk siyah.
   Extra Settings → **Order in Layer = 2**.

### Canvas (yazılar, joystick, sonuç)
1. **UI → Canvas** (Scaler ayarını unutma).
2. **BilgiYazisi**: Text - TextMeshPro, Anchor **üst-orta**, font 50. ("Tur 1/10 Can: 3")
3. **Joystick**:
   - Canvas içine **UI → Image** → adı `Taban`, Anchor **sol-alt**, Pos **(220, 220)**, boyut **300x300**, Source Image **Knob**, renk alfa ~100.
   - Taban'ın içine **UI → Image** → adı `Topuz`, boyut **130x130**, Source Image **Knob**, alfa ~200.
   - Taban'a **Add Component → Joystick** → `Taban` kutusuna Taban'ı, `Topuz` kutusuna Topuz'u sürükle.
4. **SonucPaneli**: **UI → Panel**, içine:
   - `SonucYazisi` (TextMeshPro, font 90, ortada)
   - 2 buton: `Tekrar`, `Menü`

### Oyun yöneticisi
1. **Create Empty** → adı `OyunYonetici` → **OyunYonetici** betiğini ekle.
2. Inspector'daki kutuları doldur:

| Kutu | Ne sürüklenecek |
|---|---|
| Kare Prefab | Prefabs/Kare |
| Oyuncu | Oyuncu |
| Renk Ekrani | RenkEkrani |
| Ekran Yazisi | EkranYazisi |
| Bilgi Yazisi | BilgiYazisi |
| Sonuc Paneli | SonucPaneli |
| Sonuc Yazisi | SonucYazisi |

3. **Oyuncu** objesini seç → `Joystick` kutusuna **Taban**'ı sürükle.
4. Butonların On Click'i: `OyunYonetici` → Tekrar = `TekrarOyna`, Menü = `MenuyeDon`.

**Splash sahnesini açıp Play'e bas.** Splash → Menu → Oyun akışı çalışmalı.
Klavyede **WASD**, fareyle ya da Device Simulator'da sol alttaki joystick.

---

## Inspector'a açılan değişkenler (Ek B için)
| Değişken | Tür | Değer | Ne yapar |
|---|---|---|---|
| turSuresi | float | 3 | İlk turda koşmak için süre |
| hizlanma | float | 0.2 | Her turda süre bu kadar kısalır |
| toplamTur | int | 10 | Kaç tur dayanınca kazanılır |
| can | int | 3 | Kaç kez suya düşebilirsin |
| hiz (Oyuncu) | float | 4 | Kağıt adamın koşma hızı |
| kenarAdet | int | 4 | Zemin 4x4 |

## Şartlar nerede?
- **Dokunma:** Joystick.cs ve Splash.cs içinde `Pointer` (Merhaba.cs'den)
- **Inspector'da 3+ değişken:** yukarıdaki tablo (Hareket.cs'den)
- **if ve for:** OyunYonetici.cs → `Start` (kareleri üreten for), `TurBitti` (kural if'i)
- **switch:** OyunYonetici.cs → `Renk()` (Sekme/TopUretici'den)
- **Sonucu gösterme:** SonucPaneli ekranda + Console'a Debug.Log

## Dış kaynak (tasarım kartına yaz!)
Ders betikleri dışındaki kod (Joystick, sahne geçişi, menü, TextMeshPro kullanımı) **Claude (yapay zekâ) yardımıyla** yazıldı. Kağıt adam görseli de Claude ile çizildi.

## Önerilen commit sırası (en az 8)
1. Proje kuruldu, .gitignore eklendi
2-3. A aşaması: ders betikleri eklendi ve çalıştırıldı
4-9. B aşaması: her deney için bir commit (örn. "hiz 10 yapıldı, oyun telaşlı")
10. Zemin kareleri for döngüsüyle üretildi
11. Kağıt adam WASD ile hareket ediyor
12. Joystick eklendi
13. Tur sistemi, can ve kazanma/kaybetme eklendi
14. Splash ve Menü sahneleri eklendi
15-16. Oyun testi düzeltmesi 1 ve 2
