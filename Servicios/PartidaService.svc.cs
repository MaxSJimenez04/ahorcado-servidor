using ServidorAhorcado.DTO;
using ServidorAhorcado.Modelo;
using ServidorAhorcado.Utilidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "PartidaService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione PartidaService.svc o PartidaService.svc.cs en el Explorador de soluciones e inicie la depuración.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class PartidaService : IPartidaService
    {
        public int CrearPartida(int idJugador, int idPalabra, string nombrePartida, int idIdioma)
        {
            try
            {
                var db = new AhorcadoEntities();

                // Verificar que el nombre de partida no esté en uso
                bool nombreOcupado = db.Partida.Any(p => p.NombrePartida == nombrePartida);
                if (nombreOcupado)
                {
                    // Estado -1: nombre de partida ya existe
                    return -1;
                }

                // Registrar la partida en BD con EstadoId = 1 (En espera)
                Partida nuevaPartida = new Partida
                {
                    NombrePartida = nombrePartida,
                    FechaCreacion = DateTime.Now,
                    EstadoId = 1,
                    JugadorAId = idJugador,
                    PalabraId = idPalabra,
                    IdiomaId = idIdioma
                };

                db.Partida.Add(nuevaPartida);
                db.SaveChanges();

                // Obtener la palabra para guardarla en memoria
                Palabra palabraDB = db.Palabra.Find(idPalabra);
                Jugador jugadorDB = db.Jugador.Find(idJugador);

                string palabraObjetivo = idIdioma == 1 ? palabraDB.PalabraES : palabraDB.PalabraEN;
                string descripcion = idIdioma == 1 ? palabraDB.DescripcionES : palabraDB.DescripcionEN;

                // Crear el estado inicial de la partida en memoria
                PartidaDTO partidaMemoria = new PartidaDTO
                {
                    idPartida = nuevaPartida.IdPartida,
                    nombrePartida = nombrePartida,
                    idJugadorA = idJugador,
                    usuarioJugadorA = jugadorDB.Usuario,
                    palabraObjetivo = palabraObjetivo,
                    descripcionPalabra = descripcion,
                    progresoPalabra = new string('_', palabraObjetivo.Length).ToCharArray(),
                    letrasUsadas = new List<char>(),
                    intentosFallidos = 0,
                    idIdioma = idIdioma,
                    estadoId = 1
                };

                GestorPartidas.AgregarPartida(nuevaPartida.IdPartida, partidaMemoria);

                // Registrar el callback del Jugador A
                var callback = OperationContext.Current.GetCallbackChannel<IPartidaCallback>();
                CallbackManager.RegistrarCallback(idJugador, callback);

                return nuevaPartida.IdPartida;
            }
            catch (EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return -2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -2;
            }
        }

        public List<PartidaLobbyDTO> ObtenerPartidasDisponibles()
        {
            try
            {
                var db = new AhorcadoEntities();
                var partidas = db.Partida
                    .Where(p => p.EstadoId == 1)
                    .Select(p => new
                    {
                        p.IdPartida,
                        p.NombrePartida,
                        p.FechaCreacion,
                        JugadorA = db.Jugador.FirstOrDefault(j => j.IdJugador == p.JugadorAId)
                    }).ToList();

                List<PartidaLobbyDTO> lista = new List<PartidaLobbyDTO>();
                foreach (var p in partidas)
                {
                    lista.Add(new PartidaLobbyDTO
                    {
                        idPartida = p.IdPartida,
                        nombrePartida = p.NombrePartida,
                        usuarioJugadorA = p.JugadorA.Usuario,
                        correoJugadorA = p.JugadorA.Correo,
                        fechaCreacion = (DateTime)p.FechaCreacion
                    });
                }

                return lista;
            }
            catch (EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public PartidaDTO UnirseAPartida(int idPartida, int idJugador)
        {
            try
            {
                var db = new AhorcadoEntities();

                // Verificar que la partida sigue disponible
                Partida partida = db.Partida.Find(idPartida);
                if (partida == null || partida.EstadoId != 1)
                {
                    return null;
                }

                // Verificar que no sea el mismo jugador que creó la partida
                if (partida.JugadorAId == idJugador)
                {
                    return null;
                }

                Jugador jugadorB = db.Jugador.Find(idJugador);

                // Actualizar BD: asignar Jugador B y cambiar estado a En curso
                partida.JugadorBId = idJugador;
                partida.EstadoId = 2;
                db.SaveChanges();

                // Actualizar estado en memoria
                if (!GestorPartidas.TryObtenerPartida(idPartida, out PartidaDTO partidaMemoria))
                {
                    return null;
                }

                partidaMemoria.idJugadorB = idJugador;
                partidaMemoria.usuarioJugadorB = jugadorB.Usuario;
                partidaMemoria.estadoId = 2;

                // Registrar callback del Jugador B
                var callback = OperationContext.Current.GetCallbackChannel<IPartidaCallback>();
                CallbackManager.RegistrarCallback(idJugador, callback);

                // Notificar al Jugador A que el Jugador B se unió
                if (CallbackManager.TryObtenerCallback(partidaMemoria.idJugadorA, out var callbackJugadorA))
                {
                    callbackJugadorA.NotificarJugadorUnido(jugadorB.Usuario);
                }

                return partidaMemoria;
            }
            catch (EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void ProponerLetra(int idPartida, int idJugador, char letra)
        {
            try
            {
                if (!GestorPartidas.TryObtenerPartida(idPartida, out PartidaDTO partida))
                    return;

                // Evitar letras ya usadas
                char letraMayuscula = char.ToUpper(letra);
                if (partida.letrasUsadas.Contains(letraMayuscula))
                    return;

                partida.letrasUsadas.Add(letraMayuscula);

                // Verificar si la letra está en la palabra
                string palabraUpper = partida.palabraObjetivo.ToUpper();
                bool esCorrecta = palabraUpper.Contains(letraMayuscula);

                if (esCorrecta)
                {
                    // Revelar la letra en todas sus posiciones
                    for (int i = 0; i < palabraUpper.Length; i++)
                    {
                        if (palabraUpper[i] == letraMayuscula)
                        {
                            partida.progresoPalabra[i] = partida.palabraObjetivo[i];
                        }
                    }
                }
                else
                {
                    partida.intentosFallidos++;
                }

                // Notificar a ambos jugadores
                NotificarAmbos(partida, letraMayuscula, esCorrecta);

                // Verificar condiciones de fin
                bool palabraCompleta = !partida.progresoPalabra.Contains('_');
                bool ahorcadoCompleto = partida.intentosFallidos >= 6;

                if (palabraCompleta || ahorcadoCompleto)
                {
                    int estadoFinal = palabraCompleta ? 3 : 4;
                    FinalizarPartida(idPartida, partida, estadoFinal);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AbandonarPartida(int idPartida, int idJugador)
        {
            try
            {
                if (!GestorPartidas.TryObtenerPartida(idPartida, out PartidaDTO partida))
                    return;

                // Estado 5: abandonó el creador (JugadorA)
                // Estado 6: abandonó el adivinador (JugadorB)
                int estadoFinal = idJugador == partida.idJugadorA ? 5 : 6;

                FinalizarPartida(idPartida, partida, estadoFinal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // =====================
        // Métodos privados
        // =====================

        private void NotificarAmbos(PartidaDTO partida, char letra, bool esCorrecta)
        {
            if (CallbackManager.TryObtenerCallback(partida.idJugadorA, out var cbA))
            {
                cbA.NotificarLetraPropuesta(letra, esCorrecta, partida.progresoPalabra, partida.intentosFallidos);
            }

            if (CallbackManager.TryObtenerCallback(partida.idJugadorB, out var cbB))
            {
                cbB.NotificarLetraPropuesta(letra, esCorrecta, partida.progresoPalabra, partida.intentosFallidos);
            }
        }

        private void FinalizarPartida(int idPartida, PartidaDTO partida, int estadoFinal)
        {
            try
            {
                var db = new AhorcadoEntities();
                Partida partidaDB = db.Partida.Find(idPartida);

                if (partidaDB != null)
                {
                    partidaDB.EstadoId = estadoFinal;
                    partidaDB.FechaFin = DateTime.Now;

                    // Actualizar puntos según el resultado
                    Jugador jugadorA = db.Jugador.Find(partida.idJugadorA);
                    Jugador jugadorB = db.Jugador.Find(partida.idJugadorB);

                    switch (estadoFinal)
                    {
                        case 3: // Ganó el adivinador (JugadorB)
                            jugadorB.Puntos += 10;
                            break;
                        case 4: // Ganó el creador (JugadorA)
                            jugadorA.Puntos += 5;
                            break;
                        case 5: // Abandonó el creador (JugadorA)
                            jugadorA.Puntos -= 3;
                            break;
                        case 6: // Abandonó el adivinador (JugadorB)
                            jugadorB.Puntos -= 3;
                            break;
                    }

                    db.SaveChanges();
                }

                // Notificar a ambos jugadores el fin de la partida
                if (CallbackManager.TryObtenerCallback(partida.idJugadorA, out var cbA))
                {
                    cbA.NotificarFinPartida(estadoFinal);
                    CallbackManager.EliminarCallback(partida.idJugadorA);
                }

                if (CallbackManager.TryObtenerCallback(partida.idJugadorB, out var cbB))
                {
                    cbB.NotificarFinPartida(estadoFinal);
                    CallbackManager.EliminarCallback(partida.idJugadorB);
                }

                // Limpiar la partida de memoria
                GestorPartidas.EliminarPartida(idPartida);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
