using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServidorAhorcado.DTO
{
    [DataContract]
    public class PartidaLobbyDTO
    {
        [DataMember]
        public int idPartida { get; set; }

        [DataMember]
        public string nombrePartida { get; set; }

        [DataMember]
        public string usuarioJugadorA { get; set; }

        [DataMember]
        public string correoJugadorA { get; set; }

        [DataMember]
        public DateTime fechaCreacion { get; set; }
    }
}