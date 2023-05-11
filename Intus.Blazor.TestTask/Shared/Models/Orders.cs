using System;
using System.Collections.Generic;
using System.Text;

namespace Intus.Blazor.TestTask.Shared.Models
{
    public class Orders:EntityBase
    {
        // Order Table
        public string OrderName { get; set; }
        public string State { get; set; }
        public string FullAddress { get; set; }
    }
}
