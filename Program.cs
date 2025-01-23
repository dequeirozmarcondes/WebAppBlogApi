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

// Método auxiliar para carregar o certificado
static X509Certificate2 LoadCertificate(string path, ILogger logger)
{
    if (string.IsNullOrWhiteSpace(path))
    {
        // Gera uma exceção se o caminho do certificado estiver vazio ou nulo
        throw new ArgumentException("O caminho do certificado não pode ser nulo ou vazio.", nameof(path));
    }

    try
    {
        // Obtém o tipo de conteúdo do certificado
        var certType = X509Certificate2.GetCertContentType(path);

        // Verifica se o tipo do certificado NÃO é compatível
        if (certType != X509ContentType.Pfx && certType != X509ContentType.Authenticode)
        {
            // Gera uma exceção se o tipo do certificado não for compatível
            throw new CryptographicException($"O tipo do certificado no caminho especificado ({certType}) não é compatível.");
        }

        // Carrega o certificado a partir do caminho fornecido
        return new X509Certificate2(path);

    }
    catch (Exception ex)
    {
        // Registra o erro e lança a exceção
        logger.LogError(ex, "Erro ao carregar o certificado do caminho: {Path}", path);
        throw;
    }
}

// Método de extensão para configurar RavenDB
static void ConfigureRavenDB(IServiceCollection services, IConfiguration configuration, ILogger logger)
{
    var path = configuration["RavenDB:CertificatePath"];

    if (string.IsNullOrEmpty(path))
    {
        // Gera uma exceção se o caminho do certificado não for especificado
        throw new ArgumentException("O caminho do certificado não foi especificado.", nameof(path));
    }

    // Carrega o certificado usando o método auxiliar
    var certificate = LoadCertificate(path, logger);

    var store = new DocumentStore
    {
        // Configura as URLs e o banco de dados do RavenDB
        Urls = ["https://a.dequeirozmarcondes.ravendb.community"],
        Database = "Blog",
        Certificate = certificate
    };
    store.Initialize();

    // Adiciona o DocumentStore e o IAsyncDocumentSession aos serviços
    services.AddSingleton<IDocumentStore>(store);
    services.AddScoped<IAsyncDocumentSession>(provider => store.OpenAsyncSession());
}

// Configurar os serviços
static void ConfigureServices(IServiceCollection services, IConfiguration configuration, ILogger logger)
{
    // Configura o RavenDB
    ConfigureRavenDB(services, configuration, logger);

    // Registrar repositórios
    services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
    //services.AddScoped<ICategoryRepository, CategoryRepository>();
    //services.AddScoped<ICommentRepository, CommentRepository>();
    //services.AddScoped<IImageRepository, ImageRepository>();
    //services.AddScoped<IPostRepository, PostRepository>();
    //services.AddScoped<ITagRepository, TagRepository>();

    // Registrar RavenRoleStore e RavenUserStore
    services.AddScoped<IRoleStore<IdentityRole>, RavenRoleStore>();
    services.AddScoped<IUserStore<ApplicationUser>, RavenUserStore>();

    // Registrar serviços de aplicação
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

// Configurar serviços
ConfigureServices(builder.Services, builder.Configuration, logger);

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
