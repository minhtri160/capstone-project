$(document).ready(function () {
    SignalR();
    Notification.GetAllNotification();
})

function SignalR() {
    //alert("starting signalr");
    var hub = $.connection.signalRHub;
    hub.client.getSensorData = function (deviceId, state, sensorValue) {
        var stateString = "Off";
        if (state == 1) {
            stateString = "On"
        } else if (state == -1) {
            stateString = "Not Responding"
        }
        if (state == -1)
        {
            if ($('#' + deviceId).hasClass('small-box bg-green-active')) {
                $('#' + deviceId).removeClass('small-box bg-green-active').addClass('small-box bg-red-active');
            }
        }
        else {
            if ($('#' + deviceId).hasClass('small-box bg-red-active')) {
                $('#' + deviceId).removeClass('small-box bg-red-active').addClass('small-box bg-green-active');
            }
        }
        $('#' + deviceId + 's').html(stateString);
        $('#' + deviceId + 'n').val(state);
        var listSensor = sensorValue.split(";");
        for (var i = 0; i < listSensor.length; i++) {
            var item = listSensor[i];
            var value = item.split(":");
            $('#' + value[0]).html(value[1]);
            if (value[2] == "0")
                $('#' + value[0] + 'w').hide();
            else
                $('#' + value[0] + 'w').show();
        }


    };

    hub.client.pushNotification = function (title, amount) {
        $('#noti').html(amount);
        alert('You have 1 new notification: ' + title);
        Notification.GetAllNotification();
    };

    //hub.client.remote = function (deviceId, state) {
    //    var Id = '#' + deviceId + 'n';
    //    var deviceId = '#' + deviceId + 's';
    //    var status = $(Id).val();
    //    if (status.trim() != "-1") {
    //        var deviceState = "Off";
    //        if (state == 1)
    //            deviceState = "On";
    //        $(deviceId).html(deviceState);
    //        $(Id).val(state);
    //    }
    //}

    $.connection.hub.start().done(function () {

        var channel = $('#channel').val();
        hub.server.joinGroup(channel, false);
        var stateString = $('#deviceId').val();
        var state = 0;
        if (stateString == "On")
            state = 1;
        function remote(id) {
            hub.server.RemoteDevice(channel, id, state);
        }
        $('#httc8').click(function () {
            var state = $('#httc8n').val()
            if (state.trim() != "-1") {
                var remoteState = 0;
                if (state.trim() == "0")
                    remoteState = 1;
                hub.server.remoteDevice(channel, 'httc8', remoteState);
            }
        })
        $('#quat1').click(function () {
            var state = $('#quat1n').val()
            if (state.trim() != "-1") {
                var remoteState = 0;
                if (state.trim() == "0")
                    remoteState = 1;
                hub.server.remoteDevice(channel, 'quat1', remoteState);
            }
        })
        $('#hmna7').click(function () {
            var state = $('#hmna7n').val()
            if (state.trim() != "-1") {
                var remoteState = 0;
                if (state.trim() == "0")
                    remoteState = 1;
                hub.server.remoteDevice(channel, 'hmna7', remoteState);
            }
        })
        $('#mtia8').click(function () {
            var state = $('#mtia8n').val()
            if (state.trim() != "-1") {
                var remoteState = 0;
                if (state.trim() == "0")
                    remoteState = 1;
                hub.server.remoteDevice(channel, 'mtia8', remoteState);
            }
        })
        $('#pmez5').click(function () {
            var state = $('#pmez5n').val()
            if (state.trim() != "-1") {
                var remoteState = 0;
                if (state.trim() == "0")
                    remoteState = 1;
                hub.server.remoteDevice(channel, 'pmez5', remoteState);
            }
        })
    })
}