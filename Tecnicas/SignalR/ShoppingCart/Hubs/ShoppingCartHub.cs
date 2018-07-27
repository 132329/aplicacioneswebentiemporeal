﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Model;
using ShoppingCart.Services;
using System;
using System.Threading.Tasks;

namespace ShoppingCart.Hubs
{
    public class ShoppingCartHub : Hub
    {
        private readonly ICheckoutService checkoutService;
        private readonly IMemoryCache cache;

        public ShoppingCartHub(
            ICheckoutService checkoutService,
            IMemoryCache cache)
        {
            this.checkoutService = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task Checkout(Basket basket)
        {
            var checkout = checkoutService.Checkout(basket);
            cache.Set(checkout.OrderId, Context.ConnectionId);
            await Clients.Caller.SendAsync("OnCheckoutDone", checkout);
        }
    }
}
