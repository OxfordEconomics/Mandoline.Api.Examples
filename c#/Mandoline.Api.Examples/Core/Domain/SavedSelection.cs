namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mandoline.Api.Client;
    using Mandoline.Api.Client.ServiceModels;

    public class SavedSelection
    {
        // creates a new saved selection based on SelectionDto sampleSelect
        public static async Task RunCreateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.CreateSavedSelectionAsync(sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t =>
                    RunGetSavedSelection(t.Result.Result.Id, output));
            }
            else
            {
                var result = api.CreateSavedSelectionAsync(sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).GetAwaiter().GetResult();
                RunGetSavedSelection(result.Result.Id, output).RunSync();
            }
        }

        // updates saved selection based on id and SelectionDto sampleSelect
        // note: the selection object passed into this must have the id field set
        //       i.e. it isn't enough just to include the id in the function params
        public static async Task RunUpdateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // change selection object for update
            sampleSelect.Id = AppConstants.SavedSelectionId;
            sampleSelect.Name = "Selection - Updated: " + DateTime.Now;

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.UpdateSavedSelectionAsync(sampleSelect.Id, sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t =>
                    RunGetSavedSelection(AppConstants.SavedSelectionId, output));
            }
            else
            {
                var result = api.UpdateSavedSelectionAsync(sampleSelect.Id, sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token)
                    .GetAwaiter().GetResult();
                RunGetSavedSelection(AppConstants.SavedSelectionId, output).RunSync();
            }
        }

        // creates a new saved selection based on SelectionDto sampleSelect
        // takes output object, selection id
        public static async Task RunGetSavedSelection(Guid s_id, Output output)
        {
            Console.WriteLine(string.Format("Checking selection with id={0}...", s_id));

            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelection.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BaseURL, AppConstants.ApiToken);

            // queue asynchronous api call
            if (output.IsAsync)
            {
                await api.GetSavedSelection(s_id).ContinueWith(t => output.PrintData(t.Result.Result));
            }
            else
            {
                var result = api.GetSavedSelection(s_id).GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }
        }
    }
}
