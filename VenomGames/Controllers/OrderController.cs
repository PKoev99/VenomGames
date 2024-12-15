using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VenomGames.Core.Contracts;
using VenomGames.Core.DTOs.Order;
using VenomGames.Infrastructure.Data.Models;

namespace VenomGames.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService _orderService, IShoppingCartService _shoppingCartService, UserManager<ApplicationUser> _userManager)
            :base(_shoppingCartService,_userManager)
        {
            orderService = _orderService;
        }

        // GET: /Orders
        public async Task<IActionResult> Index()
        {
            await SetCartItemCountAsync();

            IEnumerable<OrderOutputModel> orders = await orderService.GetOrdersAsync();
            return View(orders);
        }

        // GET: /Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await orderService.GetOrderDetailsAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var orderOutputModel = new OrderOutputModel
            {
                Id = order.Id,
                UserId = order.UserId,
                Username = order.Username,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                GameOrders = order.GameOrders.Select(go => new OrderItemDTO
                {
                    GameName = go.GameName,
                    Price = go.Price,
                    Quantity = go.Quantity
                }).ToList()
            };

            return View(orderOutputModel);
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