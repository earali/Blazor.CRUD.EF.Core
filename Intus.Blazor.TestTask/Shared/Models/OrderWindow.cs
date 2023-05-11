using System;
using System.Collections.Generic;
using System.Text;

namespace Intus.Blazor.TestTask.Shared.Models
{
    public class OrderWindow:EntityBase
    {
        // OrderWindow Table. A order can have multiple Windows.
        public int OrderId { get; set; }
        public int WindowID { get; set; }
        public int Quantity { get; set; }
    }
}
