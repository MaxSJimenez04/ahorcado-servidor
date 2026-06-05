using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServidorAhorcado.DTO
{
    [DataContract]
    public class CategoriaDTO
    {
        [DataMember]
        public int idCategoria { get; set; }

        [DataMember]
        public string categoriaES { get; set; }

        [DataMember]
        public string categoriaEN { get; set; }
    }
}