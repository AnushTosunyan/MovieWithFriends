using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MovieUniverse.Abstract.ApiModels;

namespace MovieUniverse.API.FileStreamProvider
{
    class CustomMultipartFileStreamProvider : MultipartMemoryStreamProvider

    {
        public UserEditModel UserModel { get; set; }

        public override Task ExecutePostProcessingAsync(CancellationToken cancellationToken)
        {
            foreach (var file in Contents)
            {
                var parameters = file.Headers.ContentDisposition.Parameters;
                var data = new UserEditModel
                {
                    Name = GetNameHeaderValue(parameters, "name"),

                };
            }

            
            return base.ExecutePostProcessingAsync(cancellationToken);
        }

        public override Task ExecutePostProcessingAsync()
        {
            foreach (var file in Contents)
            {
                var parameters = file.Headers.ContentDisposition.Parameters;
                var data = new UserEditModel
                {
                    Name = GetNameHeaderValue(parameters, "name"),
                    
                };
            }

            return base.ExecutePostProcessingAsync();
        }

        private static string GetNameHeaderValue(ICollection<NameValueHeaderValue> headerValues, string name)
        {
            var nameValueHeader = headerValues.FirstOrDefault(
                x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            return nameValueHeader != null ? nameValueHeader.Value : null;
        }


    }
}