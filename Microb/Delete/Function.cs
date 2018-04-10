using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Delete {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public APIGatewayProxyResponse LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                // TODO Delete an item
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

        private async Task<bool> DeleteItem(string id) {
            DeleteItemOperationConfig config = new DeleteItemOperationConfig {

                // Return the deleted item.
                ReturnValues = ReturnValues.AllOldAttributes
            };
            var document = await _table.DeleteItemAsync(id, config);
            LambdaLogger.Log($"*** INFO: Deleted item {id}" );
            return true;
        }
    }
}