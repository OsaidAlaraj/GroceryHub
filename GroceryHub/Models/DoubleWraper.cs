using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class DoubleWraper
    {
        public int DoubleWraperID { get; set; }
        public double num { get; set; }
        public FlyerOffer FlyerOffer { get; set; }
    }
}
