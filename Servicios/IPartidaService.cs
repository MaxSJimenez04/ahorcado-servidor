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
    public interface IPartidaCallback
    {
        [OperationContract(IsOneWay = true)]
        void NotificarJugadorUnido(PartidaDTO partida);

        [OperationContract(IsOneWay = true)]
        void NotificarLetraParaJuzgar(char letra);

        [OperationContract(IsOneWay = true)]
        void NotificarLetraPropuesta(char letra, bool esCorrecta, char[] progresoPalabra, int intentosFallidos);

        [OperationContract(IsOneWay = true)]
        void NotificarErrorJuicio(char letra, bool eraCorrecta);

        [OperationContract(IsOneWay = true)]
        void NotificarFinPartida(int estadoFinal);
    }


    [ServiceContract(CallbackContract = typeof(IPartidaCallback))]
    public interface IPartidaService
    {

        [OperationContract]
        int CrearPartida(int idJugador, int idPalabra, string nombrePartida, int idIdioma);

        [OperationContract]
        List<PartidaLobbyDTO> ObtenerPartidasDisponibles();

        [OperationContract]
        PartidaDTO UnirseAPartida(int idPartida, int idJugador);

        [OperationContract(IsOneWay = true)]
        void ProponerLetra(int idPartida, int idJugador, char letra);

        [OperationContract(IsOneWay = true)]
        void JuzgarLetra(int idPartida, int idJugador, bool decisionEsCorrecta);

        [OperationContract]
        void AbandonarPartida(int idPartida, int idJugador);
    }
}
