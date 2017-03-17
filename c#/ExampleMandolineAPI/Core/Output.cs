using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client.ServiceModels;
using Mandoline.Api.Client.Models;

namespace Core
{
    public abstract class Output
    {
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
}
