using ServidorAhorcado.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    // Lo que el servidor le empuja al cliente
    [ServiceContract]
    public interface IPartidaCallback
    {
        // Le avisa al Jugador A que el Jugador B se unió
        [OperationContract(IsOneWay = true)]
        void NotificarJugadorUnido(PartidaDTO partida);

        // NUEVO: avisa SOLO al Jugador A (juez) que hay una letra por juzgar
        [OperationContract(IsOneWay = true)]
        void NotificarLetraParaJuzgar(char letra);

        // avisa a AMBOS el resultado YA confirmado
        [OperationContract(IsOneWay = true)]
        void NotificarLetraPropuesta(char letra, bool esCorrecta, char[] progresoPalabra, int intentosFallidos);

        // NUEVO: avisa SOLO al juez que se equivocó en el veredicto
        [OperationContract(IsOneWay = true)]
        void NotificarErrorJuicio(char letra, bool eraCorrecta);

        // Le avisa a ambos jugadores que la partida terminó
        [OperationContract(IsOneWay = true)]
        void NotificarFinPartida(int estadoFinal);
    }

    // Lo que el cliente le pide al servidor
    [ServiceContract(CallbackContract = typeof(IPartidaCallback))]
    public interface IPartidaService
    {
        // Jugador A crea la partida → regresa idPartida o código de error
        [OperationContract]
        int CrearPartida(int idJugador, int idPalabra, string nombrePartida, int idIdioma);

        // Cualquier jugador consulta la lista de partidas en espera
        [OperationContract]
        List<PartidaLobbyDTO> ObtenerPartidasDisponibles();

        // Jugador B se une a una partida → regresa el estado inicial del juego
        [OperationContract]
        PartidaDTO UnirseAPartida(int idPartida, int idJugador);

        // MODIFICADO: ya no evalúa, solo reenvía la letra al juez
        [OperationContract]
        void ProponerLetra(int idPartida, int idJugador, char letra);

        // NUEVO: el juez (Jugador A) envía su veredicto
        [OperationContract]
        void JuzgarLetra(int idPartida, int idJugador, bool decisionEsCorrecta);

        // Cualquiera de los dos abandona → el servidor notifica al otro vía callback
        [OperationContract]
        void AbandonarPartida(int idPartida, int idJugador);
    }
}
