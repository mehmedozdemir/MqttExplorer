# 🐰 RabbitMQ Explorer

Modern ve profesyonel RabbitMQ yönetim ve monitoring aracı. C# WinForms ile geliştirilmiş, kullanıcı dostu arayüzü ile RabbitMQ sunucularınızı kolayca yönetin.

## ✨ Özellikler

### 🔌 Bağlantı Yönetimi
- **Çoklu Bağlantı Profilleri**: Farklı RabbitMQ sunucularını kaydedip yönetin
- **SSL/TLS Desteği**: Güvenli bağlantılar
- **Virtual Host Desteği**: Farklı sanal hostlar arasında geçiş
- **Otomatik Yeniden Bağlanma**: Bağlantı kesilmelerinde otomatik yeniden deneme

### 📦 Kuyruk Yönetimi (Queues)
- Tüm kuyrukları listeleyin ve filtreleyin
- Mesaj sayıları, consumer bilgileri, durum göstergeleri
- Dead Letter Queue (DLQ) bilgileri
- TTL, max-length gibi kuyruk özellikleri
- Otomatik yenileme (5 saniye)

### 📮 Exchange Yönetimi
- Tüm exchange'leri görüntüleyin
- Tip bazlı filtreleme (direct, fanout, topic, headers)
- Exchange istatistikleri ve mesaj sayaçları
- Detaylı exchange özellikleri

### 🔗 Binding Görüntüleme
- Exchange ve Queue binding'lerini listeleyin
- Routing key bilgileri
- Source ve destination detayları

### 📤 Mesaj Gönderme (Publisher)
- Herhangi bir exchange'e mesaj gönderin
- Routing key belirtme
- Custom headers ekleme
- Content-Type desteği (JSON, XML, Text)
- Persistent/Non-persistent mesaj seçimi
- Message priority ayarlama
- JSON otomatik formatlama

### 📨 Mesaj Dinleme (Consumer)
- Gerçek zamanlı mesaj dinleme
- Çoklu consumer desteği (birden fazla kuyruk aynı anda)
- Auto-ACK / Manual-ACK seçimi
- Mesaj geçmişi (son 1000 mesaj)
- Detaylı mesaj bilgileri ve headers
- JSON/XML otomatik formatlama

### 🎨 Modern Arayüz
- Material Design benzeri temiz arayüz
- Responsive design
- Loading göstergeleri
- Real-time güncellemeler
- Tab-based navigation
- Koyu/Açık renkli tema

## 🚀 Kurulum

### Gereksinimler
- .NET 8.0 SDK veya üzeri
- Windows 10/11
- Visual Studio 2022 (opsiyonel)

### NuGet Paketleri
```xml
<PackageReference Include="RabbitMQ.Client" Version="6.8.1" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
<PackageReference Include="System.Text.Json" Version="8.0.5" />
```

### Build
```powershell
# Restore paketleri
dotnet restore

# Build
dotnet build

# Çalıştır
dotnet run
```

### Release Build
```powershell
dotnet publish -c Release -r win-x64 --self-contained false
```

## 📖 Kullanım

### 1. Bağlantı Profili Oluşturma
1. "Profilleri Yönet..." butonuna tıklayın
2. "Yeni Profil" ile yeni bir profil oluşturun
3. Bağlantı bilgilerini girin:
   - Profil Adı
   - Sunucu (Host)
   - Port (varsayılan: 5672)
   - Virtual Host (varsayılan: /)
   - Kullanıcı Adı ve Şifre
   - SSL/TLS kullanımı
   - Management Port (varsayılan: 15672)

### 2. Bağlanma
1. Profil dropdown'dan bir profil seçin
2. "Bağlan" butonuna tıklayın
3. Bağlantı başarılı olduğunda menü butonları aktif olur

### 3. Kuyrukları Görüntüleme
1. Sol menüden "📦 Kuyruklar (Queues)" butonuna tıklayın
2. Tüm kuyruklar listelenir
3. Arama yapabilir veya sıralayabilirsiniz
4. Bir kuyruk seçtiğinizde detayları görünür

### 4. Mesaj Gönderme
1. "📤 Mesaj Gönder (Publisher)" butonuna tıklayın
2. Exchange seçin (veya default)
3. Routing key girin
4. Mesajınızı yazın
5. İsterseniz header'lar ekleyin
6. "Gönder" butonuna tıklayın

### 5. Mesaj Dinleme
1. "📨 Mesaj Dinle (Consumer)" butonuna tıklayın
2. Dinlemek istediğiniz kuyruğu seçin
3. Auto-ACK seçeneğini ayarlayın
4. "Dinlemeyi Başlat" butonuna tıklayın
5. Gelen mesajlar tabloda görünür
6. Mesaj seçince detayları görünür

## 🔧 Ayarlar

