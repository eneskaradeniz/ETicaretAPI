﻿namespace ETicaretAPI.Application.DTOs.Order
{
    public class SingleOrder
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string OrderCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Completed { get; set; }

        public object BasketItems { get; set; }
    }
}
