﻿using Catalog.Infrastructure;
using Catalog.Infrastructure.Tests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Catalog.Fixtures
{
    public class InMemoryApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseEnvironment("Testing")
                .ConfigureTestServices(services =>
                {
                    var options = new DbContextOptionsBuilder<CatalogContext>()
                        .UseInMemoryDatabase(Guid.NewGuid().ToString())
                        .Options;

                    services.AddScoped<CatalogContext>(_ => new TestCatalogContext(options));

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();

                    var db = scope.ServiceProvider.GetRequiredService<CatalogContext>();
                    db.Database.EnsureCreated();
                });
        }
    }
}
