using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Disconnected_Environment
{
    public partial class Form3 : Form
    {
        private string stringConnection = "data source = DESKTOP-ACER202\\MAHARANI; database = Faculty; user ID = sa; password = 123";
        private SqlConnection koneksi;

        private DateTime tgl;
        BindingSource customerBindingSource = new BindingSource();
        public Form3()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            this.bindingNavigator1.BindingSource = this.customerBindingSource;
            refreshform();
        }
        private void FormDataMahasiswa_Load()
        {
            koneksi.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(new SqlCommand("Select m.nim, m.nama_mahasiswa, m.alamat, m.jenis_kelamin, m.tgl_lahir, p.id_prodi From dbo.Mahasiswa m join dbo.Prodi p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);

            this.customerBindingSource.DataSource = ds.Tables[0];
            this.txtNIM.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "NIM", true));
            this.txtNama.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "nama_mahasiswa", true));
            this.txtAlamat.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "alamat", true));
            this.cbxJenisKelamin.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "jenis_kelamin", true));
            this.dtTanggalLahir.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "tgl_lahir", true));
            this.cbxProdi.DataBindings.Add(
                new Binding("Text", this.customerBindingSource, "id_prodi", true));
            koneksi.Close();
        }

        private void FormDataMahasiswa_Load(object sender, EventArgs e)
        {
           
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            dtTanggalLahir.Value = DateTime.Today;
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            cbxJenisKelamin.Enabled = true;
            txtAlamat.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbxProdi.Enabled = true;
            Prodicbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text.Trim();
            string nama = txtNama.Text.Trim();
            string alamat = txtAlamat.Text.Trim();
            string jk = cbxJenisKelamin.SelectedItem.ToString();
            string prodi = cbxProdi.SelectedValue.ToString();
            DateTime tgl = dtTanggalLahir.Value;

            if (string.IsNullOrEmpty(nim) || string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(alamat) || string.IsNullOrEmpty(jk) || string.IsNullOrEmpty(prodi))
            {
                MessageBox.Show("Please fill in all identity fields!");
            }
            else
            {
                try
                {
                    koneksi.Open();
                    string query = "INSERT INTO mahasiswa (nim, nama_mahasiswa, alamat, jenis_kelamin, id_prodi, tgl_lahir) VALUES (@nim, @nama_mahasiswa, @alamat, @jenis_kelamin, @id_prodi, @tgl_lahir)";
                    SqlCommand command = new SqlCommand(query, koneksi);
                    command.Parameters.AddWithValue("@nim", nim);
                    command.Parameters.AddWithValue("@nama_mahasiswa", nama);
                    command.Parameters.AddWithValue("@alamat", alamat);
                    command.Parameters.AddWithValue("@jenis_kelamin", jk);
                    command.Parameters.AddWithValue("@id_prodi", prodi);
                    command.Parameters.AddWithValue("@tgl_lahir", tgl);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Data has been saved to the database.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    koneksi.Close();
                }
            }

            refreshform();
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void clearBinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJenisKelamin.DataBindings.Clear();
            this.dtTanggalLahir.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();

        }

        private void txtNama_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbxJenisKelamin_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtTanggalLahir_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void refreshform()
        {
            txtNIM.Enabled = false;
            txtNama.Enabled = false;
            cbxJenisKelamin.Enabled = false;
            txtAlamat.Enabled = false;
            dtTanggalLahir.Enabled = false;
            cbxProdi.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            clearBinding();
            FormDataMahasiswa_Load();
        }

        

        private void Prodicbx()
        {
            koneksi.Open();
            string str = "SELECT id_prodi, nama_prodi FROM dbo.prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            koneksi.Close();

            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = dt;
        }
    }
}
