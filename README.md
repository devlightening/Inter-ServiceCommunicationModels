Inter-ServiceCommunicationModels
Bu depo, mikroservis mimarisinde kullanılan üç farklı iletişim yöntemini karşılaştırmalı olarak sergileyen bir projedir. Proje, gRPC, HTTP ve Mesaj Broker'ı tabanlı iletişim modellerini .NET ortamında nasıl uygulayabileceğinizi gösterir.

İletişim Modelleri
Proje, üç ana klasöre ayrılmıştır: gRPC, http ve MessageBroker. Her klasör, iki servis (ServiceA ve ServiceB) arasında farklı bir iletişim modelini kullanan örnekler içerir.

1. HTTP (Senkron)
Bu model, geleneksel RESTful API çağrıları üzerinden senkron iletişimi gösterir. İstemci, sunucuya bir istek gönderir ve yanıtı bekler. Bu yöntem, anlaşılması kolaydır ancak servisler arasında sıkı bir bağımlılık oluşturabilir.

Diyagram: Senkron HTTP İletişimi
Kod snippet'i

sequenceDiagram
    participant Client
    participant ServiceA
    participant DatabaseA as ServiceA Database
    participant ServiceB as HTTP Service
    participant DatabaseB as ServiceB Database

    Client->>ServiceA: HTTP PUT /person/{id} (Update Person)
    ServiceA->>DatabaseA: Update Person Record
    DatabaseA-->>ServiceA: Success
    ServiceA->>ServiceB: HTTP PUT /employee/{id} (Update Employee)
    ServiceB->>DatabaseB: Update Employee Record
    DatabaseB-->>ServiceB: Success
    ServiceB-->>ServiceA: HTTP 200 OK
    ServiceA-->>Client: HTTP 200 OK
2. gRPC (Senkron)
gRPC, Google tarafından geliştirilen yüksek performanslı, dil bağımsız bir RPC (Remote Procedure Call) çerçevesidir. Bu model, HTTP'ye göre daha hızlı ve daha az bant genişliği kullanır.

Diyagram: Senkron gRPC İletişimi
Kod snippet'i

sequenceDiagram
    participant Client
    participant ServiceA
    participant DatabaseA as ServiceA Database
    participant ServiceB as gRPC Service
    participant DatabaseB as ServiceB Database

    Client->>ServiceA: gRPC Call (Update Person)
    ServiceA->>DatabaseA: Update Person Record
    DatabaseA-->>ServiceA: Success
    ServiceA->>ServiceB: gRPC Call (Update Employee)
    ServiceB->>DatabaseB: Update Employee Record
    DatabaseB-->>ServiceB: Success
    ServiceB-->>ServiceA: gRPC Response
    ServiceA-->>Client: gRPC Response
3. Mesaj Broker'ı (Asenkron)
Bu model, servislerin bir mesaj kuyruğu üzerinden dolaylı olarak iletişim kurmasını sağlar. Bir servis bir olay yayınlar, diğer servisler bu olayı dinler ve işler. Bu yaklaşım, servisler arasında gevşek bağlılık, yüksek ölçeklenebilirlik ve hata toleransı sağlar.

Diyagram: Asenkron Mesaj İletişimi
Kod snippet'i

sequenceDiagram
    participant Client
    participant ServiceA
    participant DatabaseA as ServiceA Database
    participant RabbitMQ as Message Broker
    participant ServiceB
    participant DatabaseB as ServiceB Database

    Client->>ServiceA: HTTP PUT /person/{id} (Update Person)
    ServiceA->>DatabaseA: Update Person Record
    DatabaseA-->>ServiceA: Success
    ServiceA->>RabbitMQ: Publish PersonUpdatedEvent
    RabbitMQ-->>ServiceA: Ack (Event Sent)
    ServiceA-->>Client: HTTP 202 Accepted (Immediately)

    Note over RabbitMQ,ServiceB: Asynchronous Process
    RabbitMQ->>ServiceB: Consume PersonUpdatedEvent
    ServiceB->>DatabaseB: Update Employee Record
    DatabaseB-->>ServiceB: Success
Kullanılan Teknolojiler
.NET 7/8: Proje, .NET platformunda geliştirilmiştir.

gRPC: Yüksek performanslı senkron iletişim için kullanılır.

HTTP/REST: Geleneksel senkron iletişim için kullanılır.

MongoDB: Servisler için veri deposu olarak kullanılan NoSQL veritabanıdır.

MassTransit: RabbitMQ ile çalışmayı kolaylaştıran bir dağıtık uygulama framework'üdür.

RabbitMQ: Asenkron iletişim için kullanılan bir mesaj broker'ıdır.

Docker: RabbitMQ'yu yerel ortamda kolayca çalıştırmak için kullanılır.

Başlangıç Kılavuzu
Önkoşullar
.NET SDK

Docker Desktop

Projeyi Çalıştırma
Depoyu Klonlayın:
git clone https://github.com/devlightening/Inter-ServiceCommunicationModels.git
cd Inter-ServiceCommunicationModels

RabbitMQ'yu Çalıştırın (Mesaj Broker modeli için):
docker run -d --hostname my-rabbit --name rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management

Servisleri Başlatın:
Visual Studio'da çözümü açın ve istediğiniz iletişim modeli için (http, gRPC veya MessageBroker) ilgili servisleri başlatın. Multiple startup projects ayarını kullanarak servisleri eş zamanlı çalıştırabilirsiniz.

API'lere Erişin:
Servislerle etkileşim kurmak ve iletişim modellerini çalışırken gözlemlemek için Swagger UI veya Postman gibi araçları kullanabilirsiniz.
