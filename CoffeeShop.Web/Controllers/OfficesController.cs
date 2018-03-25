using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using CoffeeShop.EntityFramework;
using CoffeeShop.Model;
using CoffeeShop.Transport;

namespace CoffeeShop.Web.Controllers
{
    public class OfficesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public OfficesController()
        {
            _unitOfWork = new UnitOfWork(new CoffeeShopContext());
        }

        // GET: api/Offices
        public List<OfficeModel> GetOfficeModels()
        {
            var offices = _unitOfWork.Offices.GetAll().Select(o => AutoMapper.Mapper.Map<OfficeModel>(o)).ToList();
            return offices;
        }

        // GET: api/Offices/5
        [ResponseType(typeof(OfficeProductModel))]
        public IHttpActionResult GetOfficeModel(int id)
        {
            var product =  _unitOfWork.Products.GetDataByOfficeId(id).Select(o => AutoMapper.Mapper.Map<ProductModel>(o)).ToList();
            return Ok(product);
        }

        // POST: api/Offices
        [ResponseType(typeof(OfficeModel))]
        public IHttpActionResult PostOfficeModel(OfficeModel officeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var model = AutoMapper.Mapper.Map<OfficeModel, Office>(officeModel);
            _unitOfWork.Offices.Add(model);
            _unitOfWork.Complete();
            return CreatedAtRoute("DefaultApi", new { id = officeModel.OfficeID }, model);
        }
    }
}