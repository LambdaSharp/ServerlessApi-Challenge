﻿using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Create {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                var item = JsonConvert.DeserializeObject<MicrobItem>(request.Body);
                var id = await CreateItem(item.title, item.content);
                return new APIGatewayProxyResponse {
                    Body = "{\"id\": \"" + id + "\"}",
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

        private async Task<string> CreateItem(string title, string content) {
            var id = Guid.NewGuid().ToString();
            var now = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            var item = new Document();
            item["Id"] = id;
            item["Title"] = title;
            item["Content"] = content;
            item["DateCreated"] = now;
            await _table.PutItemAsync(item);
            LambdaLogger.Log($"*** INFO: Created item {id}");
            return id;
        }
    }
}
