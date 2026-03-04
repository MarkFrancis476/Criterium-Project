using CriteriumBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Permitir que el Frontend se conecte sin bloqueos (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 1. Agregar el servicio de MongoDB (Ya lo tenías)
builder.Services.AddSingleton<AssignmentsService>();

// 2. Agregar soporte para Controladores (¡ESTO ES NUEVO!)
builder.Services.AddControllers();

// Configuración de OpenAPI/Swagger (Para probar la API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

// 3. Activar los Controladores (¡ESTO ES NUEVO!)
app.MapControllers();

app.Run();