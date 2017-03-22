namespace Client.Repl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core;
    using Mandoline.Api.Client;
    using Replify;

    public class SelectionCommand : IReplCommand
    {
        private Output output;

        public SelectionCommand()
        {
            this.output = new ConsoleOutput();
        }

        public void GetSelection()
        {
            SavedSelection.RunGetSavedSelection(AppConstants.SavedSelectionId, this.output).RunSync();
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
}
