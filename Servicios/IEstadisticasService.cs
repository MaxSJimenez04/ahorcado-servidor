using ServidorAhorcado.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IEstadisticasService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IEstadisticasService
    {
        [OperationContract]
        List<EstadisticasDTO> ObtenerClasificacionPuntos();

        [OperationContract]
        List<EstadisticasDTO> ObtenerClasificacionVictorias();

        [OperationContract]
        EstadisticasDTO ObtenerEstadisticaUsuario(int idUsuario, int puntosOrVictorias);

        [OperationContract]
        List<HistorialDTO> ObtenerHistorial(int jugadorID, int idIdioma);

        
    }
}
