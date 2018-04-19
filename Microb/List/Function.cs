﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.List {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                var response = await GetItems();
                return new APIGatewayProxyResponse {
                    Body = JsonConvert.SerializeObject(response),
                    StatusCode = 200
                };
            }
            catch (Exception e) {
                LambdaLogger.Log($"*** ERROR: {e}");
                return new APIGatewayProxyResponse {
                    Body = e.Message,
                    StatusCode = 500
                };
            }
        }

        private async Task<IEnumerable<MicrobItem>> GetItems() {
            var request = new ScanRequest {
                TableName = _tableName
            };
            var response = await _dynamoClient.ScanAsync(request);
            return response.Items.Select(x => new MicrobItem {
                id = x["Id"].S,
                title = x["Title"].S,
                content = x["Content"].S,
                date = x["DateCreated"].S
            });
        }
    }
}
