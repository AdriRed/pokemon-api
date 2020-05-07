using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Models
{
    public class ErrorModel
    {
        public int InnerCode { get; set; }
        public string Reason { get; set; }
    }
}
