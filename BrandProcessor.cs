using Adhoc.Common;
using Adhoc.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Pricedex.Pim.Business.Managers;
using Pricedex.Pim.Model.Entities;
using System.Data;

namespace Adhoc.Importer
{
    public class BrandProcessor : IBrandProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly BrandManager? _brandManager;

        public BrandProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _brandManager = _serviceProvider.GetService<BrandManager>();
        }

        public void Process(DataTable dt)
        {

            BrandSerializer brandSerializer = new();
            Columns columns = BrandSerializer.Deserialize();

            List<Brand> brands = new();

            foreach (DataRow dataRow in dt.Rows)
            {
                int companyId = 0;
                columns.Items.ForEach(col =>
                {
                    var currentValue = dataRow[col.Name];

                    if (col.TargetTable == "dbo.Company")
                    {

                        var companies = _brandManager!.GetAllCompanies().Where(col.TargetColumn, currentValue).ToList();
                        companyId = companies[0].Id;
                    }
                });

                Brand brand = new Brand();

                if (dataRow["Id"] != null && Convert.ToInt32(dataRow["Id"]) > 0)
                {
                    brand.Id = Convert.ToInt32(dataRow["Id"]);

                }
                brand.CompanyId = companyId;
                if (dataRow["Name"] != null && !string.IsNullOrEmpty(dataRow["Name"].ToString()))
                {
                    brand.Name = dataRow["Name"].ToString();

                }

                if (dataRow["Custom"] != null)
                {
                    brand.Custom = Convert.ToBoolean(Convert.ToInt32(dataRow["Custom"]));

                }

                brands.Add(brand);
            }
            if (Options.CreateRecords && Options.UpdateRecords)
                CreateUpdateBrands(brands);
        }

        private void CreateUpdateBrands(List<Brand> brands)
        {
            brands.ForEach(brand =>
            {
                _brandManager!.Save(brand, Options.UserId);
            });
        }

    }
}
