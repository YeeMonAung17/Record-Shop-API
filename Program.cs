
using Microsoft.EntityFrameworkCore;
using Record_Shop.Data;
using System;

namespace Record_Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            // Register  DbContext — uses in-memory when in Development
            builder.Services.AddDbContext<RecordDbContext>(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.UseInMemoryDatabase("MyAppDevDb");
                }
                else
                {
                    //  real DB connection string goes here for Production
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
            });


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

            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
