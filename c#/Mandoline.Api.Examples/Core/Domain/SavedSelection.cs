using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client.ServiceModels;
using Mandoline.Api.Client;
using System.Data;

namespace Core
{
    public class SavedSelection
    {
        // creates a new saved selection based on SelectionDto sampleSelect 
        public static async Task RunCreateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            if (output.isAsync)
                await api.CreateSavedSelectionAsync(sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => 
                    RunGetSavedSelection(t.Result.Result.Id, output));
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
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // change selection object for update
            sampleSelect.Id = AppConstants.SAVED_SELECTION_ID;
            sampleSelect.Name = "Selection - Updated: " + DateTime.Now;

            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            if (output.isAsync)
                await api.UpdateSavedSelectionAsync(sampleSelect.Id, sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t =>
                    RunGetSavedSelection(AppConstants.SAVED_SELECTION_ID, output));
            else
            {
                var result = api.UpdateSavedSelectionAsync(sampleSelect.Id, sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token)
                    .GetAwaiter().GetResult();
                RunGetSavedSelection(AppConstants.SAVED_SELECTION_ID, output).RunSync();
            }

        }

        // creates a new saved selection based on SelectionDto sampleSelect 
        // takes output object, selection id
        public static async Task RunGetSavedSelection(Guid s_id, Output output)
        {
            Console.WriteLine(string.Format("Checking selection with id={0}...", s_id));

            // get our sample selection
            SelectionDto sampleSelect = AppConstants.SampleSelect.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // set up api object for making call
            var api = new ApiClient(AppConstants.BASE_URL, AppConstants.API_TOKEN);

            // queue asynchronous api call
            if (output.isAsync)
                await api.GetSavedSelection(s_id).ContinueWith(t => output.PrintData(t.Result.Result));
            else
            {
                var result = api.GetSavedSelection(s_id).GetAwaiter().GetResult();
                output.PrintData(result.Result);
            }

        }


    }
}
