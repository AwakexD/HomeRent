const connection = new signalR.HubConnectionBuilder()
    .withUrl("/userStatusHub")
    .build();

connection.start().then(() => {

}).catch((error) => {
    console.log("Error Connecting : ", error)
})