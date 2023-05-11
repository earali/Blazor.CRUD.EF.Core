using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Intus.Blazor.TestTask.Shared.Models
{
    public class Windows:EntityBase
    {
        //Window Table
        [Required]
        public string WindowName { get; set; }
    }
}
