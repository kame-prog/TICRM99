using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TICRM.BuisnessLayer
{
    public class MqttManager
    {
        private readonly IMqttClient _client;

        public MqttManager()
        {
            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();
        }

        public void Connect()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("mqtt.example.com", 1883) // Replace with your MQTT broker address and port
                .WithClientId("your_client_id") // Replace with your client ID
                .WithCredentials("your_username", "your_password") // Replace with your MQTT broker credentials
                .Build();

            _client.ConnectAsync(options).Wait();
            _client.SubscribeAsync("your_topic_name", MqttQualityOfServiceLevel.AtLeastOnce).Wait();
        }

        public void HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            // Do something with the received message, such as update a view model or database
        }

        public void Disconnect()
        {
            _client.DisconnectAsync().Wait();
        }
    }
}
