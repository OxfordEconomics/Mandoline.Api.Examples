// <copyright file="Output.cs" company="Oxford Economics">
// Copyright (c) 2017 Oxford Economics Ltd. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project
// root for full license information.
// </copyright>

namespace Core.Client
{
    using Core.Client.Models;
    using Core.Client.ServiceModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class Output
    {
        // handles load status indicator, text and visibility
        public abstract void UpdateStatus(bool v);

        public abstract void UpdateStatus(string v);

        // handles updating datagridview source
        public abstract void PrintData(List<DataseriesDto> ld);

        public abstract void PrintData(List<List<DataseriesDto>> ld);

        public abstract void PrintData(string s);

        public abstract void PrintData(ControllerDownloadResponseDto response, string filename, string s);

        public abstract void PrintData(ShapedStreamResult result);

        public abstract void PrintData(VariableCollectionDto vc);

        public abstract void PrintData(RegionCollectionDto vc);

        public abstract void PrintData(IEnumerable<Databank> ld);

        public abstract void PrintData(Models.User u);

        public abstract void PrintData(IEnumerable<Core.Client.Models.User> ul);

        public abstract void PrintData(Core.Client.Models.User u, string token);

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
