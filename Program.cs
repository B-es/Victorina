using System.Text.Json;
using Victorina.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Index}/{id?}");

var rootPath = app.Environment.ContentRootPath;

var json_path = Path.Combine(rootPath, "Data/victorins.json");
using (StreamReader sr = new StreamReader(json_path))
{
	string json = sr.ReadToEnd();
	var models = JsonSerializer.Deserialize<VictoinaModel[]>(json);
	if (models != null)
		VictorinaManager.GetInstance().Init(models.ToList());
}

var q_path = Path.Combine(rootPath, "Data/victorins_data.json");
using (StreamReader sr = new StreamReader(q_path))
{
	string json = sr.ReadToEnd();
	var models = JsonSerializer.Deserialize<QuestionApi[]>(json);
	if (models != null)
		QuizManager.GetInstance().Init(models);
}

app.Run();
