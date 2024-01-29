using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            //var dbUser = configuration.GetValue<string>("DatabaseSettings:username");
            //var dbPass = configuration.GetValue<string>("DatabaseSettings:password ");
            //MongoCredential credential = MongoCredential.CreateCredential(mechanism: "SCRAM-SHA-1", configuration.GetValue<string>("DatabaseSettings:DatabaseName"), "admin", "pass");

//            var settings = new MongoClientSettings
//            {
//                Credential = credential,
            
//            };


//            MongoCredential credential = MongoCredential.FromComponents(
//    mechanism: "SCRAM-SHA-1",
//    source: configuration.GetValue<string>("DatabaseSettings:DatabaseName"),
//    username: "admin",
//    password: "pass"
//);


//            var authenticatedClient = new MongoClient(settings);

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            //bool existProduct = Products.Find(p => true).Any();
            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
