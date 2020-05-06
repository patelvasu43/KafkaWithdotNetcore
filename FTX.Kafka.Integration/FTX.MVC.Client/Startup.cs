using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using FTX.Kafka.Integration.Configuration;
using FTX.Kafka.Integration.Events;
using FTX.Kafka.Integration.Kafka;
using FTX.Kafka.Integration.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FTX.MVC.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TestEntityFContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            

            var producerConfig = new ProducerConfig();
            Configuration.Bind("Producer", producerConfig);
            services.AddSingleton(producerConfig);
            services.AddSingleton<KafkaProducer>();

            var consumerConfig = new ConsumerConfiguration();
            Configuration.Bind("Consumer", consumerConfig);
            services.AddSingleton(consumerConfig);

            services.AddHostedService<EventManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
