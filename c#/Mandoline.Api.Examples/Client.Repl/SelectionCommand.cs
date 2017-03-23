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

        public async Task GetSelection()
        {
            await SavedSelection.RunGetSavedSelection(AppConstants.SavedSelectionId, this.output);
        }

        public async Task CreateSelection()
        {
            await SavedSelection.RunCreateSavedSelection(this.output);
        }

        public async Task UpdateSelection()
        {
            await SavedSelection.RunUpdateSavedSelection(this.output);
        }
    }
}
