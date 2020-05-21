using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Entities
{
    public class CustomPokemon
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; set; }
        public int Type1 { get; set; }
        public int? Type2 { get; set; }
    }
}
