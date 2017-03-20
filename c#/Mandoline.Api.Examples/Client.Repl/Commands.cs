using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using Replify;
using Core;

namespace Client.Repl
{
    public class UserCommand : IReplCommand
    {

        private Output output;

        public UserCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void GetUser()
        {
            User.RunGetUserAsync(this.output).RunSync(); 
        }

        public void GetUsers()
        {
            User.RunGetAllUsersAsync(this.output).RunSync(); 
        }

        public void LogIn()
        {
            do
            {
                Console.Write("Username: ");
                string user = Console.ReadLine();
                Console.Write("Password: ");
                string pass = Console.ReadLine();
                Core.User.RunLoginAsync(output, user, pass).RunSync();
            } while (Core.AppConstants.API_TOKEN == null);

        }

    }

    public class InfoCommand : IReplCommand
    {

        private Output output;

        public InfoCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void GetDatabanks()
        {
            Info.RunGetDatabanksAsync(this.output).RunSync(); 
        }

        public void GetVariables()
        {
            Info.RunGetVariablesAsync(this.output).RunSync(); 
        }

    }

    public class SelectionCommand: IReplCommand
    {

        private Output output;

        public SelectionCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void GetSelection()
        {
            SavedSelection.RunGetSavedSelection(AppConstants.SAVED_SELECTION_ID, this.output).RunSync(); 
        }

        public void CreateSelection()
        {
            SavedSelection.RunCreateSavedSelection(this.output).RunSync(); 
        }

        public void UpdateSelection()
        {
            SavedSelection.RunUpdateSavedSelection(this.output).RunSync(); 
        }

    }

    public class DownloadCommand: IReplCommand
    {

        private Output output;

        public DownloadCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void DownloadFile()
        {
            Download.RunDownloadFileAsync(output).RunSync();
        }

        public void RequestDownload()
        {
            Download.RunRequestDownloadAsync(output).RunSync();
        }

        public void RunDownload()
        {
            Download.RunDownloadAsync(output).RunSync();
        }

    }

    public class ShapedDownloadCommand: IReplCommand
    {

        private Output output;

        public ShapedDownloadCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void ShapedDownload()
        {
            DownloadShaped.RunDownloadShapedAsync(output).RunSync();
        }

        public void ShapedDownloadStream()
        {
            DownloadShaped.RunDownloadShapedStreamAsync(output).RunSync();
        }

    }
}
