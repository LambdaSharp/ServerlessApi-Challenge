﻿using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Delete {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                var itemId = request.PathParameters["id"];
                var id = await DeleteItem(itemId);
                return new APIGatewayProxyResponse {
                    StatusCode = 200,
                    Headers = corsHeaders
                };
            }
            catch (Exception e) {
                LambdaLogger.Log($"*** ERROR: {e}");
                return new APIGatewayProxyResponse {
                    Body = e.Message,
                    StatusCode = 500,
                    Headers = corsHeaders
                };
            }
        }

        private async Task<bool> DeleteItem(string id) {
            DeleteItemOperationConfig config = new DeleteItemOperationConfig {

                // Return the deleted item.
                ReturnValues = ReturnValues.AllOldAttributes
            };
            await _table.DeleteItemAsync(id, config);
            LambdaLogger.Log($"*** INFO: Deleted item {id}" );
            return true;
        }
    }
}
