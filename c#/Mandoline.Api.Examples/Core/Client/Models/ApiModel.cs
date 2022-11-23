using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Client;
namespace Core.Client
{
    public interface ApiModel
    {
        ApiClient Client { get; set; }
    }
}
