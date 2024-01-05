using ShopMate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Core.Interfaces
{
    public interface IOrderService
    {
        public Task<int> CreateOrderAsync(int userId, OrderInput orderInput);
    }
}
