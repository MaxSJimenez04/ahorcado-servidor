using ServidorAhorcado.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    [ServiceContract]
    public interface IUsuarioService
    {
        [OperationContract]
        int RegistrarJugador(JugadorDTO nuevoJugador);

        [OperationContract]
        int ActualizarJugador(JugadorDTO datosActualizados);

        [OperationContract]
        JugadorDTO ObtenerDatosJugador(string usuario);
        
    }
}
