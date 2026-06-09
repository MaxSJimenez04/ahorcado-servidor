using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServidorAhorcado.DTO
{
    [DataContract]
    public class PartidaDTO
    {
        [DataMember]
        public int idPartida { get; set; }

        [DataMember]
        public string nombrePartida { get; set; }

        [DataMember]
        public int idJugadorA { get; set; }

        [DataMember]
        public string usuarioJugadorA { get; set; }

        [DataMember]
        public int idJugadorB { get; set; }

        [DataMember]
        public string usuarioJugadorB { get; set; }

        [DataMember]
        public string palabraObjetivo { get; set; }

        [DataMember]
        public string descripcionPalabra { get; set; }

        [DataMember]
        public char[] progresoPalabra { get; set; }

        [DataMember]
        public List<char> letrasUsadas { get; set; }

        [DataMember]
        public int intentosFallidos { get; set; }

        [DataMember]
        public int idIdioma { get; set; }

        [DataMember]
        public int estadoId { get; set; }
    }
}