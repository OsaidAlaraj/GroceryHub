using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryHub.Models
{
    public class FlyerOffer
    {
        [Key]
        public int FlyerOfferId { get; set; }
        public String ItemName { get; set; }
        public Double Price { get; set; }
        public String Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int StoreID { get; set; }
        public byte[] flyerphoto { get; set; }
        public bool recognized { get; set; }
        public List<DoubleWraper> Numbers { get; set; }
        [ForeignKey("FlyerReader")]
        public FlyerReader flyerReader { get; set; }
    }
}
