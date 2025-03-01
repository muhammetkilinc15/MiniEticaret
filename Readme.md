# MiniEticaret Microservices Project

A microservices-based e-commerce application that utilizes different database technologies for different services.

## Project Overview

MiniEticaret is a modern e-commerce platform built using microservices architecture. The project demonstrates the implementation of different database technologies for specific business domains, showcasing the polyglot persistence approach in microservices.

## Architecture

The application is divided into separate microservices, each responsible for a specific business domain:

- **Products Service**: Manages product catalog and inventory
- **Carts Service**: Handles shopping cart operations
- **Orders Service**: Processes and manages customer orders

## Database Technologies

Each service uses a database technology optimized for its specific needs:

| Service | Database |
|---------|----------|
| Products | Microsoft SQL Server (MSSQL) |
| Carts | PostgreSQL |
| Orders | MongoDB |

## Technologies Used

- .NET Core / ASP.NET Core
- MSSQL
- PostgreSQL
- MongoDB
- Docker (for containerization)
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
3. Build and run each service

## API Documentation

Each service exposes its own RESTful API:

- Products API: `/products`
- Carts API: `/carts`
- Orders API: `/orders`

## Turkish Description / Türkçe Açıklama

MiniEticaret, mikroservis mimarisi kullanılarak geliştirilmiş modern bir e-ticaret platformudur. Proje, farklı iş alanları için farklı veritabanı teknolojilerinin uygulanmasını göstermektedir:

- Ürünler servisi: MSSQL veritabanı
- Sepet servisi: PostgreSQL veritabanı
- Sipariş servisi: MongoDB veritabanı

Bu yapı, her servisin kendi ihtiyaçlarına en uygun veritabanı teknolojisini kullanmasına olanak tanır ve mikroservis mimarisinin esnekliğini gösterir.

