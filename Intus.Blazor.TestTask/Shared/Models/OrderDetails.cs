using System;
using System.Collections.Generic;
using System.Text;

namespace Intus.Blazor.TestTask.Shared.Models
{
    public class OrderDetails:EntityBase
    {
        //It is not a DB table. Created it for mapping the Data and use in Order View page, Order Creation page and Order Edit page.
        public string OrderName { get; set; }
        public string State { get; set; }
        public string FullAddress { get; set; }

        public int ElementID { get; set; }
        public int Quantity { get; set; }
        public int WindowID { get; set; }
        public int OrderId { get; set; }
        public string WindowName { get; set; }
        public string ElementName { get; set; }
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
