using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

using var _mqttClient = new MqttFactory().CreateManagedMqttClient();

var options = new ManagedMqttClientOptionsBuilder()
    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
    .WithClientOptions(new MqttClientOptionsBuilder()
        .WithClientId(Guid.NewGuid().ToString())
        .WithTcpServer("mqtt.pskreporter.info")
        .Build())
.Build();

var topics = new[] {
    // numbers are ARRL DXCC entity codes
    new MqttTopicFilterBuilder().WithTopic("pskr/filter/v2/+/+/+/+/+/+/+/223").Build(),
    new MqttTopicFilterBuilder().WithTopic("pskr/filter/v2/+/+/+/+/+/+/+/294").Build(),
    new MqttTopicFilterBuilder().WithTopic("pskr/filter/v2/+/+/+/+/+/+/+/265").Build(),
    new MqttTopicFilterBuilder().WithTopic("pskr/filter/v2/+/+/+/+/+/+/+/279").Build(),
    new MqttTopicFilterBuilder().WithTopic("pskr/filter/v2/+/+/+/+/+/+/+/245").Build(),
};

await _mqttClient.SubscribeAsync(topics);
await _mqttClient.StartAsync(options);

_mqttClient.ApplicationMessageReceivedAsync += arg => {
    Console.WriteLine(arg.ApplicationMessage.ConvertPayloadToString());
    return Task.CompletedTask;
};

Console.WriteLine("Waiting for messages... press enter to quit");
Console.ReadLine();