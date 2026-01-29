# Changelog

Tüm önemli değişiklikler bu dosyada belgelenecektir.

Format [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) standardına uyar ve bu proje [Semantic Versioning](https://semver.org/spec/v2.0.0.html) kullanır.

## [1.0.2] - 2026-01-30

### 🔧 Fixed
- **Memory Leak Düzeltmeleri**
  - Timer'ların dispose edilmemesi sorunu düzeltildi (QueueBrowserControl, ExchangeBrowserControl)
  - Event handler memory leak'i düzeltildi (MessageConsumerControl)
  - Tab controls ve child controls proper dispose eklendi (MainForm)
  
- **Resource Management İyileştirmeleri**
  - HttpClient static yapıldı, socket exhaustion riski önlendi (RabbitMQService)
  - Event handler'lar dispose'da temizleniyor
  
- **Thread Safety İyileştirmeleri**
  - `Invoke` yerine `BeginInvoke` kullanımı (recursive call riski önlendi)
  - Separate `ProcessReceivedMessage` metodu ile daha güvenli mesaj işleme
  - Null check'ler ve `IsDisposed` kontrolü eklendi
  
- **Error Handling İyileştirmeleri**
  - Dispose işlemlerinde try-catch eklendi
  - Debug.WriteLine ile hata loglama eklendi

### 📚 Documentation
- PROJECT_WORKFLOW.md eklendi - Git workflow ve code review talimatları

## [1.0.1] - 2026-01-30

### 🔧 Fixed
- .gitignore dosyası güncellendi
  - Comprehensive Visual Studio ve .NET ignore patterns eklendi (400+ satır)
  - Build artifacts, IDE dosyaları, NuGet paketleri ignore listesine eklendi

## [1.0.0] - 2026-01-30

### 🎉 Initial Release

#### ✨ Features
- **Bağlantı Yönetimi**
  - RabbitMQ bağlantı profilleri yönetimi
  - Profil CRUD operasyonları (Create, Read, Update, Delete)
  - SSL/TLS desteği
  - Varsayılan profil belirleme
  - Otomatik yeniden bağlanma desteği
  - Connection heartbeat ve recovery

- **Queue Browser**
  - Kuyruk listesini görüntüleme
  - Gerçek zamanlı auto-refresh (5 saniye)
  - Kuyruk detayları (mesaj sayısı, consumer sayısı, durum)
  - Dead Letter Queue (DLQ) bilgileri
  - Kuyruk filtreleme ve arama
  - TTL, max-length gibi queue arguments görüntüleme

- **Exchange Browser**
  - Exchange listesini görüntüleme
  - Exchange tipleri filtreleme (direct, topic, fanout, headers)
  - Exchange istatistikleri (publish in/out)
  - Auto-refresh desteği

- **Binding Browser**
  - Binding ilişkilerini görüntüleme
  - Source, destination, routing key bilgileri
  - Binding arguments

- **Message Publisher**
  - Exchange'e mesaj gönderme
  - Custom header desteği
  - Message properties (priority, delivery mode, content type)
  - Persistent/Non-persistent mesaj desteği
  - JSON/XML message formatting

- **Message Consumer**
  - Gerçek zamanlı mesaj dinleme
  - Auto-ACK ve Manual-ACK desteği
  - Multiple consumer (her biri ayrı tab)
  - Mesaj geçmişi (son 1000 mesaj)
  - Mesaj detayları görüntüleme (headers, properties, body)
  - JSON/XML formatting desteği

- **Modern UI**
  - Material Design inspired arayüz
  - Responsive ve multitask tasarım
  - Loading indicators
  - Async/await pattern (UI blocking yok)
  - Türkçe arayüz
  - Professional color palette

#### 🛠️ Technical Details
- **.NET 8.0** Windows Forms
- **RabbitMQ.Client 6.8.1** - AMQP protocol
- **Management API** entegrasyonu (HTTP Basic Auth)
- **JSON** profile storage (AppData)
- **Service-based architecture**
  - RabbitMQService: AMQP ve Management API işlemleri
  - ProfileService: Profil yönetimi
- **Helper classes**
  - UIHelper: Modern styling ve loading overlays
  - MessageFormatter: JSON/XML pretty printing

#### 📋 Project Structure
```
RabbitMQExplorer/
├── Models/               # Data models (7 classes)
├── Services/            # Business logic (2 services)
├── Forms/               # UI controls (8 forms)
├── Helpers/             # Utility classes (2 helpers)
├── README.md            # Comprehensive documentation
└── .gitignore          # Git ignore patterns
```

#### 🔐 Security
- Basic authentication
- SSL/TLS support
- Password stored in plain text (local JSON file)

#### ⚠️ Known Limitations
- Read-only monitoring (no create/delete operations for queues/exchanges)
- Single connection at a time
- No cluster support
- No plugin management

---

## Versioning Scheme

Bu proje **Semantic Versioning** kullanır: `MAJOR.MINOR.PATCH`

- **MAJOR** (X.0.0): Breaking changes, API değişiklikleri
- **MINOR** (0.X.0): Yeni özellikler, backward compatible
- **PATCH** (0.0.X): Bug fixes, küçük iyileştirmeler

### Version Update Rules

| Change Type | Version Increment | Example |
|------------|-------------------|---------|
| Bug Fix / Hotfix | PATCH | 1.0.0 → 1.0.1 |
| New Feature | MINOR | 1.0.0 → 1.1.0 |
| Breaking Change | MAJOR | 1.0.0 → 2.0.0 |
| Refactoring | PATCH | 1.0.0 → 1.0.1 |
| Documentation | - | No version change |
| Performance | PATCH | 1.0.0 → 1.0.1 |

---

## Changelog Format

Her versiyon için:
- **Date**: YYYY-MM-DD formatında
- **Categories**: Added, Changed, Deprecated, Removed, Fixed, Security
- **Descriptions**: Kısa ve açıklayıcı

### Kategori Açıklamaları

- **✨ Added**: Yeni özellikler
- **🔄 Changed**: Mevcut fonksiyonlarda değişiklikler
- **⚠️ Deprecated**: Yakında kaldırılacak özellikler
- **🗑️ Removed**: Kaldırılan özellikler
- **🔧 Fixed**: Bug düzeltmeleri
- **🔐 Security**: Güvenlik güncellemeleri
- **📚 Documentation**: Dokümantasyon değişiklikleri
- **⚡ Performance**: Performans iyileştirmeleri

---

[Unreleased]: https://github.com/mehmedozdemir/MqttExplorer/compare/v1.0.2...HEAD
[1.0.2]: https://github.com/mehmedozdemir/MqttExplorer/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/mehmedozdemir/MqttExplorer/compare/v1.0.0...v1.0.1
[1.0.0]: https://github.com/mehmedozdemir/MqttExplorer/releases/tag/v1.0.0
