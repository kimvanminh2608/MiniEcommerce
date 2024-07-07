# Mini Store Management System

First words, I received task assignment and I tried to keep your deadline but It seem I couldn't finish the test assignment on time,
so I hope that you can give me a opportunity for the technical interview with you.
And again thank you for considering my resume.

*Introduction
This is my mini ecommerce microservice about a store management system, I just want to implement sample using technologies,
design, principles the assignment describe for creating microservice app.

*Technologies:
.NET 8.0
Microservices 
RabbitMQ Message broker and MassTransit library 
RabbitMQ .net client
Entity Framework core provider
Swagger & SwaggerUI
Serilog

*Setup

1. Clone the project in my GitHub to your local
2. In AppSettings.json, you can find this line

"ConnectionStrings": {
  "DefaultConnection": "data source=Admin-PC\\SQLEXPRESS;initial catalog=UserDatabase;trusted_connection=true;TrustServerCertificate=True"
},

replace the default connection with your connectionString
3. Run your application
4. using swagger to test the API


*Application Architecture
The bellow architecture shows that there is one public API which is accessible for the clients. The API gateway then routes the HTTP request to the corresponding microservice. The HTTP request is received by the microservice that hosts its own REST API. Each microservice is running within its own  databases.

![Screenshot 2024-07-07 224925](https://github.com/kimvanminh2608/MiniEcommerce/assets/56824172/7b78e8cb-773a-4a7f-bda3-9695dc0c9379)


