using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;

namespace Microb {
    
    public class MicrobFunction {

        //--- Fields ---
        protected readonly string _tableName;
        protected readonly string _awsRegion;
        protected readonly RegionEndpoint _regionEndpoint;
        protected readonly AmazonDynamoDBClient _dynamoClient;
        protected readonly Table _table;
        
        //--- Constructors ---
        public MicrobFunction() {
            _tableName = System.Environment.GetEnvironmentVariable("DYNAMO_TABLE_NAME");
            _awsRegion = System.Environment.GetEnvironmentVariable("AWS_REGION");
            LambdaLogger.Log($"***INFO: {_tableName}");
            LambdaLogger.Log($"***INFO: {_awsRegion}");
            _regionEndpoint = RegionEndpoint.GetBySystemName(_awsRegion);
            _dynamoClient = new AmazonDynamoDBClient(_regionEndpoint);
            _table = Table.LoadTable(_dynamoClient, _tableName);
        }
    }
}