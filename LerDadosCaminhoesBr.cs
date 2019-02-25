using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionsCaminhoesBR
{
    public static class LerDadosCaminhoesBr
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("LerDadosCaminhoesBr")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "ConexaoCaminhoesBR")]EventData message, [Table("tblCaminhoesBR", Connection="AzureWebJobsStorage")]ICollector<Mensagem> tabela, ILogger log)
        {
            tabela.Add(new Mensagem(Encoding.UTF8.GetString(message.Body.Array)));    
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }

    public class Mensagem: TableEntity
    {
        public Mensagem(string payLoad)
        {
            this.PartitionKey = "CaminhoesBR";
            this.RowKey = System.Guid.NewGuid().ToString();
            this.PayLoad = payLoad;
        }
        public string PayLoad { get; set; }
    }

}