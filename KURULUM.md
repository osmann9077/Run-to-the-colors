# RENK KOŞUSU – Kurulum (10 dakika)

**Kural:** Ekranda hangi renk çıkarsa süre bitmeden o renkteki kareye koş. Diğer kareler suya düşer; yanlış yerdeysen sen de düşersin ve 1 can gider.
**Kazanma:** `if (tur >= toplamTur)` → 10 tur dayan. **Kaybetme:** `if (can <= 0)` → 3 can biter.
**Kontroller:** Bilgisayarda W A S D, telefonda / Device Simulator'da sol alttaki joystick.

## 1) Unity projesi
1. Unity Hub → **New project** → **Universal 2D** → adı `RenkKosusu` → Create.
2. Proje açılınca **Unity'yi kapat.**

## 2) Dosyaları kopyala (Git geçmişi dahil)
1. Zip'i aç. İçindeki `RenkKosusu` klasörünün **içindeki her şeyi** (gizli `.git` klasörü dahil) Unity projesinin klasörüne kopyala. "Değiştir" diye sorarsa **Evet**.
   - Windows'ta `.git` görünmüyorsa: Gezgin → Görünüm → **Gizli öğeler**'i aç.
2. Proje klasöründe terminal aç (klasörde boş yere sağ tık → "Open Git Bash here" ya da "Terminalde aç") ve **bir kez** şunu çalıştır:
   ```
   git rebase --root --exec "git commit --amend --no-edit --reset-author"
   ```
   Bu komut commit'lerin yazarını senin Git hesabın yapar.

## 3) Unity'de tek tıkla kurulum
1. Projeyi Unity Hub'dan tekrar aç.
2. **Window → TextMeshPro → Import TMP Essential Resources** → Import.
3. Üst menüde **Renk Koşusu → Her Şeyi Kur** → "Hazır!" penceresi çıkar.
4. Game penceresinin üstünden çözünürlüğü **1080x1920 Portrait** yap (yoksa + ile ekle).
5. **Play** → Splash → Menü → Başla → oyun.
   - Telefon görünümü: Game penceresinin sol üstünden **Simulator**'a geç.

Console'da kırmızı hata çıkarsa ekran görüntüsünü Claude'a at.

## 4) Git'e kaydet ve GitHub'a yükle
```
git add .
git commit -m "Sahneler kuruldu, proje ayarlari eklendi"
```
GitHub'da boş repo aç (README ekleme), sonra:
```
git remote add origin https://github.com/KULLANICI_ADIN/RenkKosusu.git
git push -u origin main
```

## 5) Senin yapman gerekenler (kimse yerine yapamaz)
- [ ] **Deney sahnelerini çalıştır:** `Assets/Scenes/Deney_Merhaba`, `Deney_Hareket`, `Deney_Sekme`. Belgede yazanları gözünle doğrula, farklıysa belgeyi düzelt.
- [ ] **Oyun testi:** 2 arkadaşına 3'er dakika oynat, hiçbir şey anlatma, not al. En büyük 2 sorunu düzelt, **her düzeltmeyi ayrı commit** yap. Belgedeki test tablosunu doldur.
- [ ] **60 sn ekran kaydı:** oyunun başı + bir kazanma ya da kaybetme anı + Tekrar Oyna.
- [ ] **Kodları çalış:** Savunmada rastgele satır sorulacak.

## Dosyalar
| Klasör | Ne var |
|---|---|
| Assets/Scripts | Oyun: OyunYonetici, Oyuncu, Kare, SanalJoystick, Splash, MenuYonetici |
| Assets/DersBetikleri | Merhaba, Hareket, Sekme, TopUretici (B deneyleri işlenmiş hali) |
| Assets/Editor | Sahneleri kuran araç (oyunun parçası değil) |
| Assets/Sprites | Kağıt adam, kare, daire |
