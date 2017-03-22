namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Mandoline.Api.Client.Models;
    using Mandoline.Api.Client.ServiceModels;

    public abstract class Output
    {
        // used to determine whether running function should wait for
        // this AND api call
        public bool IsAsync;

        // handles load status indicator, text and visibility
        public abstract void UpdateStatus(bool v);

        public abstract void UpdateStatus(string v);

        // handles updating datagridview source
        public abstract void PrintData(List<DataseriesDto> ld);

        public abstract void PrintData(string s);

        public abstract void PrintData(ControllerDownloadResponseDto response, string filename, string s);

        public abstract void PrintData(ShapedStreamResult result);

        public abstract void PrintData(VariableCollectionDto vc);

        public abstract void PrintData(IEnumerable<Databank> ld);

        public abstract void PrintData(Mandoline.Api.Client.Models.User u);

        public abstract void PrintData(IEnumerable<Mandoline.Api.Client.Models.User> ul);

        public abstract void PrintData(Mandoline.Api.Client.Models.User u, string token);

        public abstract void PrintData(SelectionDto s);
    }

    // use this when isAsync is set to false, i.e. when output
    // is expected to come synchronously. runs async Task functions
    // as though they were void, blocking unnecessary await warning
    public static class VoidTaskExtension
    {
        public static void RunSync(this Task task)
        {
        }
    }
}
