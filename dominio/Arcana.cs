using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Arcana
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public Arcana()
        {

        }

        public Arcana(int id, string descripcion)
        {
            Id = id;
            Descripcion = descripcion;
        }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
