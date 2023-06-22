﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disconnected_Environment
{
    public partial class Form4 : Form
    {
        private string stringConnection = "data source = DESKTOP-ACER202\\MAHARANI; database = Faculty; user ID = sa; password = 123";
        private SqlConnection koneksi;
        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringConnection);
            refreshform();
        }

        private void FormDataStatusMahasiswa_FormClosed(object sender, EventArgs e)
        {

        }
        private void dataGridView()
        {
            try
            {
                koneksi.Open();
                string query = "SELECT id_status, nim, status_mahasiswa, tahun_masuk FROM dbo.StatusMahasiswa";
                SqlDataAdapter adapter = new SqlDataAdapter(query, koneksi);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
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




        private void refreshform()
        {
            cbxNama.Enabled = false;
            cbxStatusMahasiswa.Enabled = false;
            cbxTahunMasuk.Enabled = false;
            cbxNama.SelectedIndex = -1;
            cbxStatusMahasiswa.SelectedIndex = -1;
            cbxTahunMasuk.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnSave.Enabled = false;
            btnClear.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void cbTahunMasuk()
        {
            int currentYear = DateTime.Now.Year;
            int startYear = 2010;
            for (int year = startYear; year <= currentYear; year++)
            {
                cbxTahunMasuk.Items.Add(year.ToString());
            }
        }

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cbNama()
        {
            try
            {
                koneksi.Open();
                string query = "SELECT nama_mahasiswa, nim FROM dbo.StatusMahasiswa WHERE NOT EXISTS (SELECT id_status FROM dbo.status_mahasiswa WHERE status_mahasiswa.nim = mahasiswa.nim)";
                SqlCommand command = new SqlCommand(query, koneksi);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string namaMahasiswa = reader["nama_mahasiswa"].ToString();
                    string nim = reader["nim"].ToString();
                    cbxNama.Items.Add(namaMahasiswa);
                    cbxNama.ValueMember = nim;
                }

                reader.Close();
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


        private void cbTahunMasuk(object sender, EventArgs e)
        {
            
        }

        private void cbxNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select NIM from dbo.StatusMahasiswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbxNama.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();

            txtNIM.Text = nim;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxTahunMasuk.Enabled = true;
            cbxNama.Enabled = true;
            cbxStatusMahasiswa.Enabled = true;
            txtNIM.Visible = true;
            cbTahunMasuk();
            cbNama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            string nim = txtNIM.Text;
            string statusMahasiswa = cbxStatusMahasiswa.Text;
            string tahunMasuk = cbxTahunMasuk.Text;

            koneksi.Open();

            string getMaxIdQuery = "SELECT MAX(id_status) FROM dbo.StatusMahasiswa";
            SqlCommand getMaxIdCommand = new SqlCommand(getMaxIdQuery, koneksi);
            object maxIdResult = getMaxIdCommand.ExecuteScalar();
            int newId = (maxIdResult != DBNull.Value) ? Convert.ToInt32(maxIdResult) + 1 : 1;

            string insertQuery = "INSERT INTO dbo.StatusMahasiswa (id_status, nim, status_mahasiswa, tahun_masuk) VALUES (@idStatus, @nim, @statusMahasiswa, @tahunMasuk)";
            SqlCommand insertCommand = new SqlCommand(insertQuery, koneksi);
            insertCommand.Parameters.AddWithValue("@idStatus", newId);
            insertCommand.Parameters.AddWithValue("@nim", nim);
            insertCommand.Parameters.AddWithValue("@statusMahasiswa", statusMahasiswa);
            insertCommand.Parameters.AddWithValue("@tahunMasuk", tahunMasuk);
            insertCommand.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshform();
            dataGridView();
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void cbxStatusMahasiswa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
