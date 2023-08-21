using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ABCC.Clases;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Data.Common;
using System.Drawing.Text;

namespace ABCC.Formas
{
    public partial class Frmabcc : Form
    {
        Clases.conexion objconexion;
        SqlConnection conexion;
        int existe;
        string descontinuado;

        
        public Frmabcc()
        {
            InitializeComponent();
            cargar_departamentos();
            
            
        }

        public void cargar_departamentos()
        {
            objconexion = new Clases.conexion();
            conexion = new SqlConnection(objconexion.conn());
            conexion.Open();
            SqlCommand command = new SqlCommand("SELECT nom_Departamento, num_Departamento FROM Departamentos", conexion);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            conexion.Close();

            DataRow fila = dataTable.NewRow();
            fila["nom_Departamento"] = "SELECCIONA UN DEPARTAMENTO";
            dataTable.Rows.InsertAt(fila,0);

            cboxdepartamento.ValueMember = "num_Departamento";
            cboxdepartamento.DisplayMember = "nom_Departamento";
            cboxdepartamento.DataSource = dataTable;
        }

        public void cargar_clase(string num_Clase)
        {
            objconexion = new Clases.conexion();
            conexion = new SqlConnection(objconexion.conn());
            conexion.Open();
            SqlCommand command = new SqlCommand("Select num_Clase, nom_Clase FROM Clase where num_Clase=@num_Clase", conexion);
            command.Parameters.AddWithValue("num_Clase", num_Clase);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            conexion.Close();

            DataRow dr = dataTable.NewRow();
            dr["nom_Clase"] = "SELECCIONE UNA CLASE";
            dataTable.Rows.InsertAt(dr, 0);

            cboxclases.ValueMember = "num_Clase";
            cboxclases.DisplayMember = "nom_Clase";
            cboxclases.DataSource = dataTable;
        }

        public void cargar_familia(string num_Familia)
        {
            objconexion = new Clases.conexion();
            conexion = new SqlConnection(objconexion.conn());
            conexion.Open();
            SqlCommand command = new SqlCommand("Select num_Familia, nom_Familia FROM Familia where num_Familia=@num_Familia", conexion);
            command.Parameters.AddWithValue("num_Familia", num_Familia);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            conexion.Close();

            DataRow dr = dataTable.NewRow();
            dr["nom_Familia"] = "SELECCIONE UNA FAMILIA";
            dataTable.Rows.InsertAt(dr, 0);

            cboxfamilia.ValueMember = "num_Familia";
            cboxfamilia.DisplayMember = "nom_Familia";
            cboxfamilia.DataSource = dataTable;
        }


        private void txtsku_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar == 13)
            {
                objconexion = new Clases.conexion();
                conexion = new SqlConnection(objconexion.conn());
                //se abre la conexion
                conexion.Open();
                string query = "select * from articulos where ar_Sku=@ar_Sku";
                //asigo a comando el sql command
                SqlCommand comando = new SqlCommand(query, conexion);
                //inicializo cualquier parametro definido anteriormente
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@ar_Sku", txtsku.Text);
                comando.Parameters.AddWithValue("@ar_Articulo", txtarticulo.Text);
                comando.Parameters.AddWithValue("@ar_Marca", txtmarca.Text);
                comando.Parameters.AddWithValue("@ar_Modelo", txtmodelo.Text);
                comando.Parameters.AddWithValue("@ar_Departamento", cboxdepartamento.SelectedIndex);
                comando.Parameters.AddWithValue("@ar_Clase", cboxclases.SelectedIndex);           
                comando.Parameters.AddWithValue("@ar_Familia", cboxfamilia.SelectedIndex);
                comando.Parameters.AddWithValue("@ar_Stock", txtstock.Text);
                comando.Parameters.AddWithValue("@ar_Cantidad", txtcantidad.Text);
                SqlDataReader leer = comando.ExecuteReader();
                if (leer.Read())
                {
                    existe = 1;

                    txtsku.Text = leer["ar_Sku"].ToString();
                    txtarticulo.Text = leer["ar_Articulo"].ToString();
                    txtarticulo.Focus();
                    txtmarca.Text = leer["ar_Marca"].ToString();
                    txtmodelo.Text = leer["ar_Modelo"].ToString();
                    cboxdepartamento.SelectedIndex = int.Parse(leer["ar_Departamento"].ToString());
                    cboxclases.SelectedIndex = int.Parse(leer["ar_Clase"].ToString());
                    cboxfamilia.SelectedIndex= int.Parse(leer["ar_Familia"].ToString());
                    txtstock.Text = leer["ar_Stock"].ToString();
                    txtcantidad.Text= leer["ar_Cantidad"].ToString();
                    btnactualizar.Enabled = true;
                    btnboton.Enabled = true;

                }
                else
                {
                    if (MessageBox.Show("Sku no registrado, deseas agregar?", "Atencion!!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                    {
                        
                        txtarticulo.Focus();
                        txtarticulo.Enabled = true;
                        txtmarca.Enabled = true;
                        txtmodelo.Enabled = true;
                        cboxdepartamento.Enabled = true;
                        cboxclases.Enabled = true;
                        cboxfamilia.Enabled = true;
                        txtstock.Enabled = true;
                        txtcantidad.Enabled = true;
                        
                        

;                        
                    }
                    else
                    {
                        txtsku.Clear();
                        txtsku.Focus();
                    }

                }
                conexion.Close();

            }


        }
        
