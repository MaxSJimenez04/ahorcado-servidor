using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServidorAhorcado.DTO
{
    [DataContract]
    public class PalabraDTO
    {
        [DataMember]
        public  int idPalabra {  get; set; }

        [DataMember]
        public string palabraES { get; set; }
            
        [DataMember]
        public string palabraEN { get; set; }

        [DataMember]
        public string descripcionES { get; set; }

        [DataMember]
        public string descripcionEN {  get; set; }

        [DataMember]
        public int categoriaId { get; set; } 
    }
}