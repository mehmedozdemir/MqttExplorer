# 📋 Proje Geliştirme Talimatları

## 🔄 Git Workflow

Bu projede tüm geliştirmeler için aşağıdaki iş akışı **zorunlu** olarak uygulanacaktır.

### 1️⃣ Branch Oluşturma

Her yeni özellik, hata düzeltme veya geliştirme için **uygun isimlendirilmiş** bir branch oluşturulmalıdır.

**Branch İsimlendirme Kuralları:**
- `feature/[özellik-adı]` - Yeni özellik eklemeleri için
  - Örnek: `feature/dark-theme`, `feature/message-filter`
- `bugfix/[hata-açıklaması]` - Hata düzeltmeleri için
  - Örnek: `bugfix/connection-timeout`, `bugfix/null-reference-error`
- `refactor/[alan-adı]` - Kod iyileştirmeleri için
  - Örnek: `refactor/ui-components`, `refactor/service-layer`
- `docs/[konu]` - Dokümantasyon güncellemeleri için
  - Örnek: `docs/readme-update`, `docs/api-documentation`

**Branch Oluşturma Komutu:**
```bash
git checkout -b [branch-tipi]/[açıklama]
```

### 2️⃣ Geliştirme Süreci

1. **Geliştirmeyi Yap**
   - Kod değişikliklerini yap
   - İlgili dosyaları düzenle
   - Yeni dosyalar ekle (gerekirse)

2. **Code Review (Senior Engineer Perspektifi)**
   - **SOLID Prensipleri**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
   - **Clean Code**: Anlaşılır değişken/metod isimleri, kısa metodlar, low coupling/high cohesion
   - **Error Handling**: Try-catch blokları uygun mu? Exception'lar yakalanıyor mu?
   - **Resource Management**: IDisposable pattern doğru uygulanmış mı? Using statements var mı?
   - **Memory Leaks**: Event handler'lar unsubscribe ediliyor mu? Timer'lar dispose ediliyor mu?
   - **Thread Safety**: Async/await doğru kullanılmış mı? UI thread güncellemeleri güvenli mi?
   - **Performance**: N+1 sorunu var mı? Gereksiz döngüler var mı?
   - **Security**: Connection string'ler güvenli mi? Input validation yapılıyor mu?
   - **Maintainability**: Kod tekrarı var mı? Magic number/string kullanımı var mı?
   - **Documentation**: Kritik metodlarda XML comment var mı?

3. **Bulguları Düzelt**
   - Code review'da tespit edilen sorunları gider
   - Refactoring yap (gerekirse)
   - Best practice'lere uy

4. **Build Kontrolü**
   ```bash
   dotnet build RabbitMQExplorer.csproj
   ```
   - Build başarılı olmalı
   - Kritik hata (error) olmamalı
   - Warning'ler kabul edilebilir

5. **Uygulamayı Çalıştır ve Test Et**
   ```bash
   dotnet run --project RabbitMQExplorer.csproj
   ```
   - Uygulama sorunsuz açılmalı
   - Yeni özellik çalışmalı
   - Mevcut özellikler bozulmamalı
   - UI responsive olmalı
   - Hata mesajları kontrol edilmeli

### 3️⃣ Versiyonlama

**Her commit öncesi versiyon numarasını güncelle:**

1. **RabbitMQExplorer.csproj Güncelle**
   ```xml
   <Version>X.Y.Z</Version>
   <AssemblyVersion>X.Y.Z.0</AssemblyVersion>
   <FileVersion>X.Y.Z.0</FileVersion>
   ```

2. **Versiyon Artırma Kuralları (Semantic Versioning)**
   - **PATCH (0.0.X)**: Bug fix, refactoring, performance
     - Örnek: `1.0.0` → `1.0.1`
   - **MINOR (0.X.0)**: Yeni özellik (backward compatible)
     - Örnek: `1.0.0` → `1.1.0`
   - **MAJOR (X.0.0)**: Breaking changes
     - Örnek: `1.0.0` → `2.0.0`

3. **CHANGELOG.md Güncelle**
   ```markdown
   ## [X.Y.Z] - YYYY-MM-DD
   
   ### ✨ Added / 🔧 Fixed / 🔄 Changed / 🗑️ Removed
   - Değişiklik açıklaması
   ```

**Versiyon Kategori Tablosu:**

| Branch Tipi | Değişiklik Türü | Versiyon | Örnek |
|------------|-----------------|----------|-------|
| `bugfix/*` | Bug düzeltme | PATCH | 1.0.0 → 1.0.1 |
| `refactor/*` | Kod iyileştirme | PATCH | 1.0.0 → 1.0.1 |
| `feature/*` | Yeni özellik | MINOR | 1.0.0 → 1.1.0 |
| `docs/*` | Sadece dökümantasyon | - | Değişmez |

### 4️⃣ Kullanıcı Onayı

- Geliştirme tamamlandıktan ve testler başarılı olduktan sonra
- Kullanıcıya **"Test etmeye hazır, onay bekleniyor"** mesajı gönder
- Kullanıcı uygulamayı test etsin
- Kullanıcıdan **açık onay** bekle

### 5️⃣ Commit İşlemi

**Kullanıcı onayı alındıktan sonra:**

1. **Değişiklikleri Stage'e Al**
   ```bash
   git add .
   ```

2. **Commit Yap**
   ```bash
   git commit -m "[tip]: [açıklama]"
   ```

**Commit Mesaj Formatı:**
- `feat: [özellik açıklaması]` - Yeni özellik
  - Örnek: `feat: Dark theme desteği eklendi`
