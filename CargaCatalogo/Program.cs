using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Serilog;
using Serilog.Core;

namespace CargaCatalogo
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.json");
                var configuration = builder.Build();

                MongoClient client = new MongoClient(
                    configuration.GetConnectionString("ConexaoCatalogo"));
                IMongoDatabase db = client.GetDatabase("DBCatalogo");

                logger.Information("Incluindo produtos...");
                var catalogoProdutos = db.GetCollection<Produto>("Catalogo");

                catalogoProdutos.InsertOne(new Produto()
                {
                    Codigo = "PROD00001",
                    Nome = "Detergente",
                    Tipo = "Limpeza",
                    Preco = 5.75,
                    DadosFornecedor = new Fornecedor()
                    {
                        Codigo = "FORN00001",
                        Nome = "EMPRESA XYZ"
                    }
                });

                catalogoProdutos.InsertOne(new Produto()
                {
                    Codigo = "PROD00002",
                    Nome = "Martelo",
                    Tipo = "Ferramentas",
                    Preco = 50.70,
                    DadosFornecedor = new Fornecedor()
                    {
                        Codigo = "FORN00002",
                        Nome = "ABCD FERRAMENTAS"
                    }
                });

                logger.Information("Incluindo serviços...");
                var catalogoServicos = db.GetCollection<Servico>("Catalogo");

                catalogoServicos.InsertOne(new Servico()
                {
                    Codigo = "SERV00001",
                    Nome = "Limpeza Predial",
                    ValorHora = 150.00
                });

                catalogoServicos.InsertOne(new Servico()
                {
                    Codigo = "SERV00002",
                    Nome = "Guarda Patrimonial",
                    ValorHora = 250.00
                });

                logger.Information("Finalizado!");
            }
            catch (Exception ex)
            {
                logger.Error(
                    $"Ocorreu um erro e a aplicação será encerrada: {ex.Message}");
            }
        }
    }
}