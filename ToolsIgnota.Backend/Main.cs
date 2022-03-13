using System;
using ToolsIgnota.Backend;

var client = new CombatManagerClient();
await client.StartListening(x => 
{
    Console.WriteLine(x.Id);
});
while (Console.ReadLine() != "stop") { };
await client.StopListening();
Console.WriteLine("Stopped.");