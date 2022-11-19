using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Persona
    {
        public int Id { get; set; }
        [DisplayName("Número")]
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [DisplayName("Url Imagen")]
        public string UrlImagen { get; set; }
        public Arcana Arcana { get; set; }
        public Tipo Tipo { get; set; }
        public Tipo Debilidad { get; set; }

        public Persona()
        {

        }

        public Persona(int numero, string nombre, string descripcion, Arcana arcana, Tipo tipo, Tipo debilidad)
        {
            Numero = numero;
            Nombre = nombre;
            Descripcion = descripcion;
            Arcana = arcana;
            Tipo = tipo;
            Debilidad = debilidad;
        }

    }
}
