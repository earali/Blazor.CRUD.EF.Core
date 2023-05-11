using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Intus.Blazor.TestTask.Shared.Models
{
    public class Elements:EntityBase
    {
        //Element Table
        [Required]
        public string ElementName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
    }
}
