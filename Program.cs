using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Scalar.AspNetCore;
using WebAppBlogApi.Core.Entities;
using WebAppBlogApi.Core.IRepository;
using WebAppBlogApi.Infrastructure.Data.RavenDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar o RavenDB
var store = new DocumentStore
{
    Urls = ["http://your-ravendb-url"],
    Database = "YourDatabaseName"
};

store.Initialize();

// Registrar IAsyncDocumentSession
builder.Services.AddScoped<IAsyncDocumentSession>(sp => store.OpenAsyncSession());

//Registrar os repositórios
//builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICommentRepository, CommentRepository>();
//builder.Services.AddScoped<IImageRepository, ImageRepository>();
//builder.Services.AddScoped<IPostRepository, PostRepository>();
//builder.Services.AddScoped<ITagRepository, TagRepository>();

// Registrar RavenRoleStore e RavenUserStore
builder.Services.AddScoped<IRoleStore<IdentityRole>, RavenRoleStore>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, RavenUserStore>();

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