### Bağlantı Profili Ayarları
```
Sunucu: localhost (veya IP/hostname)
Port: 5672 (AMQP port)
Virtual Host: / (varsayılan)
Kullanıcı: guest
Şifre: guest
Management Port: 15672 (HTTP API)
```

### Consumer Ayarları
- **Auto-ACK**: Mesajlar otomatik olarak onaylanır
- **Manual-ACK**: Mesajlar manuel olarak onaylanır (kodda BasicAck çağrılır)

## 📊 Özellik Detayları

### Queue Bilgileri
- Kuyruk adı ve durum
- Toplam mesaj sayısı
- Hazır mesajlar (ready)
- Onaysız mesajlar (unacknowledged)
- Consumer sayısı
- Durable, Auto-delete, Exclusive özellikleri
- Dead Letter Exchange/Routing Key
- Message TTL, Max Length

### Exchange Bilgileri
- Exchange adı ve tipi
- Durable, Auto-delete, Internal özellikleri
- Gelen/Giden mesaj istatistikleri
- Arguments ve metadata

### Message Properties
- Content-Type (text/plain, application/json, application/xml)
- Delivery Mode (Persistent/Non-Persistent)
- Priority (0-10)
- Headers (custom key-value pairs)
- Correlation ID, Reply-To
- Message ID, Type, User ID, App ID
- Expiration (TTL)

## 🛠️ Teknolojiler

- **C# 12** (.NET 8.0)
- **Windows Forms** (WinForms)
- **RabbitMQ.Client** 6.8.1
- **Newtonsoft.Json** (JSON işleme)
- **Async/Await** (responsive UI)

## 📁 Proje Yapısı

```
RabbitMQExplorer/
├── Forms/
│   ├── MainForm.cs                    # Ana uygulama formu
│   ├── ProfileManagementForm.cs       # Profil yönetimi
│   ├── ProfileEditorForm.cs           # Profil düzenleme
│   ├── QueueBrowserControl.cs         # Kuyruk tarayıcı
│   ├── ExchangeBrowserControl.cs      # Exchange tarayıcı
│   ├── BindingBrowserControl.cs       # Binding tarayıcı
│   ├── MessagePublisherControl.cs     # Mesaj gönderici
│   └── MessageConsumerControl.cs      # Mesaj dinleyici
├── Services/
│   ├── RabbitMQService.cs             # RabbitMQ operations
│   └── ProfileService.cs              # Profil yönetimi
├── Models/
│   ├── ConnectionProfile.cs           # Bağlantı profili
│   ├── QueueInfo.cs                   # Kuyruk bilgisi
│   ├── ExchangeInfo.cs                # Exchange bilgisi
│   ├── BindingInfo.cs                 # Binding bilgisi
│   └── MessageInfo.cs                 # Mesaj bilgisi
├── Helpers/
│   ├── UIHelper.cs                    # UI yardımcıları
│   └── MessageFormatter.cs            # Mesaj formatlama
└── Program.cs                         # Uygulama giriş noktası
```

## 🎯 Kullanım Senaryoları

### Development
- Local RabbitMQ sunucusunda test mesajları gönderme
- Kuyruk ve exchange yapılarını görüntüleme
- Mesaj akışını debug etme

### Testing
- Farklı routing key'lerle mesaj testleri
- Dead letter queue testleri
- TTL ve priority testleri

### Monitoring
- Production sistemlerindeki kuyruk durumlarını izleme
- Mesaj sayılarını takip etme
- Consumer durumlarını kontrol etme

### Troubleshooting
- Mesaj içeriklerini inceleme
- Binding problemlerini tespit etme
- Exchange routing'i debug etme

## 🔒 Güvenlik

- Şifreler şifreli olarak saklanmaz (local app data)
- SSL/TLS bağlantı desteği
- Farklı kullanıcı hesapları için profil desteği

## 🐛 Bilinen Limitasyonlar

- Mesaj geçmişi son 1000 mesaj ile sınırlı
- Çok büyük mesajlar (>10MB) performans sorunlarına yol açabilir
- Queue/Exchange oluşturma ve silme özellikleri yok (sadece görüntüleme)

## 📝 Notlar

- RabbitMQ Management Plugin'in etkin olması gerekir
- Management API için port 15672 açık olmalıdır
- AMQP bağlantısı için port 5672 açık olmalıdır

## 🤝 Katkıda Bulunma

Bu proje Senior Software Engineer tarafından geliştirilmiştir.

## 📄 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 🎉 Teşekkürler

- RabbitMQ ekibine harika bir mesaj kuyruğu sistemi için
- Microsoft'a .NET ve WinForms için

---

**Not**: Bu uygulama production ortamlarında kullanılabilir, ancak önemli işlemler için RabbitMQ Management Console kullanmanız önerilir.
