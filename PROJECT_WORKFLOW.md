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

### 3️⃣ Kullanıcı Onayı

- Geliştirme tamamlandıktan ve testler başarılı olduktan sonra
- Kullanıcıya **"Test etmeye hazır, onay bekleniyor"** mesajı gönder
- Kullanıcı uygulamayı test etsin
- Kullanıcıdan **açık onay** bekle

### 4️⃣ Commit İşlemi

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

### 5️⃣ Push İşlemi

```bash
git push origin [branch-adı]
```

### 6️⃣ Merge İşlemi

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

4. **Branch'i Sil (Opsiyonel)**
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

---

## 📝 Örnek Workflow

```bash
# 1. Yeni özellik için branch oluştur
git checkout -b feature/message-search

# 2. Geliştirmeyi yap
# ... kod değişiklikleri ...

# 3. CODE REVIEW YAP
# - SOLID prensipleri kontrol et
# - Clean Code standartlarına uy
# - Error handling gözden geçir
# - Resource management kontrol et
# - Memory leak olasılıklarını araştır

# 4. Bulguları düzelt
# ... code review bulgularını gider ...

# 5. Build ve test
dotnet build RabbitMQExplorer.csproj
dotnet run --project RabbitMQExplorer.csproj

# 6. Kullanıcıdan onay bekle
# "Test etmeye hazır, onay bekleniyor"

# 7. Onay alındıktan sonra commit
git add .
git commit -m "feat: Mesaj arama özelliği eklendi"

# 8. Push
git push origin feature/message-search

# 9. Merge
git checkout main
git merge feature/message-search
git push origin main

# 10. Branch'i temizle (opsiyonel)
git branch -d feature/message-search
```

---

## 🎯 Proje Hedefleri

- **Kod Kalitesi**: Her commit çalışan, test edilmiş kod içermeli
- **İzlenebilirlik**: Git geçmişi anlaşılır ve düzenli olmalı
- **Güvenlik**: Main branch her zaman kararlı olmalı
- **İşbirliği**: Değişiklikler dokümante edilmeli

---

## 📚 Ek Kaynaklar

- [Git Branch Strategy](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [.NET Build Best Practices](https://learn.microsoft.com/en-us/dotnet/core/tools/)

---

**Son Güncelleme**: 30 Ocak 2026
**Proje**: RabbitMQ Explorer
**Geliştirici**: Senior Software Engineer
