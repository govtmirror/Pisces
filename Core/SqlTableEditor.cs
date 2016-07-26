using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using DgvFilterPopup;
using System.Collections.Generic;
namespace Reclamation.Core
{
	/// <summary>
	/// FormTableEditor allows basic editing of database tables.
	/// </summary>
	public class SqlTableEditor : System.Windows.Forms.Form
	{
    private DataTable table=null;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.DataGridView dataGrid1;
    private System.Windows.Forms.ComboBox comboBoxTableNames;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private Button buttonExcel;
        private Button buttonImport;

        private BasicDBServer m_server;
		public SqlTableEditor(BasicDBServer server,string[] tableNames=null)
		{
            m_server = server;
			InitializeComponent();
            if (tableNames == null)
            {
                LoadTableList();
            }
            else
            {
                this.comboBoxTableNames.Items.Clear();
                this.comboBoxTableNames.Items.AddRange(tableNames);
            }
		}

    private void LoadTableList()
    {
      this.comboBoxTableNames.Items.Clear();
      this.comboBoxTableNames.Items.AddRange(m_server.TableNames());
    }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlTableEditor));
            this.buttonSave = new System.Windows.Forms.Button();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.comboBoxTableNames = new System.Windows.Forms.ComboBox();
            this.buttonExcel = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(280, 8);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.Location = new System.Drawing.Point(8, 44);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(552, 412);
            this.dataGrid1.TabIndex = 4;
            // 
            // comboBoxTableNames
            // 
            this.comboBoxTableNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTableNames.Location = new System.Drawing.Point(8, 8);
            this.comboBoxTableNames.Name = "comboBoxTableNames";
            this.comboBoxTableNames.Size = new System.Drawing.Size(232, 21);
            this.comboBoxTableNames.TabIndex = 3;
            this.comboBoxTableNames.SelectedIndexChanged += new System.EventHandler(this.comboBoxTableNames_SelectedIndexChanged);
            // 
            // buttonExcel
            // 
            this.buttonExcel.Image = ((System.Drawing.Image)(resources.GetObject("buttonExcel.Image")));
            this.buttonExcel.Location = new System.Drawing.Point(390, 1);
            this.buttonExcel.Name = "buttonExcel";
            this.buttonExcel.Size = new System.Drawing.Size(44, 40);
            this.buttonExcel.TabIndex = 9;
            this.buttonExcel.Click += new System.EventHandler(this.buttonExcel_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(440, 6);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(120, 23);
            this.buttonImport.TabIndex = 10;
            this.buttonImport.Text = "Import from CSV ...";
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // SqlTableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 462);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonExcel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.comboBoxTableNames);
            this.Name = "SqlTableEditor";
            this.Text = "FormTableEditor";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

    }
		#endregion

    private void comboBoxTableNames_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      LoadData();
    }


    DgvFilterManager filterManager = new DgvFilterManager();

    private void LoadData()
    {

        filterManager = null;
        filterManager = new DgvFilterManager();

    table = null;
      string tableName = "";
      try
      {
        tableName = this.comboBoxTableNames.Text;
         table = m_server.Table(tableName);
        this.dataGrid1.DataSource=table;
        this.dataGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        //this.dataGrid1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

        filterManager.DataGridView = this.dataGrid1;
      }
      catch(Exception ex)
      {
        Logger.WriteLine(ex.Message);
        MessageBox.Show(ex.Message);
      }
    }

    private void buttonSave_Click(object sender, System.EventArgs e)
    {
      try
      {
        DataTable tbl = (DataTable)this.dataGrid1.DataSource;
        m_server.SaveTable(tbl);
      }
      catch(Exception ex )
      {
        MessageBox.Show(ex.Message);
        Logger.WriteLine(ex.Message);
      }
    }

        private void buttonExcel_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable table = (DataTable)this.dataGrid1.DataSource;

                // Save to CSV..
                string tmpFilename = FileUtility.GetTempFileName(".csv");
                DataTable t = table.Copy();
                if (t.Columns.Contains("ID"))
                {
                    t.Columns["ID"].ColumnName = "A_ID";
                }

                CsvFile.WriteToCSV(t, tmpFilename, false);
                System.Diagnostics.Process.Start(tmpFilename);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Csv file (*.csv)|*.csv";
            if( dlg.ShowDialog() ==  DialogResult.OK)
            {
                DataTable table = (DataTable)this.dataGrid1.DataSource;
                if (table == null)
                    return;
                var dataTypes = new List<string>();

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    dataTypes.Add(table.Columns[i].DataType.ToString());

                }

                CsvFile csv = new CsvFile(dlg.FileName, dataTypes.ToArray());

               
                table.Merge(csv);
              //  m_server.SaveTable(table);

            }

        }
	}
}
