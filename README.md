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
