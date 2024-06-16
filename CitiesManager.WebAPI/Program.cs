using CitiesManager.Core.Identity;
using CitiesManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
	options.Filters.Add(new ProducesAttribute("application/json"));
	options.Filters.Add(new ConsumesAttribute("application/json"));
})
	.AddXmlSerializerFormatters();

builder.Services.AddApiVersioning(config =>
{
	config.ApiVersionReader = new UrlSegmentApiVersionReader();
	// config.ApiVersionReader = new QueryStringApiVersionReader(); // Read version number from query string called 'api-version'
	// config.ApiVersionReader = new HeaderApiVersionReader("api-version"); // Read version number from header called 'api-version'
	config.DefaultApiVersion = new ApiVersion(1, 0);
	config.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

	options.SwaggerDoc("v1", new OpenApiInfo { Title = "CitiesManager Web API", Version = "1.0" });
	options.SwaggerDoc("v2", new OpenApiInfo { Title = "CitiesManager Web API", Version = "2.0" });
});

builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true; 
});

// CORS: localhost:4200, localhost:4100 
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policyBuilder =>
	{
		policyBuilder
		.WithOrigins(builder.Configuration
			.GetSection("AllowedOrigins")
			.Get<string[]>())
		.WithHeaders("Authorization", "origin", "accept", "content-type")
		.WithMethods("GET", "POST", "PUT", "DELETE");
	});

	options.AddPolicy("4100Client", policyBuilder =>
	{
		policyBuilder
		.WithOrigins(builder.Configuration
			.GetSection("AllowedOrigins2")
			.Get<string[]>())
		.WithHeaders("Authorization", "origin", "accept")
		.WithMethods("GET");
	});
});

//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
	options.Password.RequiredLength = 5;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = true;
	options.Password.RequireDigit = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>()
;

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
	options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
