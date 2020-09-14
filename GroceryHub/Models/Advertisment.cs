using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class Advertisment
    {
        public int AdvertismentID { get; set; }
        public String AdvertismentName { get; set; }
        public byte[] AdvertismentPhoto { get; set; }
        public String AdvertismentDescription { get; set; }
    }
}
