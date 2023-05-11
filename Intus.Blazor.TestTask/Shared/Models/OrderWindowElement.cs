using System;
using System.Collections.Generic;
using System.Text;

namespace Intus.Blazor.TestTask.Shared.Models
{
    public class OrderWindowElement:EntityBase
    {
        //OrderWindowElement Table. A order can have multiple Windows and each Windows can have multiple Element
        public int OrderWindowId { get; set; }
        public int ElementID { get; set; }
    }
}
