// <copyright file="Form1.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Windows.Forms;
using Core;
using Core.Client;

namespace Client.Gui;

public partial class Form1 : Form
{
    private readonly Output output;

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

    private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private async void ButtonSubmit_Click(object sender, EventArgs e)
    {
        // check the case that
        if (this.comboBox1.Text != "Login" &&
            (AppConstants.ApiToken == "INVALID_KEY" || AppConstants.ApiToken == string.Empty))
        {
            this.label1.Text = "Must be logged in to run this function...";
            this.label1.Visible = true;
            return;
        }

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
            case "GetRegions":
                await Info.RunGetRegionsAsync(this.output);
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
                await User.RunGetAllUsersAsync(this.output);
                break;
            case "Login":
                await User.RunLoginAsync(this.output, this.textBox0.Text, this.textBox1.Text);
                break;
            case "GetUser":
                await User.RunGetUserAsync(this.output);
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

    private void PasswordKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            this.label1.Text = "Running login...";
            this.label1.Visible = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            User.RunLoginAsync(this.output, this.textBox0.Text, this.textBox1.Text);
        }
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        // check for valid api key
        if (AppConstants.ApiToken != string.Empty)
        {
            Console.WriteLine("Checking api key from config settings...");
            ApiClient api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);
            Core.Client.Models.Assertion<Core.Client.Models.User> userCheck = await api.GetUserAsync();
            if (userCheck.Failed == true)
            {
                this.label1.Text = "Invalid api key. Please log in.";
                this.label1.Visible = true;
                AppConstants.ApiToken = "INVALID_KEY";
            }
        }
    }
}
