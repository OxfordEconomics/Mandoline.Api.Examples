using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Client.ServiceModels;

namespace Core.Client.Models.Requests
{
    public class ShapedDownloadRequest
    {
        public ShapeConfigurationDto Config { get; internal set; }
        public SelectionDto Selection { get; internal set; }
    }
}
