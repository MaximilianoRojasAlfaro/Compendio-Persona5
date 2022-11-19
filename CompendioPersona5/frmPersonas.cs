using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace CompendioPersona5
{
    public partial class cpdPersonas : Form
    {
        private List<Persona> listaPersonas = new List<Persona> ();
        private List<Tipo> listaTipos = new List<Tipo> ();
        private List<Arcana> listaArcanas = new List<Arcana> ();

        public cpdPersonas()
        {
            InitializeComponent();
        }

        private void cpdPersonas_Load(object sender, EventArgs e)
        {
            cargar();
            cbCampo.Items.Add("Número");
            cbCampo.Items.Add("Nombre");
            cbCampo.Items.Add("Arcana");
        }

        private void cargar()
        {
            PersonaNegocio pNegocio = new PersonaNegocio();
            listaPersonas = pNegocio.listar();

            TipoNegocio tNegocio = new TipoNegocio();
            listaTipos = tNegocio.listar();

            ArcanaNegocio aNegocio = new ArcanaNegocio();
            listaArcanas = aNegocio.listar();

            dgvPersonas.DataSource = listaPersonas;
            dgvTipos.DataSource = listaTipos;
            dgvArcanas.DataSource = listaArcanas;

            ocultarColumnas();
            CargarImagen(listaPersonas[0].UrlImagen);
        }

        private void dgvPersonas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPersonas.CurrentRow != null)
            {
                Persona seleccionado = (Persona)dgvPersonas.CurrentRow.DataBoundItem;

                CargarImagen(seleccionado.UrlImagen);
            }

        }

        private void CargarImagen(string imagen)
        {
            try
            {
                ptbPersonas.Load(imagen);
            }
            catch (Exception)
            {

                ptbPersonas.Load("https://storage.googleapis.com/proudcity/mebanenc/uploads/2021/03/placeholder-image.png");
            }

        }

        private void btnAgregarPersona_Click(object sender, EventArgs e)
        {
            frmAgregarPersona agregar = new frmAgregarPersona();
            agregar.ShowDialog();

            cargar();
        }

        private void Modificar_Click(object sender, EventArgs e)
        {
            Persona personaSeleccionada;

            personaSeleccionada = (Persona)dgvPersonas.CurrentRow.DataBoundItem;


            frmAgregarPersona modificar = new frmAgregarPersona(personaSeleccionada);
            modificar.ShowDialog();

            cargar();
        }

        private void eliminar(bool logico = false )
        {
            PersonaNegocio negocio = new PersonaNegocio();
            Persona personaSeleccionada;

            try
            {
                DialogResult respuesta = MessageBox.Show("¿De verdad quieres eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    personaSeleccionada = (Persona)dgvPersonas.CurrentRow.DataBoundItem;

                    if (logico)
                        negocio.eliminarLogico(personaSeleccionada.Id);
                    else 
                        negocio.eliminarFisico(personaSeleccionada.Id);

                    
                    cargar();
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            eliminar(true);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PersonaNegocio negocio = new PersonaNegocio();

            try
            {
                if (validarFiltro())
                {
                    return;
                }

                string campo = cbCampo.SelectedItem.ToString();
                string criterio = cbCriterio.SelectedItem.ToString();
                string filtro = tbFiltroAvanzado.Text;

                dgvPersonas.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if (cbCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor ingrese el campo para poder filtrar.");
                return true;
            }
            if (cbCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor ingrese el criterio para poder filtrar.");
                return true;
            }
            if (cbCampo.SelectedItem.ToString() == "Número")
            {
                if (string.IsNullOrEmpty(tbFiltroAvanzado.Text))
                {
                    MessageBox.Show("El filtro no puede estar vacío.");
                    return true;
                }

                if (!(soloNumeros(tbFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Ingrese sólo números para filtrar por un campo numérico");
                    return true;
                }

            }



            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }

            return true;
        }

        private void ocultarColumnas()
        {
            dgvPersonas.Columns["id"].Visible = false;
            dgvPersonas.Columns["UrlImagen"].Visible = false;
        }

        private void tbxFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Persona> listaFiltrada;
            string filtro = tbxFiltro.Text;

            if (filtro.Length >= 2)
            {
                listaFiltrada = listaPersonas.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Tipo.Descripcion.ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = listaPersonas;
            }


            dgvPersonas.DataSource = null;
            dgvPersonas.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbCampo.SelectedItem.ToString();

            if (opcion == "Número")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Mayor a");
                cbCriterio.Items.Add("Menor a");
                cbCriterio.Items.Add("Igual a");
            }
            else
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Comienza con");
                cbCriterio.Items.Add("Termina con");
                cbCriterio.Items.Add("Contiene");
            }
        }
    }
}
