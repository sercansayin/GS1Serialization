------------------GS1 L3 Serilizasyon, Agregasyon ve Otomasyon Sistemi------------------------------------------
Bu proje, ilaç ve hızlı tüketim ürünleri endüstrisinde kullanılan Track & Trace (İzlenebilirlik) standartlarına (GS1) uygun olarak geliştirilmiş bir L3 (Hat Seviyesi) yönetim sistemidir.
Proje; .NET 8, Clean Architecture prensipleri ve Domain Driven Design (DDD) yaklaşımları kullanılarak tasarlanmıştır.

------------------Mimari Yaklaşım
Proje, bağımlılıkların içten dışa doğru olduğu Onion Architecture (Clean Architecture) yapısına sahiptir:
Core (Domain): Saf iş kuralları, Entity'ler ve Enum'lar. Dış dünyaya bağımlılığı yoktur. (Örn: Package, WorkOrder)
Core (Application): İş senaryoları (Use Cases), Interface tanımları ve DTO'lar. (Örn: IWorkOrderService, IGS1GeneratorService)
Infrastructure: Veritabanı erişimi (EF Core), Dış donanım simülasyonları ve servis implementasyonları.
API: Dış dünyaya açılan RESTful uç noktalar.

-------------------Kullanılan Teknolojiler ve Desenler
Backend: .NET 8 Web API
Veritabanı: MS SQL Server / Entity Framework Core 8 (Code-First)
Validasyon & Logic: Custom Exception Handling Middleware, Transaction Management (ACID)
Loglama: Serilog (Dosya ve Konsol tabanlı yapılandırılmış loglama)
Dokümantasyon: Swagger / OpenAPI
Diğer: Dependency Injection, Repository Pattern (DbContext üzerinden), Self-Referencing Entity (Agregasyon için).

-------------------Kurulum ve Çalıştırma
Gereksinimler
.NET 8 SDK
SQL Server (LocalDB veya Express)

GS1Serialization.API projesindeki appsettings.json dosyasında ConnectionStrings bölümünü kendi SQL sunucunuza göre düzenleyin.
Veritabanı Oluşturma (Migration): Visual Studio Package Manager Console üzerinden (Default Project: Infrastructure seçili iken):

Update-Database

GS1Serialization.API projesini Startup Project olarak ayarlayın ve çalıştırın.

-------------------Test Senaryosu (Swagger Üzerinden)
Sistemi uçtan uca test etmek için aşağıdaki adımları izleyebilirsiniz:

1. Hazırlık (Seed Data)
POST /api/Seed/create-test-data: Sisteme örnek bir Müşteri (GLN) ve İlaç (GTIN) ekler.

Dönen ProductId değerini not alın

2. İş Emri Oluşturma (L3 Serilizasyon)
POST /api/WorkOrders: Yeni bir üretim emri başlatır.

Örnek JSON:

JSON
{
  "productId": 1,
  "quantity": 10,
  "lotNumber": "LOT-2026-X",
  "expireDate": "2028-12-31",
  "serialNumberStart": 10000
}
Bu işlem DB'ye 10 adet "Item" seviyesinde paket ve GS1 datalarını yazar.

3. Agregasyon (Koli Oluşturma)
POST /api/WorkOrders/aggregate: Üretilen ilaçları bir koliye (Box) atar.

Örnek JSON:

JSON
{
  "workOrderId": 1,
  "targetLevel": 3,  // 3: Box, 4: Pallet
  "childSerialNumbers": [ "10000", "10001", "10002" ]
}

4. Raporlama ve Doğrulama

GET /api/WorkOrders/{id}: Oluşan hiyerarşik yapıyı (Tree View) ve GS1 stringlerini (01...21...17...10...) görüntüler.