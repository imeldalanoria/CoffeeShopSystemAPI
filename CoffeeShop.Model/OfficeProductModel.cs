using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model
{
    public class OfficeProductModel
    {
        [Key]
        public int OfficeID { get; set; }
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        [Display(Name = "Pantry Name")]
        public string PantryName { get; set; }
        public bool HasProduct { get; set; }

        public int ProductID { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public int Unit { get; set; }


    }
}
