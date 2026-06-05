using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ServidorAhorcado.DTO
{
    [DataContract]
    public class JugadorDTO
    {
        [DataMember]
        public int idJugador { get; set; }

        [DataMember]
        public string nombre { get; set; }

        [DataMember]
        public string primerApellido { get; set; }

        [DataMember]
        public string segundoApellido { get; set; }

        [DataMember]
        public System.DateTime fechaNacimiento { get; set; }

        [DataMember]
        public string telefono { get; set; }

        [DataMember]
        public string contrasena { get; set; }

        [DataMember]
        public string correo { get; set; }

        [DataMember]
        public int puntos { get; set; }

        [DataMember]
        public string usuario { get; set; }
    }
}