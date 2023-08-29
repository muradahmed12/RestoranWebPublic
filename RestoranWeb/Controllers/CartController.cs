﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoranWeb.Data;
using RestoranWeb.Handlers;
using RestoranWeb.Models;

namespace RestoranWeb.Controllers
{
    [Authorized]
    public class CartController : Controller
    {
        private AppDbContext _context;
        private string loggedInUserId;

        public CartController(AppDbContext context)
        {
            _context = context;
            loggedInUserId = _context.GetLoggedInUser()?.Id;
        }


        [Authorized]
        public IActionResult Index()
        {
   
            return View();
        }

        [HttpPost]
        public IActionResult GetCartItems()
        {
            System.Threading.Thread.Sleep(250);
            if (string.IsNullOrEmpty(loggedInUserId))
            {
                return Json(new { Status = false, Msg = "Login is requried." });
            }

            var cart = _context.ShoppingCart.Include(m => m.CartItems).Where(m => m.UserId == loggedInUserId).FirstOrDefault();
            if (cart == null)
            {
                cart = new ShoppingCart { UserId = loggedInUserId, CartItems = new() };
                _context.ShoppingCart.Add(cart);
                _context.SaveChanges();
            }

            var productIds = cart.CartItems.Select(m => m.FoodItemId).ToList();
            var products = _context.FoodItem.Where(m => productIds.Contains(m.Id))
                .Select(m => new
                {
                    ProductId = m.Id,
                    m.Name,
                    ImageUrl = m.ImageUrl,
                    m.Price,
                    CategoryName = m.Type.Name
                }).ToList();

            var result = products.Select(m => new
            {
                m.ProductId,
                m.Name,
                m.CategoryName,
                m.Price,
                m.ImageUrl,
                Qty = cart.CartItems.Where(i => i.FoodItemId == m.ProductId).Select(q => q.Quantity).FirstOrDefault()
            }).ToList();

            return Json(new { Status = true, Data = result });
        }
        [HttpPost]
        public IActionResult AddOrUpdateCart(string id, int qty = 1, bool isUpdate = false)
        {
            System.Threading.Thread.Sleep(250);
            var product = _context.FoodItem.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return Json(new { Status = false, Msg = "Product not found for id " + id });
            }

            var cart = _context.ShoppingCart.Include(m => m.CartItems).Where(m => m.UserId == loggedInUserId).FirstOrDefault();
            if (cart == null)
            {
                cart = new ShoppingCart { UserId = loggedInUserId, CartItems = new() };
                _context.ShoppingCart.Add(cart);
                _context.SaveChanges();
            }

            var existingItem = cart.CartItems.Where(m => m.FoodItemId == id).FirstOrDefault();
            if (existingItem != null)
            {
                if (isUpdate) existingItem.Quantity = qty;
                else existingItem.Quantity += qty;
                _context.SaveChanges();
            }
            else
            {
                CartItem currentItem = new() { FoodItemId = id, Quantity = qty, ShoppingCartId = cart.Id };
                cart.CartItems.Add(currentItem);

                _context.Add(currentItem);
                _context.SaveChanges();
            }

            //var d = _context.Carts.Where(m => m.UserId == loggedInUserId).SelectMany(m => m.CartItems.Select(i => i.Product))
            //    .Select(m => new { })
            //    .ToList();

            var productIds = cart.CartItems.Select(m => m.FoodItemId).ToList();
            var products = _context.FoodItem.Where(m => productIds.Contains(m.Id))
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    ImageUrl = m.ImageUrl,
                    m.Price,
                    CategoryName = m.Type.Name
                }).ToList();

            var result = products.Select(m => new
            {
                ProductId = m.Id,
                m.Name,
                m.CategoryName,
                m.Price,
                m.ImageUrl,
                Qty = cart.CartItems.Where(i => i.FoodItemId == m.Id).Select(q => q.Quantity).FirstOrDefault()
            }).ToList();
            TempData["message"] = "Successfully Added to Cart";
            return Json(new { Status = true, Data = result });
        }


   

        [HttpPost]
        public IActionResult DeleteItem(string id)
        {
            var product = _context.FoodItem.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return Json(new { Status = false, Msg = "Product not found for id " + id });
            }

            var cart = _context.ShoppingCart.Include(m => m.CartItems).Where(m => m.UserId == loggedInUserId).FirstOrDefault();

            var existingItem = cart.CartItems.Where(m => m.FoodItemId == id).FirstOrDefault();
            if (existingItem != null)
            {
                _context.Remove(existingItem);
            }
            _context.SaveChanges();

            TempData["delete"] = "Successfully deleted from Cart";
            return GetCartItems();
        }

    }
}
