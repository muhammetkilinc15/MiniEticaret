# MiniEticaret Microservices Project

A microservices-based e-commerce application that utilizes different database technologies for different services.

## Project Overview

MiniEticaret is a modern e-commerce platform built using microservices architecture. The project demonstrates the implementation of different database technologies for specific business domains, showcasing the polyglot persistence approach in microservices. Services are containerized using Docker and communicate through a YARP API Gateway with JWT authentication.

## Architecture

The application is divided into separate microservices, each responsible for a specific business domain:

- **API Gateway**: YARP (Yet Another Reverse Proxy) handles routing and JWT authentication
- **Users Service**: Manages user accounts, authentication and authorization
- **Products Service**: Manages product catalog and inventory
- **Carts Service**: Handles shopping cart operations
- **Orders Service**: Processes and manages customer orders

### API Gateway Pattern

This project implements the API Gateway pattern using YARP:

- **Centralized Entry Point**: All client requests go through the gateway
- **Authentication & Authorization**: JWT-based security implemented at the gateway level
- **Request Routing**: Requests are routed to appropriate microservices
- **Load Balancing**: Distribution of requests across service instances

## Database Technologies

Each service uses a database technology optimized for its specific needs:

| Service | Database |
|---------|----------|
| Users | PostgreSQL |
| Products | Microsoft SQL Server (MSSQL) |
| Carts | PostgreSQL |
| Orders | MongoDB |

## Technologies Used

- .NET Core / ASP.NET Core
- YARP API Gateway
- JWT Authentication
- MSSQL
- PostgreSQL
- MongoDB
- Docker & Docker Compose (for containerization)
- RESTful APIs for inter-service communication

## Getting Started

### Prerequisites

- .NET Core SDK
- Docker and Docker Compose
- MSSQL Server
- PostgreSQL
- MongoDB

### Installation

1. Clone the repository
2. Configure connection strings for each service
3. Build Docker images for each service:
   ```
   docker-compose build
   ```
4. Run the containerized services:
   ```
   docker-compose up -d
   ```

### Authentication

The system uses JWT (JSON Web Token) authentication implemented at the API Gateway level. User management is handled through the Users service which uses PostgreSQL database. To access protected endpoints:

1. Register a new user account at `/api/users/register`
2. Login at `/api/users/login` to receive a JWT token
3. Include the token in the Authorization header for subsequent requests:
   ```
   Authorization: Bearer {your-jwt-token}
   ```

## API Documentation

All API endpoints are accessible through the API Gateway:

- Gateway: `http://localhost:5000`
- Users API: `/api/users`
- Products API: `/api/products`
- Carts API: `/api/carts`
- Orders API: `/api/orders`

## Container Architecture

Each service runs in its own Docker container:

- `api-gateway`: YARP API Gateway
- `users-service`: User management service with PostgreSQL
- `products-service`: Product management service with MSSQL
- `carts-service`: Shopping cart service with PostgreSQL
- `orders-service`: Order management service with MongoDB

## Turkish Description / Türkçe Açıklama

MiniEticaret, mikroservis mimarisi kullanılarak geliştirilmiş modern bir e-ticaret platformudur. Proje, farklı iş alanları için farklı veritabanı teknolojilerinin uygulanmasını göstermektedir:

- Kullanıcılar servisi: PostgreSQL veritabanı
- Ürünler servisi: MSSQL veritabanı
- Sepet servisi: PostgreSQL veritabanı
- Sipariş servisi: MongoDB veritabanı

Bu yapı, her servisin kendi ihtiyaçlarına en uygun veritabanı teknolojisini kullanmasına olanak tanır ve mikroservis mimarisinin esnekliğini gösterir. Tüm servisler Docker container'ları içinde çalışmakta ve YARP API Gateway üzerinden iletişim kurmaktadır. Güvenlik, API Gateway seviyesinde JWT token tabanlı kimlik doğrulama ile sağlanmıştır. Kullanıcı kayıt ve giriş işlemleri PostgreSQL veritabanına bağlanarak gerçekleştirilmektedir.