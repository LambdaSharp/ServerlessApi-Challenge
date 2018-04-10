using System;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Read {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public APIGatewayProxyResponse LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                // TODO Read single item
                return new APIGatewayProxyResponse {
                    Body = "{\"message\": \"TODO\"}",
                    StatusCode = 200
                };
            }
            catch (Exception e) {
                LambdaLogger.Log($"*** ERROR: {e}");
                return new APIGatewayProxyResponse {
                    Body = "{\"message\": \"{e.message}\"}",
                    StatusCode = 500
                };
            }
        }

        private async Task<MicrobItem> GetItem(string id) {
            var response = await _table.GetItemAsync("Id", id);
            return new MicrobItem {
                id = response["Id"],
                title = response["Title"],
                content = response["content"],
                date = response["DateCreated"]
            };
        }
    }
}