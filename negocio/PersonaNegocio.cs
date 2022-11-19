using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class PersonaNegocio
    {
        AccesoDatos datos = new AccesoDatos();

        public List<Persona> listar()
        {
            List<Persona> lista = new List<Persona>();

            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\PRUEBA; database=PERSONA5; integrated security=false; user=sa; password=Desakato000;";

                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select p.numero, p.nombre, p.descripcion, a.descripcion as Arcana, p.url_imagen, t.descripcion as Tipo, ti.descripcion as Debilidad, p.arcana_id as ArcanaID, p.tipo_id as TipoID, p.debilidad_id as DebilidadID, p.id  from Personas p inner join Tipos t on t.id = p.tipo_id inner join Tipos ti on ti.id = p.debilidad_id inner join Arcanas a on p.arcana_id = a.id where p.activo = 1";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Persona aux = new Persona((int)lector["numero"], (string)lector["nombre"], (string)lector["descripcion"], new Arcana(), new Tipo(), new Tipo());

                    aux.Id = (int)lector["id"];
                    if (!(lector["url_imagen"] is DBNull))
                    {
                        aux.UrlImagen = (string)lector["url_imagen"];
                    }
                    aux.Tipo.Id = (int)lector["TipoID"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Debilidad.Id = (int)lector["DebilidadID"];
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];
                    aux.Arcana.Id = (int)lector["ArcanaID"];
                    aux.Arcana.Descripcion = (string)lector["Arcana"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }

        public void agregar(Persona persona)
        {
            try
            {
                datos.setearConsulta("insert into Personas (numero, nombre, descripcion, url_imagen, activo, arcana_id, tipo_id, debilidad_id) values ( @numero , @nombre, @descripcion, @url_imagen, 1, @arcana_id, @tipo_id, @debilidad_id)");
                setearParametrosSql(persona);
                
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Persona persona)
        {

            try
            {
                datos.setearConsulta("update Personas set numero = @numero, nombre = @nombre, descripcion = @descripcion, url_imagen = @url_imagen, arcana_id = @arcana_id, tipo_id = @tipo_id, debilidad_id = @debilidad_id where id = @id");
                setearParametrosSql(persona);
                datos.setearParametros("@id", persona.Id);
                datos.ejecutarAccion();
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

        public void eliminarFisico(int id)
        {
            try
            {
                datos.setearConsulta("delete from Personas where id = @id");
                datos.setearParametros("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarLogico(int id)
        {
            try
            {
                datos.setearConsulta("update Personas set activo = 0 where id = @id");
                datos.setearParametros("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Persona> filtrar(string campo, string criterio, string filtro)
        {
            List<Persona> lista = new List<Persona>();

            try
            {
                string consulta = "select p.numero, p.nombre, p.descripcion, a.descripcion as Arcana, p.url_imagen, t.descripcion as Tipo, ti.descripcion as Debilidad, p.arcana_id as ArcanaID, p.tipo_id as TipoID, p.debilidad_id as DebilidadID, p.id  from Personas p inner join Tipos t on t.id = p.tipo_id inner join Tipos ti on ti.id = p.debilidad_id inner join Arcanas a on p.arcana_id = a.id where p.activo = 1 and ";

                if (campo == "Número")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "p.numero > " + filtro;
                            break;

                        case "Menor a":
                            consulta += "p.numero < " + filtro;
                            break;

                        default:
                            consulta += "p.numero = " + filtro;
                            break;
                    }
                }
                else if(campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "p.nombre like '" + filtro + "%'";
                            break;

                        case "Termina con":
                            consulta += "p.nombre like '%" + filtro + "'";
                            break;

                        default:
                            consulta += "p.nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "a.descripcion like '" + filtro + "%'";
                            break;

                        case "Termina con":
                            consulta += "a.descripcion like '%" + filtro + "'";
                            break;

                        default:
                            consulta += "a.descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Persona aux = new Persona((int)datos.Lector["numero"], (string)datos.Lector["nombre"], (string)datos.Lector["descripcion"], new Arcana(), new Tipo(), new Tipo());

                    aux.Id = (int)datos.Lector["id"];
                    if (!(datos.Lector["url_imagen"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["url_imagen"];
                    }
                    aux.Tipo.Id = (int)datos.Lector["TipoID"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad.Id = (int)datos.Lector["DebilidadID"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];
                    aux.Arcana.Id = (int)datos.Lector["ArcanaID"];
                    aux.Arcana.Descripcion = (string)datos.Lector["Arcana"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void setearParametrosSql(Persona persona)
        {
            datos.setearParametros("@numero", persona.Numero);
            datos.setearParametros("@nombre", persona.Nombre);
            datos.setearParametros("@descripcion", persona.Descripcion);
            datos.setearParametros("@url_imagen", persona.UrlImagen);
            datos.setearParametros("@arcana_id", persona.Arcana.Id);
            datos.setearParametros("@tipo_id", persona.Tipo.Id);
            datos.setearParametros("@debilidad_id", persona.Debilidad.Id);
        }
    }
}
