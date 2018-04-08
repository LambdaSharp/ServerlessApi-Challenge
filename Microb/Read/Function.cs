using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace Microb.Read {
    
    //--- Methods ---
    class Function {
        
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public APIGatewayProxyResponse LambdaHandler(APIGatewayProxyRequest request) {
            LambdaLogger.Log(JsonConvert.SerializeObject(request));
            return new APIGatewayProxyResponse {
                Body = "{\"title\": \"Hello API Gateway!\", \"content\": \"foo bar\"}",
                StatusCode = 200
            };
        }
    }
}