using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAP.NET6.Services;

namespace SAP.Controllers
{
    [Authorize]
    [Route("orders")]
    public class OrdersController : Controller
    {
        IOrderService OrderService { get; }

        public OrdersController(IOrderService orderService)
        {
            OrderService = orderService;
        }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var orders = await OrderService.GetUserOrdersAsync();
            return View(orders);
        }

        [Route("details")]
        public async Task<ActionResult> Details(Guid id)
        {
            var order = await OrderService.GetOrderAsync(id);
            return View(order);
        }

        [Route("create")]
        public async Task<ActionResult> Create()
        {
            try
            {
                await OrderService.CreateOrderFromCartWithValidationAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                // Handle the exception and show an error message to the user
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("cancel/{id}")]
        [HttpGet]
        public ActionResult Cancel(Guid id)
        {
            return View();
        }

        [Route("cancel")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}