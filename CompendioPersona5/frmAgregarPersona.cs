using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration;

namespace CompendioPersona5
{
    public partial class frmAgregarPersona : Form
    {
        private Persona persona = null;
        private OpenFileDialog archivo = null;

        public frmAgregarPersona()
        {
            InitializeComponent();
        }

        public frmAgregarPersona(Persona persona)
        {
            InitializeComponent();
            this.persona = persona;
            Text = "Modificar Persona";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
                PersonaNegocio negocio = new PersonaNegocio();
            

            try
            {
                if (persona == null)
                {
                    persona = new Persona();
                }
                
                //persona = atributo privado tipo Persona
                persona.Numero = int.Parse(txtNumero.Text);
                persona.Nombre = txtNombre.Text;
                persona.Descripcion = txtDescripcion.Text;
                persona.UrlImagen = txtUrlImagen.Text;
                persona.Arcana = (Arcana)cbArcana.SelectedItem;
                persona.Tipo = (Tipo)cbTipo.SelectedItem;
                persona.Debilidad = (Tipo)cbDebilidad.SelectedItem;

                if (persona.Id != 0)
                {
                    negocio.modificar(persona);
                    MessageBox.Show("Modificado exitosamente ");
                }
                else
                {
                    negocio.agregar(persona);
                    MessageBox.Show("Agregado con exito!");
                }

                //Guardar imagen si es local
                if (archivo != null && !(txtUrlImagen.Text.ToLower().Contains("http")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["PersonasImg"] + archivo.SafeFileName);
                }

                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAgregarPersona_Load(object sender, EventArgs e)
        {
            TipoNegocio tipoNegocio = new TipoNegocio();
            ArcanaNegocio arcanaNegocio = new ArcanaNegocio();

            try
            {
                cbArcana.DataSource = arcanaNegocio.listar();
                cbArcana.ValueMember = "Id";
                cbArcana.DisplayMember= "Descripcion";

                cbTipo.DataSource = tipoNegocio.listar();
                cbTipo.ValueMember = "Id";
                cbTipo.DisplayMember = "Descripcion";

                cbDebilidad.DataSource = tipoNegocio.listar();
                cbDebilidad.ValueMember = "Id";
                cbDebilidad.DisplayMember = "Descripcion";

                if (persona != null)
                {
                    
                    txtNumero.Text = persona.Numero.ToString();
                    txtNombre.Text = persona.Nombre;
                    txtDescripcion.Text = persona.Descripcion;
                    txtUrlImagen.Text = persona.UrlImagen;
                    cbArcana.SelectedValue = persona.Arcana.Id;
                    cbTipo.SelectedValue = persona.Tipo.Id;
                    cbDebilidad.SelectedValue = persona.Debilidad.Id;

                    CargarImagen(persona.UrlImagen);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtUrlImagen.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pcbPersonaAgregar.Load(imagen);
            }
            catch (Exception ex)
            {

                pcbPersonaAgregar.Load("https://storage.googleapis.com/proudcity/mebanenc/uploads/2021/03/placeholder-image.png");
            }

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);
            }
        }
    }
}
