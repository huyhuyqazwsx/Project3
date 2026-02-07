using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project3.Application.Interfaces;
using Project3.Application.Interfaces.IAuthorization;
using Project3.Application.Interfaces.Websocket;
using Project3.Application.Queues;
using Project3.Application.Services;
using Project3.Application.Services.Authorization;
using Project3.Application.Services.Background;
using Project3.Application.Services.Websocket;
using Project3.Application.Settings;
using Project3.Domain.Interfaces;
using Project3.Infrastructure.Data;
using Project3.Infrastructure.Repository;
using Project3.Middlewares;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(ICrudService<>), typeof(CrudService<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IExamBlueprintService, ExamBlueprintService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IExamGradingService, ExamGradingService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IStudentAuthorizationService, StudentAuthorizationService>();
builder.Services.AddScoped<ITeacherAuthorizationService, TeacherAuthorizationService>();

builder.Services.AddSingleton<IExamAnswerCache, ExamAnswerCache>();
builder.Services.AddSingleton<ExamSubmitQueue>();
builder.Services.AddSingleton<WsSessionManager>();
builder.Services.AddHostedService<ExamAutoSubmitBackgroundService>();
builder.Services.AddHostedService<ExamDraftSaveBackgroundService>();


builder.Services.AddDbContext<ExamSystemDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
));

//Jwt
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();         

app.UseWebSockets();           
app.UseMiddleware<ExamWebSocketMiddleware>();

app.MapControllers();          

app.Run();