        private void txtcantidad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if (existe == 0)
                {
                    objconexion = new Clases.conexion();
                    conexion = new SqlConnection(objconexion.conn());
                    conexion.Open();
                    string query = "insert into articulos values (@ar_Sku, @ar_Articulo, @ar_Marca, @ar_Modelo, @ar_Departamento, @ar_Clase, @ar_Familia, @ar_Stock, @ar_Alta, @ar_Cantidad, 0, 1900-01-01)";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@ar_sku", this.txtsku.Text);
                    comando.Parameters.AddWithValue("@ar_Articulo", this.txtarticulo.Text);
                    comando.Parameters.AddWithValue("@ar_Marca", this.txtmarca.Text);
                    comando.Parameters.AddWithValue("@ar_Modelo", this.txtmodelo.Text);
                    comando.Parameters.AddWithValue("@ar_Departamento", this.cboxdepartamento.SelectedIndex);
                    comando.Parameters.AddWithValue("@ar_Clase", this.cboxclases.SelectedIndex);
                    comando.Parameters.AddWithValue("@ar_Familia", this.cboxfamilia.SelectedIndex);
                    comando.Parameters.AddWithValue("@ar_Stock", this.txtstock.Text);
                    comando.Parameters.AddWithValue("@ar_Alta", fechaalta.Value.Date.Add(new TimeSpan(0, 0, 0)));
                    comando.Parameters.AddWithValue("@ar_Cantidad", this.txtcantidad.Text);
                    comando.ExecuteNonQuery();
                    MessageBox.Show("REGISTRO EXITOSO", "GUARDADO", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtsku.Clear();
                    txtarticulo.Clear();
                    txtmarca.Clear();
                    txtmodelo.Clear();
                    txtstock.Clear();
                    txtcantidad.Clear();
                    txtsku.Focus();
                    cboxfamilia.SelectedIndex = 0;
                    cboxdepartamento.SelectedIndex = 0;
                    cboxclases.SelectedIndex = 0;
                    txtarticulo.Enabled = false;
                    txtmarca.Enabled = false;
                    txtmodelo.Enabled = false;
                    cboxdepartamento.Enabled = false;
                    cboxclases.Enabled = false;
                    cboxfamilia.Enabled = false;
                    txtstock.Enabled = false;
                    txtcantidad.Enabled = false;

                }
                if (existe == 1)
                {
                    objconexion = new Clases.conexion();
                    conexion = new SqlConnection(objconexion.conn());
                    conexion.Open();
                    string query = "update articulos set ar_Articulo=@ar_Articulo, ar_Marca=@ar_Marca, ar_Modelo=@ar_Modelo, ar_Departamento=@ar_Departamento, ar_Clase=@ar_Clase, ar_Familia=@ar_Familia, ar_Stock=@ar_Stock, ar_Alta=@ar_Alta, ar_Cantidad=@ar_Cantidad, ar_Baja=@ar_Baja where ar_Sku=@ar_Sku";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@ar_sku", this.txtsku.Text);
                    comando.Parameters.AddWithValue("@ar_Articulo", this.txtarticulo.Text);
                    comando.Parameters.AddWithValue("@ar_Marca", this.txtmarca.Text);
                    comando.Parameters.AddWithValue("@ar_Modelo", this.txtmodelo.Text);
                    comando.Parameters.AddWithValue("@ar_Departamento", this.cboxdepartamento.SelectedIndex);
                    comando.Parameters.AddWithValue("@ar_Clase", this.cboxclases.SelectedIndex);
                    comando.Parameters.AddWithValue("@ar_Familia", this.cboxfamilia.SelectedIndex);
                    comando.Parameters.AddWithValue("@ar_Stock", this.txtstock.Text);
                    comando.Parameters.AddWithValue("@ar_Alta", fechaalta.Value.Date.Add(new TimeSpan(0, 0, 0)));
                    comando.Parameters.AddWithValue("@ar_Cantidad", this.txtcantidad.Text);
                    comando.Parameters.AddWithValue("@ar_Baja", fechabaja.Value.Date.Add(new TimeSpan(0, 0, 0)));
                    comando.ExecuteNonQuery();
                    MessageBox.Show("ACTUALIZACION EXITOSA", "GUARDADO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtsku.Clear();
                    txtarticulo.Clear();
                    txtmarca.Clear();
                    txtmodelo.Clear();
                    txtstock.Clear();
                    txtcantidad.Clear();
                    txtsku.Focus();
                    cboxfamilia.SelectedIndex = 0;
                    cboxdepartamento.SelectedIndex = 0;
                    cboxclases.SelectedIndex = 0;
                    txtarticulo.Enabled = false;
                    txtmarca.Enabled = false;
                    txtmodelo.Enabled = false;
                    cboxdepartamento.Enabled = false;
                    cboxclases.Enabled = false;
                    cboxfamilia.Enabled = false;
                    txtstock.Enabled = false;
                    txtcantidad.Enabled = false;
                    fechaalta.Enabled = false;
                    fechabaja.Enabled = false;
                    txtdescontinuado.Enabled = false;

                }
                

            }
            
            
        }

