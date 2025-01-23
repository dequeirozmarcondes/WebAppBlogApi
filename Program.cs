using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Scalar.AspNetCore;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Core.IRepository;
using WebAppBlogApi.Infrastructure.Data.RavenDb;
using WebAppBlogApi.Infrastructure.Data.Repository;
using WebAppBlogApi.Application.Services;
using WebAppBlogApi.Application.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Método auxiliar para carregar o certificado
static X509Certificate2 LoadCertificate(string path)
{
    if (string.IsNullOrWhiteSpace(path))
    {
        throw new ArgumentException("O caminho do certificado não pode ser nulo ou vazio.", nameof(path));
    }

    var certType = X509Certificate2.GetCertContentType(path);

    if (certType == X509ContentType.Pfx || certType == X509ContentType.Authenticode)
    {
#pragma warning disable SYSLIB0057 // O tipo ou membro é obsoleto
        return new X509Certificate2(path);
#pragma warning restore SYSLIB0057 // O tipo ou membro é obsoleto
    }

    throw new CryptographicException($"O tipo do certificado no caminho especificado ({certType}) não é compatível.");
}

// Configure RavenDB with client certificate
builder.Services.AddSingleton<IDocumentStore>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var path = configuration["RavenDB:CertificatePath"];

    if (string.IsNullOrEmpty(path))
    {
        throw new ArgumentNullException(nameof(path), "O caminho do certificado não pode ser nulo ou vazio.");
    }

    // Carrega o certificado usando o método auxiliar
    X509Certificate2 certificate = LoadCertificate(path);

    var store = new DocumentStore
    {
        Urls = ["https://a.dequeirozmarcondes.ravendb.community"],
        Database = "Blog",
        Certificate = certificate
    };
    store.Initialize();
    return store;
});

// Registro do IAsyncDocumentSession
builder.Services.AddScoped<IAsyncDocumentSession>(provider =>
    provider.GetRequiredService<IDocumentStore>().OpenAsyncSession());

//Registrar os repositórios
builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICommentRepository, CommentRepository>();
//builder.Services.AddScoped<IImageRepository, ImageRepository>();
//builder.Services.AddScoped<IPostRepository, PostRepository>();
//builder.Services.AddScoped<ITagRepository, TagRepository>();

// Registrar RavenRoleStore e RavenUserStore
builder.Services.AddScoped<IRoleStore<IdentityRole>, RavenRoleStore>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, RavenUserStore>();

// Registrar os serviços
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();

// Configurar Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
