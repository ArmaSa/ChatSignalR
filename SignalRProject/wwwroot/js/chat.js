"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var connectionIdClient = 0;

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveNotification", function (name,message) {

    var textContent = "";

    if (message == '' || message == null) {
        li.textContent = `  ${name}`;
    }
    else {
        li.textContent = `  ${name} :=> ${message}`;
    }
    var subsequentli = $('<li class="friendmsg">' + textContent +'</li>'); 


    $("#messagesList").before(subsequentli);

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connectionIdClient = connection.connection.connectionId;
    let inputmyId = document.getElementById("myId");
    inputmyId.value = connectionIdClient;

    console.log(connectionIdClient); 
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("userInput").addEventListener("input", function (event) {
    var user = this.value;
    if (user.length > 0) {
        document.getElementById("connect").disabled = false;
    }
    else {
        document.getElementById("connect").disabled = true;
    }
});

document.getElementById("connect").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;

    if (user.length > 0) {
        var connectionIdEndPoint = document.getElementById("myId").value;
        if (connectionIdEndPoint === null || connectionIdEndPoint === '') {
            connectionIdEndPoint = 0;
        }

        connection.invoke("SetNameConnectionId", connectionIdEndPoint, user).catch(function (err) {
            return console.error(err.toString());
        });

        document.getElementById("disconnect").style.display = "block";
        document.getElementById("connect").style.display = "none";
    }

});

document.getElementById("disconnect").addEventListener("click", function (event) {

    document.getElementById("connect").style.display = "block";
    document.getElementById("disconnect").style.display = "none";

    var connectionIdEndPoint = document.getElementById("myId").value;
    if (connectionIdEndPoint === null || connectionIdEndPoint === '') {
        connectionIdEndPoint = 0;
    }
    connection.invoke("Oisconnect").catch(function (err) {
        return console.error(err.toString());
    });
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var connectionIdEndPoint = document.getElementById("connectionId").value;
    if (connectionIdEndPoint == null) {
        connectionIdEndPoint = 0;
    }

    var userName = document.getElementById("userInput").value;
    var textContent = `${userName} :=> ${message}`;
    var subsequentli = $('<li class="mymsg">' + textContent + '</li>');

    $("#messagesList").prepend(subsequentli);


    connection.invoke("SendMessage", connectionIdEndPoint, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//connection.start().then(function () {
//    console.log("Setup_SignalR: connection open.");

//    // Get the connectionID...
//    connectionIdClient = _signalconnection.connection.connectionId;

//    // Make a log entry that is page contextual...
//    var page = window.location.pathname;
//    console.log("Setup_SignalR:ConnectionOpen for page (" + page + ") with connectionID = " + _connectionid);

//}).catch(function (err) {
//    console.error("Setup_SignalR: connection failed. err = " + err);
//});

$(document).ready(function () {
    $("body").backgroundCycle({
        imageUrls: [
            '../images/background.jpg',
            '../images/background1.jpg',
        ],
        fadeSpeed: 2000,
        duration: 5000,
        backgroundSize: 2
    });
});