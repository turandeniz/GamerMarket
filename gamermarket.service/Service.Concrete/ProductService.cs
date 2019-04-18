using GamerMarket.Model.Model.Entities;
using GamerMarket.Service.Service.Abstract;
using GamerMarket.Core.Core.Entity.Enums;
using System.Collections.Generic;

namespace GamerMarket.Service.Service.Concrete
{
    public class ProductService:BaseService<Product>
    {
        public int ProductCount() => GetDefault((x => x.Status == Status.Active || x.Status == Status.Updated)).Count;

        public List<Product> GetWebProducts() => GetDefault(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.DisplayOnWeb == true);

        public List<Product> GetMobileProducts() => GetDefault(x => (x.Status == Status.Active || x.Status == Status.Updated) && x.DisplayOnMobile == true);
    }
}