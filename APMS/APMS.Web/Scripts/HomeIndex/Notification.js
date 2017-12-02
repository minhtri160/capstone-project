var Notification = {
    GetAllNotification: function () {
        $.ajax({
            url: '/Home/Notification',
            datatype: 'html',
            success: function (data) {
                
                $("#noti").html(data);
                $("#noti_icon").click();
            },
            error: function () {
                alert("fail");
            }
        })
    },

    GetDetailNotification: function (id) {
        $.ajax({
            url: '/Home/DetailNotification/'+id,
            datatype: 'html',
            success: function (data) {
                $("#modalContent").html(data);
                $("#myModal").modal('toggle');
                Notification.GetAllNotification();
            },
            error: function () {
                alert("fail");
            }
        })
    }
}