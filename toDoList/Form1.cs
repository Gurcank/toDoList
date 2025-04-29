using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace toDoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataTable todoList = new DataTable();
        bool isEditing = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            //Create columns
            todoList.Columns.Add("Title");
            todoList.Columns.Add("Description");
            todoList.Columns.Add("Done", typeof(bool)); // Checkbox gibi görünecek


            //It displays all the data from the DataTable inside the DataGridView.
            dataGridView1.DataSource = todoList;


        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTitle.Text))
                {
                    MessageBox.Show("Title cannot be empty.","Warning",MessageBoxButtons.OK ,MessageBoxIcon.Warning);
                    return;
                }
                if (isEditing && dataGridView1.CurrentCell != null)
                {
                    int rowIndex  = dataGridView1.CurrentCell.RowIndex; 
                    todoList.Rows[rowIndex]["Title"] = txtTitle.Text;
                    todoList.Rows[rowIndex]["Description"] = txtDescription.Text;

                }
                else
                {
                    todoList.Rows.Add(txtTitle.Text, txtDescription.Text , false); //Done columns defaults false
                }
                //Clear fields 
                txtTitle.Clear();
                txtDescription.Clear();
                isEditing = false;
                UpdateProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell == null) return;

                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                todoList.Rows[rowIndex]["Done"] = true;
                UpdateProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell == null) return;

                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                todoList.Rows[rowIndex].Delete();
                UpdateProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateProgress();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentCell == null)  return;   

                int rowIndex = dataGridView1.CurrentCell.RowIndex;

                isEditing = true;
                //Fill text fields with data from dataTable
                txtTitle.Text = todoList.Rows[rowIndex].ItemArray[0].ToString();
                txtDescription.Text = todoList.Rows[rowIndex].ItemArray[1].ToString();
                isEditing = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            todoList.Rows.Clear();
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            int total = todoList.Rows.Count;
            int doneCount = todoList.AsEnumerable().Count(row => row.RowState != DataRowState.Deleted 
            && row.Field<bool>("Done"));

            if (total == 0)
            {
                progressBar1.Value = 0;
            }
            else
            {
                progressBar1.Value = total == 0 ? 0 : (int)((doneCount / (double)total) * 100);
            }

            lblProgress.Text = doneCount + " of " + total + " done";
        }

        private void r(object sender, EventArgs e)
        {

        }
    }
}
