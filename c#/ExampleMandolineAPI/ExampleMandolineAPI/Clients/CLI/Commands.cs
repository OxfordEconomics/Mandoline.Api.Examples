using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandoline.Api.Client;
using Replify;

namespace ExampleMandolineAPI
{
    public class UserCommand : IReplCommand
    {

        public UserCommand(ApiClient client)
        {
            var api = new ApiClient();
        }


    }
}
