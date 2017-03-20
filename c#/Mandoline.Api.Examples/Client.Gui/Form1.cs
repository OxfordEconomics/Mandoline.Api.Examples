using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mandoline.Api.Client;
using Mandoline.Api.Client.Models;
using Mandoline.Api.Client.ServiceModels;
using Core;

namespace Client.Gui 
{
    public partial class Form1 : Form
    {
        Output output;
        public Form1()
        {
            InitializeComponent();
            output = new GridOutput(this);
        }

        public void setLabelVisible(bool v)
        {
            label1.Visible = v;
        }

        public void setDGV(object o)
        {
            dataGridView1.DataSource = o;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void buttonSubmit_Click(object sender, EventArgs e)
        {
            label1.Text = "Running " + comboBox1.Text + "...";
            label1.Visible = true;

            switch (comboBox1.Text)
            {
                case ("DownloadShaped"):
                    await DownloadShaped.RunDownloadShapedAsync(output);
                    break;
                case ("DownloadShapedStream"):
                    await DownloadShaped.RunDownloadShapedStreamAsync(output);
                    break;
                case ("DownloadFile"):
                    await Download.RunDownloadFileAsync(output);
                    break;
                case ("GetVariables"):
                    await Info.RunGetVariablesAsync(output);
                    break;
                case ("RequestDownload"):
                    await Download.RunRequestDownloadAsync(output);
                    break;
                case ("GetSavedSelection"):
                    await SavedSelection.RunGetSavedSelection(AppConstants.SAVED_SELECTION_ID,output);
                    break;
                case ("UpdateSavedSelection"):
                    await SavedSelection.RunUpdateSavedSelection(output);
                    break;
                case ("CreateSavedSelection"):
                    await SavedSelection.RunCreateSavedSelection(output);
                    break;
                case ("GetAllUsers"):
                    await Core.User.RunGetAllUsersAsync(output);
                    break;
                case ("Login"):
                    await Core.User.RunLoginAsync(output, textBox0.Text, textBox1.Text);
                    break;
                case ("GetUser"):
                    await Core.User.RunGetUserAsync(output);
                    break;
                case ("GetDatabanks"):
                    await Info.RunGetDatabanksAsync(output);
                    break;
                case ("Download"):
                    await Download.RunDownloadAsync(output);
                    break;
                default:
                    label1.Text = "Invalid selection";
                    break;
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void PasswordKeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) 
                label1.Text = "Running login...";
                label1.Visible = true;
                Core.User.RunLoginAsync(output, textBox0.Text, textBox1.Text);
        }
    }

}
