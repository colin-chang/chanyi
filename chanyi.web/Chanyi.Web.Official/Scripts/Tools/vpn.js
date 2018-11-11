/// <reference path="../jquery-1.11.3.min.js" />
$(function () {
    $.getJSON("/Tools/GetVpn", {}, function (data, status) {
        $(".progress-vpn").addClass("hidden");
        var defaultMsg = "暂不可用"
        var vpn = { IP: defaultMsg, Uid: defaultMsg, Pwd: defaultMsg };

        if (status == "success" && !!data && !!data.Result && data.Result.length > 0) {
            for (var i = 0; i < data.Result.length; i++)
                vpn[data.Result[i].Key] = data.Result[i].Value;
        }

        var html = $("#tmp-vpn").render(vpn);
        $("#vpn").html(html);
    });

    $.getJSON("/Tools/GetShadowSocks", {}, function (data, status) {
        $(".progress-sdws").addClass("hidden");
        var defaultMsg = "暂不可用"
        var sdw = { Addr: defaultMsg, Port: defaultMsg, Pwd: defaultMsg, Encry: defaultMsg };

        if (status == "success" && !!data && !!data.Result && data.Result.length > 0) {
            var getDict = function (obj) {
                var dict = {};
                for (var i = 0; i < obj.length; i++)
                    dict[obj[i].Key] = obj[i].Value;
                return dict;
            }
            
            $("#sdws-us").html($("#tmp-sdws").render(getDict(data.Result[0])));
            $("#sdws-hk").html($("#tmp-sdws").render(getDict(data.Result[1])));
            $("#sdws-jp").html($("#tmp-sdws").render(getDict(data.Result[2])));
        }
        else {
            var html = $("#tmp-sdws").render(sdw);
            $("#sdws-us").html(html);
            $("#sdws-hk").html(html);
            $("#sdws-jp").html(html);
        }
    });
});