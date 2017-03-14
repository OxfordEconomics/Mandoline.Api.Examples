using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client.ServiceModels;
using Mandoline.Api.Client;
using System.Data;

namespace ExampleMandolineAPI 
{
    class SavedSelection
    {
        // creates a new saved selection based on SelectionDto sampleSelect 
        public static void RunCreateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = Settings.SampleSelect.GetInstance();

            // set up api object for making call
            var api = new ApiClient("https://services.oxfordeconomics.com/", Settings.API_TOKEN);

            // queue asynchronous api call
            api.CreateSavedSelectionAsync(sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);

                // update table
                RunGetSavedSelection(output);

            },TaskScheduler.FromCurrentSynchronizationContext());
        }

        // updates saved selection based on id and SelectionDto sampleSelect 
        // note: the selection object passed into this must have the id field set
        //       i.e. it isn't enough just to include the id in the function params
        public static void RunUpdateSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = Settings.SampleSelect.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // change selection object for update
            sampleSelect.Id = new Guid(Settings.SAVED_SELECTION_ID);
            sampleSelect.Name = "Selection - Updated: " + DateTime.Now;

            // set up api object for making call
            var api = new ApiClient("https://services.oxfordeconomics.com/", Settings.API_TOKEN);

            // queue asynchronous api call
            api.UpdateSavedSelectionAsync(sampleSelect.Id, sampleSelect, new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5)).Token).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);
                Console.WriteLine("DESC: {0}...", t.Result.Description);
                Console.WriteLine("RESULT: {0}...", t.Result.ToString());

                // update table
                RunGetSavedSelection(output);

            },TaskScheduler.FromCurrentSynchronizationContext());
        }

        // creates a new saved selection based on SelectionDto sampleSelect 
        public static void RunGetSavedSelection(Output output)
        {
            // get our sample selection
            SelectionDto sampleSelect = Settings.SampleSelect.GetInstance();

            // create table for displaying selection data
            Table.SelectionTable dt = new Table.SelectionTable();

            // set up api object for making call
            var api = new ApiClient("https://services.oxfordeconomics.com/", Settings.API_TOKEN);

            // queue asynchronous api call
            api.GetSavedSelection(new Guid(Settings.SAVED_SELECTION_ID)).ContinueWith(t => {
                Console.WriteLine("STATUS: {0}...", t.Result.Reason);

                output.PrintData(t.Result.Result);

            },TaskScheduler.FromCurrentSynchronizationContext());
        }


    }
}
