using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CoffeeShop.EntityFramework;
using CoffeeShop.Model;
using CoffeeShop.Transport;

namespace CoffeeShop.Web.Controllers
{
    public class OrdersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController()
        {
            _unitOfWork = new UnitOfWork(new CoffeeShopContext());
        }


        // GET: api/Orders
        public List<OrderItemModel> GetOrderItemModels()
        {
            //var orders = _unitOfWork.OrderItems.GetOrderByOfficeId(id).Select(o => AutoMapper.Mapper.Map<OrderItemModel>(o)).ToList();
            //return orders;
            throw new NotImplementedException();
        }

        // GET: api/Orders/5
        [ResponseType(typeof(OrderItemModel))]
        public IHttpActionResult GetOrderItemModel(int id)
        {
            var orders = _unitOfWork.OrderItems.GetOrderByOfficeId(id).Select(o => AutoMapper.Mapper.Map<OrderItemModel>(o)).ToList();
            return Ok(orders);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderItemModel(int id, OrderItemModel orderItemModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != orderItemModel.ID)
            //{
            //    return BadRequest();
            //}

            //db.Entry(orderItemModel).State = EntityState.Modified;
            var type = orderItemModel.OrderName;

            OrderItemModel model = new OrderItemModel();
            model.OrderName = type;
            model.ClientName = orderItemModel.ClientName;
            model.OfficeID = id;
            model.AddedDate = DateTime.Now;

            try
            {

                switch (type)
                {
                    case "Double Americano":
                        if (!OrderDoubleAmericano(model))
                        { }
                        //return RedirectToAction("Index", "Order", TempData["msg"] = "<script>alert('Insufficient Product');</script>");
                        break;
                    case "Sweet Latte":
                        if (!OrderSweetLatte(model))
                        { }
                        //return RedirectToAction("Index", "Order", TempData["msg"] = "<script>alert('Insufficient Product');</script>");
                        break;
                    case "Flat White":
                        if (!OrderFlatWhite(model))
                        { }
                        //return RedirectToAction("Index", "Order", TempData["msg"] = "<script>alert('Insufficient Product');</script>");
                        break;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(model);
        }

        public bool OrderDoubleAmericano(OrderItemModel ordermodel)
        {
            return Order(ordermodel, 3, 0, 0);
        }

        private bool OrderFlatWhite(OrderItemModel ordermodel)
        {
            return Order(ordermodel, 2, 1, 4);
        }

        private bool OrderSweetLatte(OrderItemModel ordermodel)
        {
            return Order(ordermodel, 2, 5, 3);
        }

        private bool Order(OrderItemModel ordermodel, int coffeeBeans, int sugar, int milk)
        {
            var result = true;
            var modelCF = AutoMapper.Mapper.Map<ProductModel>(_unitOfWork.Products.GetDataByProductName(ordermodel.OfficeID, "Coffee Beans"));
            var productCoffeeBeansCount = modelCF.Unit;
            var unitCF = _unitOfWork.Products.Get(modelCF.ProductID);
            unitCF.Unit = productCoffeeBeansCount - coffeeBeans;
            if (unitCF.Unit < 0)
            {
                return false;
            }
            _unitOfWork.Products.Update(unitCF);

            var modelSugar = AutoMapper.Mapper.Map<ProductModel>(_unitOfWork.Products.GetDataByProductName(ordermodel.OfficeID, "Sugar"));
            var productSugar = modelSugar.Unit;
            var unitSugar = _unitOfWork.Products.Get(modelSugar.ProductID);
            unitSugar.Unit = productSugar - sugar;
            if (unitSugar.Unit < 0)
            {
                return false;
            }
            _unitOfWork.Products.Update(unitSugar);

            var modelMilk = AutoMapper.Mapper.Map<ProductModel>(_unitOfWork.Products.GetDataByProductName(ordermodel.OfficeID, "Milk"));
            var productMilk = modelMilk.Unit;
            var unitMilk = _unitOfWork.Products.Get(modelMilk.ProductID);
            unitMilk.Unit = productMilk - milk;
            if (unitMilk.Unit < 0)
            {
                return false;
            }
            _unitOfWork.Products.Update(unitMilk);

            var ordermodels = AutoMapper.Mapper.Map<OrderItemModel, OrderItem>(ordermodel);
            _unitOfWork.OrderItems.Add(ordermodels);

            _unitOfWork.Complete();
            return result;
        }

    }
}