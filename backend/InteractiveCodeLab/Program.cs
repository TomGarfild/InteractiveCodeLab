using InteractiveCodeLab.MappingConfigs;
using Mapster;
using Serilog;
using InteractiveCodeLab.Application;
using InteractiveCodeLab.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//builder.Services.AddAuthentication()
//    .AddCookie("cookie")
//    .AddOAuth("github", o =>
//    {
//        o.SignInScheme = "cookie";
//        o.ClientId = builder.Configuration["GitHub:ClientId"]!;
//        o.ClientSecret = builder.Configuration["GitHub:ClientSecret"]!;

//        o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
//        o.TokenEndpoint = "https://github.com/login/oauth/access_token";
//        o.CallbackPath = "/oauth/github-cb";

//        o.UserInformationEndpoint = "https://api.github.com/user";
//    })
//    .AddGoogle("google", o =>
//    {

//    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEnd",
        policy => { policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000").AllowCredentials(); });
});

AlgorithmConfig.Configure();

builder.Services.AddServices();
builder.Services.AddInfraServices(builder.Configuration);

builder.Services.AddMapster();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("FrontEnd");
app.MapControllers();

app.Run();