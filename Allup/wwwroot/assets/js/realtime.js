

$(document).ready(function () {

    var connection = new signalR.HubConnectionBuilder().withUrl('/chat').build();

    connection.start();

    console.log(connection)

    connection.on("online", function (id) {
        let span = $(`${id} span`);
        span.addClass('bg-success');
        span.removeClass('bg-secondary');

    })

    connection.on("offline", function (id) {
        let span = $(`${id} span`);
        span.removeClass('bg-success');
        span.addClass('bg-secondary');
    })

    $(document).on('click', '.userItem', function () {
        let fullName = $(this).attr("data-userName")
        let userId = $(this).attr('id');
        console.log(`${fullName} ${userId}`);
        $('.sender').text(fullName);
        $('#userId').val(userId);

        fetch('/home/GetMessages/?userId=' + userId)
            .then(res => res.text())
            .then(data => {
                $('.msgContainer').html(data);
            })

    })

    connection.on('orderaccepted', function () {
        alert("Order is accepted!");
    })

    $(document).on('click', '#sendmessage', function (e) {
        e.preventDefault();
        let userId = $('#userId').val();
        let msg = $('#message').val();
        $('#message').val('');
        let listItem = `<li class="list-group-item">${msg}</li>`;
        $('.msgContainer').append(listItem);
        connection.invoke('SendMessage', userId, msg);
    })

    connection.on('privatemessage', function (message, senderid, recieverId) {
        console.log(`${message} ${senderid}`);
        let userId = $('#userId').val();
        console.log(`${recieverId} ${userId} ${senderid}`)

        if (userId == senderid) {
            let listItem = `<li class="list-group-item">${message}</li>`;
            $('.msgContainer').append(listItem);
        }
    })
})