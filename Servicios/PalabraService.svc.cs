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
    public class PalabraService : IPalabraService
    {
        List<CategoriaDTO> IPalabraService.ObtenerCategorias()
        {
            try
            {
                var db = new AhorcadoEntities();
                var categorias = db.Categoria.Select(j => j);
                List<CategoriaDTO> listaCategorias = new List<CategoriaDTO>();
                foreach (var categoria in categorias)
                {
                    CategoriaDTO c = new CategoriaDTO
                    {
                        idCategoria = categoria.IdCategoria,
                        categoriaEN = categoria.CategoriaEN,
                        categoriaES = categoria.CategoriaES
                    };
                    listaCategorias.Add(c);
                }

                return listaCategorias;
            }
            catch(EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
        }

        PalabraDTO IPalabraService.ObtenerDatosPalabra(int palabraID)
        {
            try
            {
                var db = new AhorcadoEntities();
                Palabra datosPalabra = db.Palabra.Find(palabraID);
                PalabraDTO palabra = new PalabraDTO
                {
                    idPalabra = datosPalabra.IdPalabra,
                    palabraES = datosPalabra.PalabraES,
                    palabraEN = datosPalabra.PalabraEN,
                    descripcionES = datosPalabra.DescripcionES,
                    descripcionEN = datosPalabra.DescripcionEN
                };
                return palabra;
            }
            catch (EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        List<PalabraDTO> IPalabraService.ObtenerPalabrasPorCategoria(int categoriaId)
        {
            try
            {
                var db = new AhorcadoEntities();
                var lista = db.Palabra.Where(p => p.CategoriaId == categoriaId);
                List<PalabraDTO> listaPalabras = new List<PalabraDTO>();
                foreach (var p in lista)
                {
                    PalabraDTO palabra = new PalabraDTO()
                    {
                        idPalabra = p.IdPalabra,
                        palabraES = p.PalabraES,
                        palabraEN = p.PalabraEN,
                        descripcionEN = p.DescripcionEN,
                        descripcionES = p.DescripcionES,
                        categoriaId = categoriaId
                    };

                   listaPalabras.Add(palabra);
                }

                return listaPalabras;
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

        
    }
}
