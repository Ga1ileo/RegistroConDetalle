using RegistroConDetalle.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using RegistroConDetalle.BLL;

namespace RegistroConDetalle.UI.Registros
{
    /// <summary>
    /// Interaction logic for Registro.xaml
    /// </summary>
    public partial class Registro : Window
    {
        public List<TelefonosDetalle> Detalle { get; set; }
        public Registro()
        {
            InitializeComponent();
            this.Detalle = new List<TelefonosDetalle>();
        }

        public void Limpiar()
        {
            IdTextBox.Text = string.Empty;
            NombreTextBox.Text = string.Empty;
            TelefonoTextBox.Text = string.Empty;
            CedulaTextBox.Text = string.Empty;
            DireccionTextBox.Text = string.Empty;
            FechaNacimeintoDatePicker.Text = Convert.ToString(DateTime.Now);

            this.Detalle = new List<TelefonosDetalle>();
            CargarGrid();
            
        }

        public void CargarGrid()
        {
            DetalleDataGrid.ItemsSource = null;
            DetalleDataGrid.ItemsSource = this.Detalle;
        }

        private Personas LlenaClase()
        {
            Personas persona = new Personas();
            if (string.IsNullOrWhiteSpace(IdTextBox.Text))
            {
                IdTextBox.Text = "0";
            }
            else     
                
                persona.PersonaId = Convert.ToInt32(IdTextBox.Text);
                persona.Nombre = NombreTextBox.Text;
                persona.Cedula = CedulaTextBox.Text;
                persona.Direccion = DireccionTextBox.Text;
                persona.FechaNacimiento = Convert.ToDateTime(FechaNacimeintoDatePicker.SelectedDate);
                
                persona.Telefonos = this.Detalle;
                return persona;
        }

        private void LlenaCampos(Personas persona)
        {
            IdTextBox.Text = Convert.ToString(persona.PersonaId);
            NombreTextBox.Text = persona.Nombre;
            CedulaTextBox.Text = persona.Cedula;
            DireccionTextBox.Text = persona.Direccion;
            FechaNacimeintoDatePicker.SelectedDate = persona.FechaNacimiento;

            this.Detalle = persona.Telefonos;
            CargarGrid();
        }

        private bool Validar()
        {
            bool paso = true;

            if(NombreTextBox.Text == string.Empty)
            {
                MessageBox.Show(NombreTextBox.Text, "No Puede ser 0");
                NombreTextBox.Focus();
                paso = false;
            }

            if (string.IsNullOrWhiteSpace(DireccionTextBox.Text))
            {
                MessageBox.Show(DireccionTextBox.Text, "No puede estar vacio");
                DireccionTextBox.Focus();
                paso = false;
            }

            if (string.IsNullOrWhiteSpace(CedulaTextBox.Text))
            {
                MessageBox.Show(CedulaTextBox.Text, "No puede estar vacio");
                CedulaTextBox.Focus();
                paso = false;
            }

            if(this.Detalle.Count == 0)
            {
                TelefonoTextBox.Focus();
                paso = false;
            }

            return paso;
        }

        private bool ExisteBd()
        {
            Personas persona = PersonasBLL.Buscar(Convert.ToInt32(IdTextBox.Text));
            return (persona != null);
        }

        private void NuevoButton_Click(object sender, RoutedEventArgs e)
        {
            Limpiar();
        }

        private void GuardarButton_Click(object sender, RoutedEventArgs e)
        {
            Personas persona;
            bool paso = false;

            if (!Validar())
                return;

            persona = LlenaClase();

            if (string.IsNullOrWhiteSpace(IdTextBox.Text) || IdTextBox.Text == "0")
                paso = PersonasBLL.Guardar(persona);
            else
            {
                if (!ExisteBd())
                {
                    MessageBox.Show("Perosna no ha nacido");
                    return;
                }
                paso = PersonasBLL.Modificar(persona);
            }

            if (paso)
            {
                Limpiar();
                MessageBox.Show("Guardado!", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No fue posible guardar");
            }
           
        }

        private void EliminarButton_Click(object sender, RoutedEventArgs e)
        {
            int id;
            id = Convert.ToInt32(IdTextBox.Text);

            Limpiar();

            if (PersonasBLL.Eliminar(id))
                MessageBox.Show("Eliminado", "Exito", MessageBoxButton.OK);
            else
                MessageBox.Show(IdTextBox.Text, "No se puede eliminar una persona que no ha nacido");
        }

        private void BuscarButton_Click(object sender, RoutedEventArgs e)
        {
            int id;
            Personas persona = new Personas();
            int.TryParse(IdTextBox.Text, out id);

            Limpiar();

            persona = PersonasBLL.Buscar(id);

            if(persona != null)
            {
                MessageBox.Show("Persona Encontrada");
                LlenaCampos(persona);
            }
            else
            {
                MessageBox.Show("Persona no encontrada");
            }
        }

        //Este es el Boton de agregar Telefonos al detalle
        private void TipoButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetalleDataGrid.ItemsSource != null)
                this.Detalle = (List<TelefonosDetalle>)DetalleDataGrid.ItemsSource;
            this.Detalle.Add(
                new TelefonosDetalle{ 
                    Telefono = TelefonoTextBox.Text,
                    TipoTelefono = TipoTextBox.Text
                });
            CargarGrid();
            TelefonoTextBox.Focus();
            TelefonoTextBox.Clear();
            TipoTextBox.Clear();
        }
    }
}
