using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

public class MQTTAttack : MonoBehaviour
{
    private IMqttClient _mqttClient;
    public string receivedAttackPayload = ""; // add a public string variable to store the received message payload


    private async void Start()
    {
        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("iot.cpe.ku.ac.th", 1883) // Replace with your broker's address and port
            .WithCleanSession()
            .WithCredentials("b6510503298","chayanon.ma@ku.th")
            .Build();

        _mqttClient.UseConnectedHandler(async e =>
        {
            Debug.Log("Connected to MQTT broker!");
            await _mqttClient.SubscribeAsync("b6510503298/attack"); // Replace with the topic you want to subscribe to
        });

        _mqttClient.UseDisconnectedHandler(async e =>
        {
            Debug.LogWarning("Disconnected from MQTT broker.");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await _mqttClient.ConnectAsync(options);
        });

        _mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            receivedAttackPayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload); // store the received payload in the public variable
            Debug.Log($"Received message from MQTT broker: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        });

        await _mqttClient.ConnectAsync(options);
    }

    private async void OnDestroy()
    {
        if (_mqttClient != null && _mqttClient.IsConnected)
        {
            await _mqttClient.DisconnectAsync();
        }
    }
}

