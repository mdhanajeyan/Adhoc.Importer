using Adhoc.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Pdx.Core.Common;
using Pdx.Core.Data.Dapper;
using Pdx.Core.Data.Dapper.Infrastructure;
using Pdx.Core.Data.Dapper.Managers;
using Pricedex.Pim.Model.ViewModels.Settings;
using static Pdx.Core.Data.Dapper.Managers.PdxDBManager;

namespace Adhoc.Importer
{
    internal class Program
    {
        public static IServiceProvider CurrentProvider { get; internal set; }

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static IConfiguration Configuration { get; set; }

        private static IBrandProcessor? _brandProcessor;
        static void Main(string[] args)
        {
            var builder = AppConfiguration.GetBuilder();
            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services, args);

            CurrentProvider = services.BuildServiceProvider();


            Process(args);
        }

        static void ConfigureServices(IServiceCollection services, string[] args)
        {

            services.AddSingleton(Configuration);
            var connectionString = Configuration.GetConnectionString("PdxConnection");
            services.AddScoped<IDbConnectionFactory, ConnectionFactory>(_ => new ConnectionFactory(connectionString));

            _logger.Info("Connection Established");

            services.AddScoped<AppStartup>();
            services.AddScoped<PdxQueryManager>();
            services.AddScoped<PdxDBManager>();
            services.AddDbContext<Pricedex.Pim.Data.PdxContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<Pdx.Core.Data.PdxCoreContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<Pricedex.Pim.Data.UnitOfWork>();
            services.AddScoped<Pdx.Core.Data.UnitOfWork>();

            services.AddScoped<Pdx.Core.Business.Managers.PdxAppSystemManager>();
            //services.AddScoped<Pricedex.Pim.Data.Factories.LovFactory>();
            //services.AddScoped<Pricedex.Pim.Business.Managers.LovManager>();
            //services.AddScoped<Pricedex.Pim.Data.Factories.CloneFactory>();
            services.AddScoped<Pricedex.Pim.Data.Factories.DeleteFactory>();
            services.AddScoped<Pricedex.Pim.Business.Managers.EntityManager>();
            services.AddScoped<Pricedex.Pim.Data.CommonQueries>();
            services.AddScoped<Pricedex.Pim.Business.Managers.BrandManager>();
            //services.AddScoped<Pricedex.Pim.Business.Managers.DescriptionManager>();
            //services.AddScoped<Pricedex.Pim.Business.Managers.BulkPartManager>();
            //services.AddScoped<Pricedex.Pim.Business.Managers.CompetitorPartManager>();
            //services.AddScoped<Pricedex.Pim.Business.Managers.FinishedGoodsManager>();
            //services.AddScoped<Pricedex.Pim.Business.Managers.OePartManager>();

            //services.AddSingleton<QualifierManager>();
            //services.AddScoped<ApplicationManager>();
            services.AddScoped<PdxJob>();

            services.AddSingleton<AppSettings>();

            CommandLineArguments cla = new CommandLineArguments(args);
            services.AddSingleton(cla);
            services.AddSingleton<Options>();

            _logger.Info("All Job Parameters set in Options");

            services.AddScoped<Common.Importer>();

            services.AddScoped<IBrandProcessor, BrandProcessor>();

            _logger.Info("Dependencies Registered");

        }

        static void Process(string[] args)
        {

            Common.Importer? importer = CurrentProvider.GetService<Common.Importer>();

            _brandProcessor = CurrentProvider.GetService<IBrandProcessor>();

            var dt = importer!.Setup();

            switch (Options.ClassType)
            {
                case "Brand":
                    _brandProcessor!.Process(dt);
                    break;
                default:
                    break;
            }




        }
    }
}