using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Create {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public APIGatewayProxyResponse LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                // TODO Create an item
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

        private async Task<string> CreateItem(string title, string content) {
            var id = Guid.NewGuid();
            var now = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            var item = new Document();
            item["Id"] = id.ToString();
            item["Title"] = title;
            item["Content"] = content;
            item["DateCreated"] = now;
            await _table.PutItemAsync(item);
            LambdaLogger.Log($"*** INFO: Created item {id}");
            return id.ToString();
        }
    }
}
