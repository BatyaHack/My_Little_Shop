using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SportsStore.Models.Entities;

namespace SportsStore.Models.Abstract
{
    //Данный интерфейс слудит для обработки заказа
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
