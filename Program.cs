using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Victorina.Data;
using Victorina.Models;
using Victorina.Services;
using Victorina.Services.Impls;
using Victorina.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

VictorinaHolder victorinaHolder = new VictorinaHolder();
QuizHolder quizHolder = new QuizHolder();

builder.Services.AddSingleton<IVictorinaHolder>(victorinaHolder);
builder.Services.AddSingleton<QuizManager, QuizManager>();
builder.Services.AddSingleton<IQuizHolder>(quizHolder);

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

#region InitData
var rootPath = app.Environment.ContentRootPath;
var json_path = Path.Combine(rootPath, "Data/victorins.json");

using (StreamReader sr = new StreamReader(json_path))
{
    string json = sr.ReadToEnd();
    var models = JsonSerializer.Deserialize<VictoinaModel[]>(json);
    if (models != null)
        victorinaHolder.Init(models.ToList());
}

var q_path = Path.Combine(rootPath, "Data/victorins_data.json");

using (StreamReader sr = new StreamReader(q_path))
{
    string json = sr.ReadToEnd();
    var models = JsonSerializer.Deserialize<QuestionPack[]>(json);
    if (models != null)
        quizHolder.Init(models);
}
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Index}/{id?}");


app.Run();
