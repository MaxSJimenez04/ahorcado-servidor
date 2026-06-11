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
        void NotificarJugadorUnido(string usuarioJugadorB);

        // Le avisa a ambos jugadores el resultado de una letra propuesta
        [OperationContract(IsOneWay = true)]
        void NotificarLetraPropuesta(char letra, bool esCorrecta, char[] progresoPalabra, int intentosFallidos);

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

        // Jugador B propone una letra → el servidor notifica a ambos vía callback
        [OperationContract]
        void ProponerLetra(int idPartida, int idJugador, char letra);

        // Cualquiera de los dos abandona → el servidor notifica al otro vía callback
        [OperationContract]
        void AbandonarPartida(int idPartida, int idJugador);
    }
}
