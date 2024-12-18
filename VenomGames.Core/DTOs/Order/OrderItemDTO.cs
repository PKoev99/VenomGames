﻿namespace VenomGames.Core.DTOs.Order
{
    public class OrderItemDTO
    {
        public int GameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
