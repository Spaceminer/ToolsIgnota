using System;
using ToolsIgnota.Backend;

var client = new CombatManagerClient();
await client.StartListening(x => 
{
    Console.WriteLine(x.Id);
});
while (Console.ReadLine() != "stop") { };
client.StopListening();
Console.WriteLine("Stopped.");