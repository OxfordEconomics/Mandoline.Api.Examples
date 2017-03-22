namespace Client.Gui
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Core;
    using Mandoline.Api.Client;
    using Mandoline.Api.Client.Models;
    using Mandoline.Api.Client.ServiceModels;

    public partial class Form1 : Form
    {
        private Output output;

        public Form1()
        {
            this.InitializeComponent();
            this.output = new GridOutput(this);
        }

        public void SetLabelVisible(bool v)
        {
            this.label1.Visible = v;
        }

        public void SetDGV(object o)
        {
            this.dataGridView1.DataSource = o;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private async void buttonSubmit_Click(object sender, EventArgs e)
        {
            this.label1.Text = "Running " + this.comboBox1.Text + "...";
            this.label1.Visible = true;

            switch (this.comboBox1.Text)
            {
                case "DownloadShaped":
                    await DownloadShaped.RunDownloadShapedAsync(this.output);
                    break;
                case "DownloadShapedStream":
                    await DownloadShaped.RunDownloadShapedStreamAsync(this.output);
                    break;
                case "DownloadFile":
                    await Download.RunDownloadFileAsync(this.output);
                    break;
                case "GetVariables":
                    await Info.RunGetVariablesAsync(this.output);
                    break;
                case "RequestDownload":
                    await Download.RunRequestDownloadAsync(this.output);
                    break;
                case "GetSavedSelection":
                    await SavedSelection.RunGetSavedSelection(AppConstants.SavedSelectionId, this.output);
                    break;
                case "UpdateSavedSelection":
                    await SavedSelection.RunUpdateSavedSelection(this.output);
                    break;
                case "CreateSavedSelection":
                    await SavedSelection.RunCreateSavedSelection(this.output);
                    break;
                case "GetAllUsers":
                    await Core.User.RunGetAllUsersAsync(this.output);
                    break;
                case "Login":
                    await Core.User.RunLoginAsync(this.output, this.textBox0.Text, this.textBox1.Text);
                    break;
                case "GetUser":
                    await Core.User.RunGetUserAsync(this.output);
                    break;
                case "GetDatabanks":
                    await Info.RunGetDatabanksAsync(this.output);
                    break;
                case "Download":
                    await Download.RunDownloadAsync(this.output);
                    break;
                default:
                    this.label1.Text = "Invalid selection";
                    break;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void PasswordKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.label1.Text = "Running login...";
                this.label1.Visible = true;
                Core.User.RunLoginAsync(this.output, this.textBox0.Text, this.textBox1.Text);
            }
        }
    }
}
