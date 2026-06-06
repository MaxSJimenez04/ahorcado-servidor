using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServidorAhorcado.DTO
{
    [DataContract]
    public class HistorialDTO
    {
        [DataMember]
        public string palabra { get; set; }

        [DataMember]
        public int puntos { get; set; }

        [DataMember]
        public string usuarioContrincante { get; set; }

        [DataMember]
        public DateTime fechaPartida { get; set; }

        [DataMember]
        public int estadoPartida { get; set; }
    }
}