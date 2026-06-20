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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class PartidaService : IPartidaService
    {
        public int CrearPartida(int idJugador, int idPalabra, string nombrePartida, int idIdioma)
        {
            try
            {
                var db = new AhorcadoEntities();

                bool nombreOcupado = db.Partida.Any(p => p.NombrePartida == nombrePartida);
                if (nombreOcupado)
                {
                    return -1;
                }

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

                Palabra palabraDB = db.Palabra.Find(idPalabra);
                Jugador jugadorDB = db.Jugador.Find(idJugador);
                Categoria categoriaDB = db.Categoria.Find(palabraDB.CategoriaId);
                string nombreCategoria = idIdioma == 1 ? categoriaDB.CategoriaES : categoriaDB.CategoriaEN;
                string palabraObjetivo = idIdioma == 1 ? palabraDB.PalabraES : palabraDB.PalabraEN;
                string descripcion = idIdioma == 1 ? palabraDB.DescripcionES : palabraDB.DescripcionEN;

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
                    categoriaPalabra = nombreCategoria,
                    estadoId = 1
                };

                GestorPartidas.AgregarPartida(nuevaPartida.IdPartida, partidaMemoria);

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

                Partida partida = db.Partida.Find(idPartida);
                if (partida == null || partida.EstadoId != 1)
                {
                    return null;
                }

                if (partida.JugadorAId == idJugador)
                {
                    return null;
                }

                Jugador jugadorB = db.Jugador.Find(idJugador);

                partida.JugadorBId = idJugador;
                partida.EstadoId = 2;
                db.SaveChanges();

                if (!GestorPartidas.TryObtenerPartida(idPartida, out PartidaDTO partidaMemoria))
                {
                    return null;
                }

                partidaMemoria.idJugadorB = idJugador;
                partidaMemoria.usuarioJugadorB = jugadorB.Usuario;
                partidaMemoria.estadoId = 2;

                var callback = OperationContext.Current.GetCallbackChannel<IPartidaCallback>();
                CallbackManager.RegistrarCallback(idJugador, callback);

                if (CallbackManager.TryObtenerCallback(partidaMemoria.idJugadorA, out var callbackJugadorA))
                {
                    callbackJugadorA.NotificarJugadorUnido(partidaMemoria);
                }

                PartidaDTO partidaParaB = new PartidaDTO
                {
                    idPartida = partidaMemoria.idPartida,
                    nombrePartida = partidaMemoria.nombrePartida,
                    idJugadorA = partidaMemoria.idJugadorA,
                    usuarioJugadorA = partidaMemoria.usuarioJugadorA,
                    idJugadorB = partidaMemoria.idJugadorB,
                    usuarioJugadorB = partidaMemoria.usuarioJugadorB,
                    palabraObjetivo = null,   
                    descripcionPalabra = partidaMemoria.descripcionPalabra,
                    progresoPalabra = partidaMemoria.progresoPalabra,
                    letrasUsadas = partidaMemoria.letrasUsadas,
                    intentosFallidos = partidaMemoria.intentosFallidos,
                    idIdioma = partidaMemoria.idIdioma,
                    categoriaPalabra = partidaMemoria.categoriaPalabra,
                    estadoId = partidaMemoria.estadoId
                };

                return partidaParaB;
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

                char letraMayuscula = char.ToUpper(letra);

                if (partida.letrasUsadas.Contains(letraMayuscula))
                    return;

                if (partida.hayLetraPendiente)
                    return;

                partida.letraPendiente = letraMayuscula;
                partida.hayLetraPendiente = true;

                if (CallbackManager.TryObtenerCallback(partida.idJugadorA, out var cbA))
                {
                    cbA.NotificarLetraParaJuzgar(letraMayuscula);
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

                if (partida.estadoId == 1)
                {
                    CancelarPartidaEnEspera(idPartida, partida);
                    return;
                }

                int estadoFinal = idJugador == partida.idJugadorA ? 5 : 6;

                FinalizarPartida(idPartida, partida, estadoFinal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void CancelarPartidaEnEspera(int idPartida, PartidaDTO partida)
        {
            try
            {
                var db = new AhorcadoEntities();
                Partida partidaDB = db.Partida.Find(idPartida);

                if (partidaDB != null)
                {
                    db.Partida.Remove(partidaDB);
                    db.SaveChanges();
                }

                CallbackManager.EliminarCallback(partida.idJugadorA);
                GestorPartidas.EliminarPartida(idPartida);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void JuzgarLetra(int idPartida, int idJugador, bool decisionEsCorrecta)
        {
            try
            {
                if (!GestorPartidas.TryObtenerPartida(idPartida, out PartidaDTO partida))
                    return;

                if (idJugador != partida.idJugadorA)
                    return;


                if (!partida.hayLetraPendiente)
                    return;

                char letra = partida.letraPendiente;
                string palabraUpper = partida.palabraObjetivo.ToUpper();
                bool esRealmenteCorrecta = palabraUpper.Contains(letra);

                if (decisionEsCorrecta != esRealmenteCorrecta)
                {

                    if (CallbackManager.TryObtenerCallback(partida.idJugadorA, out var cbError))
                    {
                        cbError.NotificarErrorJuicio(letra, esRealmenteCorrecta);
                    }
                    return;
                }

                partida.letrasUsadas.Add(letra);

                if (esRealmenteCorrecta)
                {
                    for (int i = 0; i < palabraUpper.Length; i++)
                    {
                        if (palabraUpper[i] == letra)
                            partida.progresoPalabra[i] = partida.palabraObjetivo[i];
                    }
                }
                else
                {
                    partida.intentosFallidos++;
                }

                partida.hayLetraPendiente = false;
                partida.letraPendiente = '\0';

                NotificarAmbos(partida, letra, esRealmenteCorrecta);

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

                    Jugador jugadorA = db.Jugador.Find(partida.idJugadorA);
                    Jugador jugadorB = db.Jugador.Find(partida.idJugadorB);

                    switch (estadoFinal)
                    {
                        case 3:
                            jugadorB.Puntos += 10;
                            break;
                        case 4:
                            jugadorA.Puntos += 5;
                            break;
                        case 5:
                            jugadorA.Puntos -= 3;
                            break;
                        case 6:
                            jugadorB.Puntos -= 3;
                            break;
                    }

                    db.SaveChanges();
                }

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

                GestorPartidas.EliminarPartida(idPartida);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
