# .NET6-Template-DevStandard

Standard Pattern WebAPI
- .NET6
- Asynchronous
- Repository Pattern
- Autofac
- Swagger
- Jwt
- Serilog
- Elmahlog
- MailSMTP
- AD

## Example
![net6-api-swgger](/net6_template_devstandard_api/wwwroot/Img/Swagger.JPG)

## Installation

package install
- Dapper
- System.Data.SqlClient
- Autofac.Extensions.DependencyInjection
- System.DirectoryServices.AccountManagement
- Swashbuckle.AspNetCore
- elmahcore
- Microsoft.AspNetCore.Authentication.JwtBearer
- System.IdentityModel.Tokens.Jwt
- Serilog.AspNetCore
- Serilog.Sinks.File
- Serilog.Sinks.Seq

## 1. Setting DB
1. run query db ../wwwroot/DeploySript/Script-MovieStore.sql
2. เพิ่มโค้ดส่วนนี้ในไฟล์ appsetting.json
```bash
"ConnectionStrings": {
    "AppDB": "data source=(localdb)\\MSSQLLocalDB;initial catalog=MovieStore;Integrated Security=True;"
  }
```
## 2. Setting Autofac
เพิ่มโค้ดส่วนนี้ในไฟล์ Program.cs
```bash
//Now register our services with Autofac container
// Call UseServiceProviderFactory on the Host sub property 
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Call ConfigureContainer on the Host sub property 
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    var serviceAssembly = typeof(MovieService).Assembly;
    builder.RegisterAssemblyTypes(serviceAssembly).Where(t => t.Name.EndsWith("Service"))
    .AsImplementedInterfaces()
    .SingleInstance();

    var repositoryAssembly = typeof(MovieRepository).Assembly;
    builder.RegisterAssemblyTypes(repositoryAssembly).Where(t => t.Name.EndsWith("Repository"))
    .AsImplementedInterfaces()
    .SingleInstance();
});
```
## 3. Setting Elmahlog
เพิ่มโค้ดส่วนนี้ในไฟล์ Program.cs
```bash
builder.Services.AddElmah(options =>
{
    //options.CheckPermissionAction = context => context.User.Identity.IsAuthenticated;
    options.Path = @"elmah";
});
```
```bash
app.UseElmah();
```
## 4. Setting Jwt
เพิ่มโค้ดส่วนนี้ในไฟล์ appsetting.json
```bash
  "JwtBearer": {
    "JwtKey": "C1CF4B7DC4C4175B6618DE4F55CA4",
    "JwtIssuer": "net6demo",
    "JwtAudience": "net6demo",
    "JwtExpireDays": 1
  }
```
เพิ่มโค้ดส่วนนี้ในไฟล์ Program.cs
```bash
// ===== Add Jwt Authentication ========
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = Configuration["JwtBearer:JwtIssuer"],
            ValidAudience = Configuration["JwtBearer:JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtBearer:JwtKey"])),
            ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
    });
```
```bash
// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();
```
## 5. Setting Swagger Auth
แก้ไขโค้ดในไฟล์ Program.cs ของ Medthod นี้ builder.Services.AddSwaggerGen()
```bash
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});
```

## 6. Setting Serilog
เพิ่มโค้ดส่วนนี้ในไฟล์ appsetting.json
```bash
  "Serilog": {
    "Using": [ "Serilog.Sinks.File" ],
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "../logs/webapi-.log",
          "rollingInterval": "Day",
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {CorrelationId} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
```
เพิ่มโค้ดส่วนนี้ในไฟล์ Program.cs
```bash
//setting serilog
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
```

## 7. Other
เพิ่มโค้ดส่วนนี้ในไฟล์ appsetting.json
- AD
```bash
"LdapDomain": "domain_xxx"
```
- MailSMTP
```bash
   "MailServerConfiguration": {
    "Host": "10.0.0.xx",
    "Port": xx,
    "MailTemp": "D:\\MailTemp",
    "MailFrom": "net6@mail.com",
    "MailTo": "net6@mail.com",
    "IsStatus": false
  }
```

## License
[MIT](https://choosealicense.com/licenses/mit/)