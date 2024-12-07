using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Order;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService _orderService)
        {
            orderService = _orderService;
        }

        // GET: /Orders
        public async Task<IActionResult> Index(GetOrdersQuery query)
        {
            IEnumerable<OrderOutputModel> orders = await orderService.GetOrdersAsync(query);
            return View(orders);
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            OrderOutputModel? order = await orderService.GetOrderDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: /Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateDTO order)
        {
            if (ModelState.IsValid)
            {
                await orderService.CreateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: /Orders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            OrderOutputModel? order = await orderService.GetOrderDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: /Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderUpdateDTO order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await orderService.UpdateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: /Orders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            OrderOutputModel? order = await orderService.GetOrderDetailsAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: /Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await orderService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}