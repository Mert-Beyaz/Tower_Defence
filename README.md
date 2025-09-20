# 🏰 Tower Defence

Unity ile geliştirilmiş, dalga tabanlı bir Tower Defence oyunu.

![Unity](https://img.shields.io/badge/Unity-2022.3.62f1-blue)

---

## 🎮 Kullanılan Oyun Motoru ve Versiyonu
- **Oyun Motoru:** Unity  
- **Versiyon:** 2022.3.62f1 LTS  

---

## 📌 Varsayımlar
- Oyuncu **WASD** tuşları ile hareket eder.
- En yakındaki rakibe menzilli otomatik saldırı gönderir.
- Hasar alınca kısa süreli dokunulmazlık (I-frame) aktif olur.
- Düşmanlar dalga dalga (wave-based) gelir.
- Düşmanlar belirlenen yolu takip eder.
- **Space** tuşu ile bir sonraki dalga erken çağrılabilir.
- Düşmanların renk, hız, can ve güç değerleri ayarlanabilir.
- Tower Defence mekaniği için başlangıç ve bitiş noktası hazırdır.
- Oyun **30 FPS**'te stabil çalışır.
- Oyuna başlangıç menüsü eklenmiştir.
- **E** tuşu ile farklı bir silah seçilebilir.
- VFX ve partikül efektleri eklenmiştir.
- Ses efektleri eklenmiştir (vurma, vurulma, yürüme vb.).
- 5., 10. ve 15. dalgalarda **Boss Wave** yapılmıştır.

---

## 📝 Açıklamalar ve Notlar
- Bu proje **study case** amacıyla hazırlanmıştır.
- `Assets/Undead Survivor` klasöründeki dosyalar [Unity Asset Store](https://assetstore.unity.com/packages/2d/undead-survivor-assets-pack-238068)'dan alınmıştır.
- Düşman ayarları:  
  ```text
  Assets/PoolingSystem/Prefab/Monster
  ```
- Mermilerin özellikleri:  
  ```text
  Assets/PoolingSystem/Prefab/Projectile
  ```
- Dalgaların ayarlanması:  
  ```text
  SampleScene > Hierarchy > Wave Controller > Wave Controller script > Wave
  ```
- Oyuncu özellikleri:  
  ```text
  SampleScene > Hierarchy > Player > Player Movement & Player Controller scripts
  ```
- Seslerin düzenlenmesi:  
  ```text
  Assets/Resources/SoundData
  ```

---

## 🚀 Çalıştırma
1. Bu repository’yi klonlayın:
   ```bash
   git clone https://github.com/Mert-Beyaz/Tower_Defence.git
   ```
2. Projeyi Unity ile açın.
3. `SampleScene` dosyasını çalıştırın.
