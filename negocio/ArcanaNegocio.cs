using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class ArcanaNegocio
    {
        public List<Arcana> listar()
        {
            List<Arcana> lista = new List<Arcana>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select id, descripcion from Arcanas");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Arcana aux = new Arcana((int)datos.Lector["id"], (string)datos.Lector["descripcion"]);

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }


        }
    }
}
