﻿using System;
 using System.Collections.Generic;
 using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Read {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                var response = await GetItem(request.PathParameters["id"]);
                return new APIGatewayProxyResponse {
                    Body = JsonConvert.SerializeObject(response),
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

        private async Task<MicrobItem> GetItem(string id) {
            var response = await _table.GetItemAsync(id);
            return new MicrobItem {
                id = response["Id"],
                title = response["Title"],
                content = response["Content"],
                date = response["DateCreated"]
            };
        }
    }
}
