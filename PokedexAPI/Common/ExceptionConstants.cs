using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI
{
    public enum ExceptionConstants : byte
    {
        BAD_LOGIN,
        USER_NOT_FOUND,
        BAD_REGISTER
    }

    public static class Literals
    {
        public const string CODE = "CODE";
    }


    [Serializable]
    public class PokemonAPIException : Exception
    {
        public byte Code { get; private set; }
        public PokemonAPIException() { }
        public PokemonAPIException(string message) : base(message) { }
        public PokemonAPIException(string message, Exception inner) : base(message, inner) { }
        public PokemonAPIException(string message, byte code) : this(message) 
        {
            Code = code;
        }

        public PokemonAPIException(string message, ExceptionConstants code) : this(message, (byte)code)
        {
        }
        protected PokemonAPIException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
