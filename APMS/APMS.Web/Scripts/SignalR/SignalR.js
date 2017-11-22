$(document).ready(function () {
    SignalR();
})

function SignalR() {
    //alert("starting signalr");
    var hub = $.connection.signalRHub;
    hub.client.getSensorData = function (deviceId, state, sensorValue) {
        var stateString = "Off";
        if (state != 0)
        {
            stateString = "On"
        }
        $('#deviceId').html(stateString);
        var listSensor = sensorValue.split(";");
        for (item in listSensor) {
            var sensorValue = item.split(":");
            $('#' + sensorValue[0]).html(sensorValue[1]);
            if (sensorValue[2] == "0")
                $('#' + sensor[0] + 'w').hide();
            else
                $('#' + sensor[0] + 'w').show();
        }

        
    };

    hub.client.notification = function (title) {
        alert('You have 1 new notification: '+title);
    };

    hub.client.remote = function (deviceId, state)
    {
        alert(deviceId+" " +state);
    }

    $.connection.hub.start().done(function () {
        
        var accountId = $('#accountId').val();
        hub.server.joinGroup(accountId, false);
        alert(accountId);
        var stateString = $('#deviceId').val();
        var state = 0;
        if (stateString == "On")
            state = 1;
        function remote(id) {
            hub.server.RemoteDevice(accountId, id, state);
        }
        $('#fan').click(function () {
            alert("clicked!");
            hub.server.remoteDevice(accountId, 'abc123', 1);
        })
    })
}