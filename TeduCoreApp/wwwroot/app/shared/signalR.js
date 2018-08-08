var connection = new signalR.HubConnectionBuilder()
    .withUrl("/teduHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {
    console.log(message);
});