using Record_Shop.AI.Services;
using Microsoft.EntityFrameworkCore;
using Record_Shop.Data;
using Record_Shop.Middleware;
using Record_Shop.Repositories;
using Record_Shop.Services;

namespace Record_Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<ExceptionHandlerMiddleware>();

            builder.Services.AddDbContext<RecordDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();

            var useMockAi = builder.Configuration.GetValue<bool>("AzureAi:UseMock");

            if (useMockAi)
            {
                builder.Services.AddScoped<IChatService, MockChatService>();
            }
            else
            {
                builder.Services.AddScoped<IChatService, ChatService>();
            }

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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