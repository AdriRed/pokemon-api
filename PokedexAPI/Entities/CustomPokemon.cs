using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Entities
{
    public class CustomPokemon
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public byte[] Image { get; set; }
    }
}
