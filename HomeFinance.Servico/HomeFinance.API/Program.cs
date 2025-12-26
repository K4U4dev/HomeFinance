using HomeFinance.Repository.Data;
using HomeFinance.Repository.Interfaces;
using HomeFinance.Repository.Repositories;
using HomeFinance.Service.Interfaces;
using HomeFinance.Service.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Entity Framework com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' não encontrada.");

builder.Services.AddDbContext<HomeFinanceDbContext>(options =>
    options.UseNpgsql(connectionString));

// Registro dos repositórios (padrão Repository)
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

// Registro dos serviços de negócio
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IConsultaService, ConsultaService>();

// Configuração dos controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do CORS para permitir requisições do front-end React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173") // Portas comuns do React
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

// Aplicar migrações do banco de dados automaticamente (apenas em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HomeFinanceDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("Tentando aplicar migrações do banco de dados...");
            dbContext.Database.Migrate();
            logger.LogInformation("Migrações aplicadas com sucesso.");
        }
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, 
            "ERRO: Não foi possível conectar ao banco de dados PostgreSQL. " +
            "Verifique se o PostgreSQL está rodando e se a string de conexão está correta. " +
            "A aplicação continuará, mas as operações de banco de dados falharão.");
        logger.LogWarning(
            "Para resolver:\n" +
            "1. Certifique-se de que o PostgreSQL está instalado e rodando\n" +
            "2. Verifique a string de conexão em appsettings.json\n" +
            "3. Crie o banco de dados: CREATE DATABASE HomeFinanceDB;\n" +
            "4. Execute as migrações manualmente: dotnet ef database update");
    }
}

app.Run();