        private void boton_Click(object sender, EventArgs e)
        {
            
            objconexion = new Clases.conexion();
            conexion = new SqlConnection(objconexion.conn());
            
            conexion.Open();
            string query = "Delete  articulos  where ar_Sku=@ar_Sku";
            
            SqlCommand comando = new SqlCommand(query, conexion);
            comando.Parameters.AddWithValue("@ar_Sku", txtsku.Text);


            if (MessageBox.Show("Seguro que quiere dar de baja??", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                comando.ExecuteNonQuery();
                MessageBox.Show("Baja exitosa", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtsku.Clear();
                txtarticulo.Clear();
                txtmarca.Clear();
                txtmodelo.Clear();
                txtstock.Clear();
                txtcantidad.Clear();
                txtsku.Focus();
                cboxfamilia.SelectedIndex = 0;
                cboxdepartamento.SelectedIndex = 0;
                cboxclases.SelectedIndex = 0;
                txtarticulo.Enabled = false;
                txtmarca.Enabled = false;
                txtmodelo.Enabled = false;
                cboxdepartamento.Enabled = false;
                cboxclases.Enabled = false;
                cboxfamilia.Enabled = false;
                txtstock.Enabled = false;
                txtcantidad.Enabled = false;
                fechaalta.Enabled = false;
                fechabaja.Enabled = false;
                txtdescontinuado.Enabled = false;
                btnboton.Enabled = false;
                btnactualizar.Enabled = false;


            }
            conexion.Close();
            



        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            txtarticulo.Focus();
            txtarticulo.Enabled = true;
            txtmarca.Enabled = true;
            txtmodelo.Enabled = true;
            cboxdepartamento.Enabled = true;
            cboxclases.Enabled = true;
            cboxfamilia.Enabled = true;
            txtstock.Enabled = true;
            txtcantidad.Enabled = true;
            txtdescontinuado.Enabled = true;
            fechaalta.Enabled = true;
            fechabaja.Enabled = true;
        }

        private void cboxdepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboxdepartamento.SelectedValue.ToString() != null)
            {
                string num_Departamento = cboxdepartamento.SelectedValue.ToString();
                cargar_clase(num_Departamento);

                

            }
        }

        private void cboxclases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxclases.SelectedValue.ToString() != null)
            {
                string num_Clase = cboxclases.SelectedValue.ToString();
                cargar_familia(num_Clase);
            }
        }
    }
}
