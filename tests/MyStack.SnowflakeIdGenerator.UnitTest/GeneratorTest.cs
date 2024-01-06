using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;

namespace MyStack.SnowflakeIdGenerator.UnitTest
{
    public class GeneratorTest
    {
        public IServiceProvider ServiceProvider { get; private set; }
        [SetUp]
        public void Setup()
        {
            var builder = new HostBuilder()
              .ConfigureHostConfiguration(configure =>
              {
                  configure.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json");
              })
              .ConfigureServices((context, services) =>
              {
                  //services.AddSnowflakeId(context.Configuration);
                  // or  
                  services.AddSnowflakeId(configure =>
                  {
                      configure.GroupId = context.Configuration.GetValue<ushort>("SnowflakeIdGenerator:GroupId");
                      configure.MachineId = context.Configuration.GetValue<ushort>("SnowflakeIdGenerator:MachineId");
                  });
              });

            var app = builder.Build();
            ServiceProvider = app.Services;
        }
        [Test]
        public void Generate()
        {
            var snowflakeId = ServiceProvider.GetRequiredService<ISnowflakeId>();
            var id = snowflakeId.NewId();
            Assert.IsTrue(id > 0);
        }
        [Test]
        public void ParallelForGenerate()
        {
            var snowflakeId = ServiceProvider.GetRequiredService<ISnowflakeId>();
            ConcurrentDictionary<long, int> keyValuePairs = new ConcurrentDictionary<long, int>();
            Parallel.For(0, 100, i =>
            {
                var id = snowflakeId.NewId();
                keyValuePairs.TryAdd(id, i);
            });
            Assert.IsTrue(keyValuePairs.Count == 100);
        }
    }
}