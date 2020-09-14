using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryHub.Models
{
    public class ByteWraper
    {
        public int Id { get; set; }
        public byte[] photo { get; set; }
        [ForeignKey("FlyerReader")]
        public int FlyerReaderId { get; set; }
        public FlyerReader FlyerReader { get; set; }
    }
}
