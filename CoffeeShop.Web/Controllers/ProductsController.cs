using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CoffeeShop.EntityFramework;
using CoffeeShop.Model;
using CoffeeShop.Transport;

namespace CoffeeShop.Web.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController()
        {
            _unitOfWork = new UnitOfWork(new CoffeeShopContext());
        }

        // GET: api/Products
        public List<ProductModel> GetProducts()
        {
            var products = _unitOfWork.Products.GetAll().Select(o => AutoMapper.Mapper.Map<ProductModel>(o)).ToList();
            return products;
        }

        // POST: api/Products
        [ResponseType(typeof(OfficeProductModel))]
        public  IHttpActionResult PostProduct(OfficeProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = AutoMapper.Mapper.Map<OfficeProductModel, Product>(productModel);
            _unitOfWork.Products.Add(model);

            OfficeModel office = new OfficeModel();
            office.OfficeID = productModel.OfficeID;
            office.OfficeName = productModel.OfficeName;
            office.PantryName = productModel.PantryName;
            office.HasProduct = true;
            var models = AutoMapper.Mapper.Map<OfficeModel, Office>(office);

            _unitOfWork.Offices.Update(models);
            _unitOfWork.Complete();

            return CreatedAtRoute("DefaultApi", new { id = productModel.ProductID }, productModel);
        }
    }
}