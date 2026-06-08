
using Microsoft.EntityFrameworkCore;
using Record_Shop.Data;
using Record_Shop.Middleware;
using Record_Shop.Repositories;
using Record_Shop.Services;
using System;

namespace Record_Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddTransient<ExceptionHandlerMiddleware>();


            // Register  DbContext — uses in-memory when in Development

            builder.Services.AddDbContext<RecordDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlerMiddleware>();


            app.MapControllers();

            app.Run();
        }
    }
}
