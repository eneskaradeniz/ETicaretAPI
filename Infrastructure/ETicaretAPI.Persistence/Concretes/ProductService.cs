﻿using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public List<Product> GetProducts() => new()
        {
            new() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100, Stock = 10 },
            new() { Id = Guid.NewGuid(), Name = "Product 2", Price = 200, Stock = 20 },
            new() { Id = Guid.NewGuid(), Name = "Product 3", Price = 300, Stock = 30 },
            new() { Id = Guid.NewGuid(), Name = "Product 4", Price = 400, Stock = 40 },
            new() { Id = Guid.NewGuid(), Name = "Product 5", Price = 500, Stock = 50 }
        };
    }
}
