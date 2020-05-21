using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Models
{
    public class CustomPokemonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public int Type1 { get; set; }
        public int? Type2 { get; set; }
        public string Owner { get; set; }
    }
}
