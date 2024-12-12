﻿namespace VenomGames.Core.DTOs.CartItem
{
    public class CartItemOutputModel
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
    }
}
