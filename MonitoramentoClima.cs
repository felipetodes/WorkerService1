using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace WorkerService1
{
    public class MonitoramentoClima : BackgroundService
    {
        
        private readonly ILogger<MonitoramentoClima> _logger;
        private string caminhoLog = @"D:\Curso C#\Log\LogCondicoesClimaticas.txt";

        public MonitoramentoClima(ILogger<MonitoramentoClima> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               



                var client = new RestClient("http://apiadvisor.climatempo.com.br/api/v1");
                var request = new RestRequest("weather/locale/5346/current", Method.Get);
                request.AddQueryParameter("token", "94ea3795e171cb3a321ff95529cab20f");

                var response = await client.GetAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var objClimaAtual = JsonConvert.DeserializeObject<ClimaAtualDto>(response.Content);

                    if (objClimaAtual != null)
                    {
                        GravarLog($"Cidade: {objClimaAtual.Name} - Temperatura: {objClimaAtual.Data.Temperature} - Condição: {objClimaAtual.Data.Condition}");
                        _logger.LogWarning($"Cidade: {objClimaAtual.Name} - Temperatura: {objClimaAtual.Data.Temperature} - Condição: {objClimaAtual.Data.Condition}"));
                    }
                }

                await Task.Delay(5000, stoppingToken);
                }

            }

        private void GravarLog(string mensagem)
        {
            File.AppendAllText(caminhoLog, $"{DateTime.Now.ToString("dd/MM/yyyy 'às' HH:mm:ss")} - {mensagem}");
        }

    }
    public class ClimaAtualDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("data")]
        public DadosClimaDto Data { get; set; }

    }

    public class DadosClimaDto
    {
        [JsonProperty("temperature")]
        public string Temperature { get; set; }

        [JsonProperty("wind_direction")]
        public string WindDirection { get; set; }

        [JsonProperty("wind_velocity")]
        public decimal WindVelocity { get; set; }

        [JsonProperty("humidity")]
        public decimal Humidity { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }

        [JsonProperty("pressure")]
        public decimal Pressure { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("sensation")]
        public string Sensation { get; set; }
    }
}


