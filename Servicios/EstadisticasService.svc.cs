using ServidorAhorcado.DTO;
using ServidorAhorcado.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    public class EstadisticasService : IEstadisticasService
    {
        List<EstadisticasDTO> IEstadisticasService.ObtenerClasificacionPuntos()
        {
            try
            {
                var db = new AhorcadoEntities();
                var clasificacion = db.Jugador.OrderByDescending(j => j.Puntos).Take(25).ToList();
                List<EstadisticasDTO> posiciones = new List<EstadisticasDTO>();
                int posicion = 1;

                foreach (var c in clasificacion)
                {
                    EstadisticasDTO estadisticas = new EstadisticasDTO
                    {
                        posicion = posicion,
                        usuario = c.Usuario,
                        puntos = c.Puntos
                    };

                    posiciones.Add(estadisticas);
                    posicion++;
                }

                return posiciones;
            }
            catch(EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return null; 
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        List<EstadisticasDTO> IEstadisticasService.ObtenerClasificacionVictorias()
        {
            try
            {
                var db = new AhorcadoEntities();
                var clasificacion = db.Jugador.Select(j => new
                {
                    j.Usuario,
                    CantidadVictorias= db.Partida.Count(p => 
                    (p.EstadoId == 3 && p.JugadorBId == j.IdJugador) || (p.EstadoId == 4 && p.JugadorAId == j.IdJugador))
                }).ToList();

                var clasificacionOrdenada = clasificacion.OrderByDescending(c => c.CantidadVictorias);

                List<EstadisticasDTO> posiciones = new List<EstadisticasDTO>();
                int lugar = 1;
                foreach (var c in clasificacionOrdenada)
                {
                    EstadisticasDTO estadisticas = new EstadisticasDTO
                    {
                        posicion = lugar,
                        usuario = c.Usuario,
                        puntos = c.CantidadVictorias
                    };

                    posiciones.Add(estadisticas);
                    lugar++;
                }
                return posiciones;
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

        EstadisticasDTO IEstadisticasService.ObtenerEstadisticaUsuario(int idUsuario, int puntosOrVictorias)
        {
            try
            {
                var db = new AhorcadoEntities();
                switch (puntosOrVictorias)
                {
                    case 1:
                        var clasificacion = db.Jugador.OrderByDescending(j => j.Puntos).ToList();
                        int posicionJugador = 1;
                        foreach (var c in clasificacion)
                        {
                            if (c.IdJugador == idUsuario)
                            {
                                EstadisticasDTO estadisticasJugador = new EstadisticasDTO
                                {
                                    posicion = posicionJugador,
                                    usuario = c.Usuario,
                                    puntos = c.Puntos
                                };
                                return estadisticasJugador;
                            }
                            
                            posicionJugador++;
                        }
                        break;
                    case 2:
                        var clasificacionVictorias = db.Jugador.Select(j => new
                        {
                            j.IdJugador,
                            j.Usuario,
                            Victorias = db.Partida.Count(p => 
                            (p.EstadoId == 3 && j.IdJugador == p.JugadorBId) || (p.EstadoId == 4 && j.IdJugador == p.JugadorAId))
                        }).ToList();
                        var clasificacionVictoriasOrdenada = clasificacionVictorias.OrderByDescending(c => c.Victorias);
                        int posicionVictoriasJugador = 1;
                        foreach (var c in clasificacionVictoriasOrdenada)
                        {
                            if (c.IdJugador == idUsuario)
                            {
                                EstadisticasDTO estadisticasJugador = new EstadisticasDTO
                                {
                                    posicion = posicionVictoriasJugador,
                                    usuario = c.Usuario,
                                    puntos = c.Victorias
                                };
                                return estadisticasJugador;
                            }

                            posicionVictoriasJugador++;
                        }
                        break;
                    default: 
                        break;
                }
                return null;
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

        List<HistorialDTO> IEstadisticasService.ObtenerHistorial(int jugadorID)
        {
            try
            {
                var db = new AhorcadoEntities();
                var historial = db.Partida.Where(p=> p.JugadorAId == jugadorID || p.JugadorBId == jugadorID).Select(p => new
                {
                    p.Palabra,
                    Resultado = p.Estado,
                    JugadorA = db.Jugador.FirstOrDefault(j => j.IdJugador == p.JugadorAId),
                    JugadorB = db.Jugador.FirstOrDefault(j => j.IdJugador == p.JugadorBId),
                    p.FechaFin,
                    p.IdiomaId
                }).ToList();

                List<HistorialDTO> historialPartidas = new List<HistorialDTO>();
                foreach (var h in historial)
                {
                    HistorialDTO partida = new HistorialDTO
                    {
                        palabra = obtenerPalabraEnIdiomaJugado(h.Palabra, (int)h.IdiomaId),
                        usuarioContrincante = obtenerContrincante(h.JugadorA, h.JugadorB, jugadorID),
                        puntos = puntosObtenidos(h.Resultado.IdEstado, jugadorID, h.JugadorA),
                        fechaPartida = (DateTime)h.FechaFin,
                        estadoPartida = h.Resultado.IdEstado
                    };

                    historialPartidas.Add(partida);
                }

                return historialPartidas;
            }
            catch (EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            } catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private string obtenerContrincante(Jugador jugadorA, Jugador jugadorB, int idJugador)
        {
            string contrincante = "";
           if(jugadorA.IdJugador == idJugador)
            {
                contrincante = jugadorB.Usuario;
                return contrincante;
            }
            else
            {
                contrincante = jugadorA.Usuario;
                return contrincante;
            }
        }

        private string obtenerPalabraEnIdiomaJugado(Palabra palabraOrigen, int idIdioma)
        {
            string palabra = "";
            switch (idIdioma)
            {
                case 1:
                    palabra = palabraOrigen.PalabraES;
                    return palabra;
                case 2:
                    palabra = palabraOrigen.PalabraEN;
                    return palabra;
                default:
                    palabra = palabraOrigen.PalabraES;
                    return palabra;
            }
        }

        private int puntosObtenidos(int idEstado, int idJugador, Jugador jugadorAnfitrion) 
        {
            if (idEstado == 3 && idJugador != jugadorAnfitrion.IdJugador)
            {
                return 10;
            }

            if (idEstado == 4 && idJugador == jugadorAnfitrion.IdJugador)
            {
                return 5;
            }

            if (idEstado == 5 && jugadorAnfitrion.IdJugador != idJugador)
            {
                return -3;
            }

            return 0;
        }


    }
}
