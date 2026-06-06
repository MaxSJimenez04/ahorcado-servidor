using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServidorAhorcado.DTO
{
    [DataContract]
    public class EstadisticasDTO
    {
        [DataMember]
        public int posicion {  get; set; }
        [DataMember]
        public string usuario { get; set; }
        [DataMember]
        public int puntos { get; set; }
    }
}