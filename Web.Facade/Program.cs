// Copyright (c) Fedor Bashilov. All rights reserved.

namespace Web.Facade
{
    using Microsoft.AspNetCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHost(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}