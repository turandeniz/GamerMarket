using GamerMarket.Model.Model.Entities;
using GamerMarket.Service.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamerMarket.UI.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        OrderService _orderService;
        OrderDetailService _orderDetailService;
        ProductService _productService;

        public OrderController()
        {
            _orderService = new OrderService();
            _orderDetailService = new OrderDetailService();
            _productService = new ProductService();
        }

        public ActionResult List()
        {
            List<Order> Orders = _orderService.ListPendingOrders();
            if (Orders.Count!=0)
            {
                return View(_orderService.ListPendingOrders());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Detail(Guid id)
        {
            List<OrderDetail> OrderDetails = _orderDetailService.GetDefault(x => x.OrderID == id);
            if (OrderDetails.Count != 0)
            {
                return View(OrderDetails);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

          
        }

        public ActionResult ConfirmOrder(Guid id)
        {
            Order order = _orderService.GetById(id);

            //Stoktan Düşme İşlemi. Her bir sipariş detayda gezilir ve içindeki ürün yakalanır. Sonrasında stoklardan siparişteki miktar kadar düşülür.
            foreach (var item in order.OrderDetails)
            {
                Product p = _productService.GetById(item.ProductID);
                p.UnitsInStock -= item.Quantity;
                _productService.Update(p);
            }

            order.OrderStatus = OrderStatus.Completed;
            _orderService.Update(order);

            return RedirectToAction("List");

        }

        public ActionResult RejectOrder(Guid id)
        {
            Order o = _orderService.GetById(id);
            o.OrderStatus = OrderStatus.Declined;
            _orderService.Remove(o);
            return RedirectToAction("List");
        }
    }
}