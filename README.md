GS1 L3 Serilizasyon, Agregasyon ve Otomasyon Sistemi
Bu proje, ilaç ve hýzlý tüketim ürünleri endüstrisinde kullanýlan Track & Trace (Ýzlenebilirlik) standartlarýna (GS1) uygun olarak geliþtirilmiþ bir L3 (Hat Seviyesi) yönetim sistemidir.

Proje; .NET 8, Clean Architecture prensipleri ve Domain Driven Design (DDD) yaklaþýmlarý kullanýlarak tasarlanmýþtýr.

 Mimari Yaklaþým
Proje, baðýmlýlýklarýn içten dýþa doðru olduðu Onion Architecture (Clean Architecture) yapýsýna sahiptir:

Core (Domain): Saf iþ kurallarý, Entity'ler ve Enum'lar. Dýþ dünyaya baðýmlýlýðý yoktur. (Örn: Package, WorkOrder)

Core (Application): Ýþ senaryolarý (Use Cases), Interface tanýmlarý ve DTO'lar. (Örn: IWorkOrderService, IGS1GeneratorService)

Infrastructure: Veritabaný eriþimi (EF Core), Dýþ donaným simülasyonlarý ve servis implementasyonlarý.

API: Dýþ dünyaya açýlan RESTful uç noktalar.

Client: Operatör ekraný (WinForms - Demo amaçlý API tüketimi).

 Kullanýlan Teknolojiler ve Desenler
Backend: .NET 8 Web API

Veritabaný: MS SQL Server / Entity Framework Core 8 (Code-First)

Validasyon & Logic: Custom Exception Handling Middleware, Transaction Management (ACID)

Loglama: Serilog (Dosya ve Konsol tabanlý yapýlandýrýlmýþ loglama)

Dokümantasyon: Swagger / OpenAPI

Diðer: Dependency Injection, Repository Pattern (DbContext üzerinden), Self-Referencing Entity (Agregasyon için).