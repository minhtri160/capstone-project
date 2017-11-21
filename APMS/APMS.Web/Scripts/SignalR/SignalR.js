function SignalR() {
    var hub=$.connection.SignalRHub;
    hub.clients.GetSensorData = function (deviceId, state, sensorValue, warningState) {
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
        }

        if (warningState != 0) {
            //warning
        }
    };

    hub.clients.notification = function (title) {
        alert('You have 1 new notification: '+title);
    };

    hub.clients.remote = function (device, state)
    {
        alert(device);
    }

    $.connection.hub.start().done(function () {
        var accountId = $('#accountId').val();
        hub.server.JoinGroup(accountId);
        var stateString = $('#deviceId').val();
        var state = 0;
        if (stateString == "On")
            state = 1;
        function remote(id) {
            hub.server.RemoteDevice(accountId, id, state);
        }
    })
}