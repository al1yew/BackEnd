
var connection = new signalR.HubConnectionBuilder().withUrl('/hublink').build();

connection.start();

console.log(connection)