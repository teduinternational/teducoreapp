var connection = new signalR.HubConnectionBuilder()
    .withUrl("/teduHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start().catch(err => console.error(err.toString()));

connection.on("ReceiveMessage", (message) => {
    var template = $('#announcement-template').html();
    var html = Mustache.render(template, {
        Content: message.content,
        Id: message.id,
        Title: message.title,
        FullName: message.fullName,
        Avatar: message.avatar
    });
    $('#annoncementList').prepend(html);

    var totalAnnounce = parseInt($('#totalAnnouncement').text()) + 1;

    $('#totalAnnouncement').text(totalAnnounce);
});