- `fix: [hata açıklaması]` - Hata düzeltme
  - Örnek: `fix: Profil yönetiminde null reference hatası düzeltildi`
- `refactor: [açıklama]` - Kod iyileştirme
  - Örnek: `refactor: RabbitMQService sınıfı yeniden yapılandırıldı`
- `docs: [açıklama]` - Dokümantasyon
  - Örnek: `docs: README.md güncellendi`
- `style: [açıklama]` - UI/Stil değişiklikleri
  - Örnek: `style: Queue browser grid renkleri güncellendi`
- `perf: [açıklama]` - Performans iyileştirmesi
  - Örnek: `perf: Mesaj yükleme optimizasyonu`
- `chore: [açıklama]` - Build, versiyon, dependency güncellemeleri
  - Örnek: `chore: Version 1.0.3'e güncellendi`

### 6️⃣ Push İşlemi

```bash
git push origin [branch-adı]
```

### 7️⃣ Merge İşlemi

1. **Main Branch'e Geç**
   ```bash
   git checkout main
   ```

2. **Branch'i Merge Et**
   ```bash
   git merge [branch-adı]
   ```

3. **Main'i Push Et**
   ```bash
   git push origin main
   ```

4. **Git Tag Oluştur (Versiyon için)**
   ```bash
   git tag -a v1.0.X -m "Version 1.0.X - [Açıklama]"
   git push origin v1.0.X
   ```

5. **Branch'i Sil (Opsiyonel)**
   ```bash
   git branch -d [branch-adı]
   git push origin --delete [branch-adı]
   ```

---

## 🚫 Yapılmaması Gerekenler

- ❌ Doğrudan `main` branch üzerinde çalışma
- ❌ Test edilmemiş kodu commit etme
- ❌ Build hatası olan kodu commit etme
- ❌ Kullanıcı onayı almadan merge etme
- ❌ Anlamsız commit mesajları (`update`, `fix`, `changes` gibi)

---

## ✅ Checklist (Her Geliştirme İçin)

- [ ] Branch oluşturuldu mu?
- [ ] Geliştirme tamamlandı mı?
- [ ] **Code review yapıldı mı?** (SOLID, Clean Code, Error Handling, Memory Management)
- [ ] **Code review bulguları düzeltildi mi?**
- [ ] **Versiyon numarası güncellendi mi?** (.csproj dosyası)
- [ ] **CHANGELOG.md güncellendi mi?**
- [ ] `dotnet build` başarılı mı?
- [ ] `dotnet run` ile uygulama çalışıyor mu?
- [ ] Yeni özellik test edildi mi?
- [ ] Mevcut özellikler bozulmadı mı?
- [ ] **Performans kontrolü yapıldı mı?**
- [ ] **Memory leak kontrolü yapıldı mı?**
- [ ] Kullanıcı onayı alındı mı?
- [ ] Commit mesajı uygun formatta mı?
- [ ] Push yapıldı mı?
- [ ] Merge işlemi tamamlandı mı?
- [ ] **Git tag oluşturuldu mu?** (Versiyon için)

---

## 📝 Örnek Workflow

```bash
# 1. Yeni özellik için branch oluştur
git checkout -b feature/message-search

# 2. Geliştirmeyi yap
# ... kod değişiklikleri ...

# 3. VERSİYON GÜNCELLE
# RabbitMQExplorer.csproj: 1.0.0 → 1.1.0 (yeni özellik)
# CHANGELOG.md: [1.1.0] bölümü ekle

# 4. CODE REVIEW YAP
# - SOLID prensipleri kontrol et
# - Clean Code standartlarına uy
# - Error handling gözden geçir
# - Resource management kontrol et
# - Memory leak olasılıklarını araştır

# 5. Bulguları düzelt
# ... code review bulgularını gider ...

# 6. Build ve test
dotnet build RabbitMQExplorer.csproj
dotnet run --project RabbitMQExplorer.csproj

# 7. Kullanıcıdan onay bekle
# "Test etmeye hazır, onay bekleniyor"

# 8. Onay alındıktan sonra commit
git add .
git commit -m "feat: Mesaj arama özelliği eklendi"

# 9. Push
git push origin feature/message-search

# 10. Merge
git checkout main
git merge feature/message-search
git push origin main

# 11. Git Tag oluştur
git tag -a v1.1.0 -m "Version 1.1.0 - Mesaj arama özelliği"
git push origin v1.1.0

# 12. Branch'i temizle (opsiyonel)
git branch -d feature/message-search
git push origin --delete feature/message-search
```

---

## 🎯 Proje Hedefleri

- **Kod Kalitesi**: Her commit çalışan, test edilmiş kod içermeli
- **İzlenebilirlik**: Git geçmişi anlaşılır ve düzenli olmalı, her versiyon CHANGELOG.md'de dokümante edilmeli
- **Güvenlik**: Main branch her zaman kararlı olmalı
- **İşbirliği**: Değişiklikler dokümante edilmeli
- **Versiyonlama**: Semantic Versioning'e uygun versiyon yönetimi

---

## 📚 Ek Kaynaklar

- [Git Branch Strategy](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [.NET Build Best Practices](https://learn.microsoft.com/en-us/dotnet/core/tools/)
- [Semantic Versioning](https://semver.org/)
- [Keep a Changelog](https://keepachangelog.com/)

---

**Son Güncelleme**: 30 Ocak 2026  
**Proje**: RabbitMQ Explorer  
**Geliştirici**: Senior Software Engineer  
**Mevcut Versiyon**: 1.0.2

---

**Son Güncelleme**: 30 Ocak 2026
**Proje**: RabbitMQ Explorer
**Geliştirici**: Senior Software Engineer
