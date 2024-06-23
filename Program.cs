using CloudinaryDotNet;
using dotenv.net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using postgresTest.Model;

// Cloudinary set up
// ======================

DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));

Account account = new Account(
  Environment.GetEnvironmentVariable("CLOUDINARY_NAME"),
  Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY"),
  Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
);
// Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
Cloudinary cloudinary = new Cloudinary(account);
cloudinary.Api.Secure = true;
Console.WriteLine("checking cloudinary");
Console.WriteLine(cloudinary);
Console.WriteLine("\n");

// ======================

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
  options.AddPolicy(
    name: MyAllowSpecificOrigins,
    policy =>
    {
      policy.WithOrigins(["http://localhost:5173"])
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials()
      // .SetIsOriginAllowed((host) => true)
      ;

    });
});

builder.Services.AddSingleton(cloudinary);
builder.Services.AddControllers();


var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
// AddIdentityCore might need to come after AddDbContext
builder.Services.AddIdentityCore<User>()
// don't know if this will be useful
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// builder.Services.AddIdentity<User>();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Swagger support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
  config.DocumentName = "a";
  config.Title = "lovely rita";
  config.Version = "v1";
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}
else
{
  app.UseOpenApi();
  app.UseSwaggerUi(config =>
  {
    config.DocumentTitle = "a";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
  });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();


app.MapGet("/", () =>
{
  Console.WriteLine("hi");
});

app.MapIdentityApi<User>();
app.MapControllers();

app.Run();
