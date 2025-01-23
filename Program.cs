using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Scalar.AspNetCore;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Core.IRepository;
using WebAppBlogApi.Infrastructure.Data.Repository;
using WebAppBlogApi.Application.Services;
using WebAppBlogApi.Application.IServices;
using WebAppBlogApi.Infrastructure.Data.RavenDb;

var builder = WebApplication.CreateBuilder(args);

// M�todo auxiliar para carregar o certificado
static X509Certificate2 LoadCertificate(string path, ILogger logger)
{
    if (string.IsNullOrWhiteSpace(path))
    {
        // Gera uma exce��o se o caminho do certificado estiver vazio ou nulo
        throw new ArgumentException("O caminho do certificado n�o pode ser nulo ou vazio.", nameof(path));
    }

    try
    {
        // Obt�m o tipo de conte�do do certificado
        var certType = X509Certificate2.GetCertContentType(path);

        // Verifica se o tipo do certificado N�O � compat�vel
        if (certType != X509ContentType.Pfx && certType != X509ContentType.Authenticode)
        {
            // Gera uma exce��o se o tipo do certificado n�o for compat�vel
            throw new CryptographicException($"O tipo do certificado no caminho especificado ({certType}) n�o � compat�vel.");
        }

        // Carrega o certificado a partir do caminho fornecido
        return new X509Certificate2(path);

    }
    catch (Exception ex)
    {
        // Registra o erro e lan�a a exce��o
        logger.LogError(ex, "Erro ao carregar o certificado do caminho: {Path}", path);
        throw;
    }
}

// M�todo de extens�o para configurar RavenDB
static void ConfigureRavenDB(IServiceCollection services, IConfiguration configuration, ILogger logger)
{
    var path = configuration["RavenDB:CertificatePath"];

    if (string.IsNullOrEmpty(path))
    {
        // Gera uma exce��o se o caminho do certificado n�o for especificado
        throw new ArgumentException("O caminho do certificado n�o foi especificado.", nameof(path));
    }

    // Carrega o certificado usando o m�todo auxiliar
    var certificate = LoadCertificate(path, logger);

    var store = new DocumentStore
    {
        // Configura as URLs e o banco de dados do RavenDB
        Urls = ["https://a.dequeirozmarcondes.ravendb.community"],
        Database = "Blog",
        Certificate = certificate
    };
    store.Initialize();

    // Adiciona o DocumentStore e o IAsyncDocumentSession aos servi�os
    services.AddSingleton<IDocumentStore>(store);
    services.AddScoped<IAsyncDocumentSession>(provider => store.OpenAsyncSession());
}

// Configurar os servi�os
static void ConfigureServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
{
    // Configura o RavenDB
    ConfigureRavenDB(services, configuration, logger);

    // Registrar reposit�rios
    services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
    //services.AddScoped<ICategoryRepository, CategoryRepository>();
    //services.AddScoped<ICommentRepository, CommentRepository>();
    //services.AddScoped<IImageRepository, ImageRepository>();
    //services.AddScoped<IPostRepository, PostRepository>();
    //services.AddScoped<ITagRepository, TagRepository>();

    // Registrar RavenRoleStore e RavenUserStore
    services.AddScoped<IRoleStore<IdentityRole>, RavenRoleStore>();
    services.AddScoped<IUserStore<ApplicationUser>, RavenUserStore>();

    // Registrar servi�os de aplica��o
    services.AddScoped<IApplicationUserService, ApplicationUserService>();

    // Configurar Identity
    services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddDefaultTokenProviders();

    // Adicionar controladores e OpenAPI
    services.AddControllers();
    services.AddOpenApi();
}

// Configurar o logger
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Program>();

// Configurar servi�os
ConfigureServices(builder.Services, builder.Configuration, logger);

var app = builder.Build();

// Configurar o pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
