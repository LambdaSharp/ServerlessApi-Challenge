﻿using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Update {
    
    class Function: MicrobFunction {
        
        //--- Methods ---
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            try {
                var itemId = request.PathParameters["id"];
                var item = JsonConvert.DeserializeObject<MicrobItem>(request.Body);
                await UpdateItem(itemId, item.title, item.content);
                return new APIGatewayProxyResponse {
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

        private async Task UpdateItem(string id, string title, string content) {
            var item = new Document();
            if (!await ItemExists(id)) {
                var now = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
                item["DateCreated"] = now;
            }
            item["Id"] = id;
            item["Title"] = title;
            item["Content"] = content;
            await _table.UpdateItemAsync(item);
        }

        private async Task<bool> ItemExists(string id) {
            var response = await _table.GetItemAsync(id);
            return response != null;
        }
    }
}
