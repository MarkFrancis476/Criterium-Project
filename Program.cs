using CriteriumBackend.Models; // 👈 Crucial para corregir el error CriteriumDatabaseSettings
using CriteriumBackend.Services; // 👈 Crucial para reconocer AssignmentsService
using CloudinaryDotNet; 

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CONFIGURACIÓN DE CLOUDINARY (LA NUBE)
// ==========================================
var cloudinaryAccount = new Account(
    "dbquxjibn",           // Tu Cloud Name
    "431769186783491",     // Tu API Key
    "6A6BJC6bDNghU2KwsFDlkNZjDTM"   // ⚠️ REEMPLAZA ESTO CON TU SECRET COMPLETO
);
var cloudinary = new Cloudinary(cloudinaryAccount);
builder.Services.AddSingleton(cloudinary); 

// ==========================================
// 2. CONFIGURACIÓN DE TU BASE DE DATOS
// ==========================================
builder.Services.Configure<CriteriumDatabaseSettings>(
    builder.Configuration.GetSection("CriteriumDatabase"));

builder.Services.AddSingleton<AssignmentsService>();

// ==========================================
// 3. SERVICIOS BASE DEL SISTEMA
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Habilitar CORS para que tu celular pueda hablar con la laptop
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();