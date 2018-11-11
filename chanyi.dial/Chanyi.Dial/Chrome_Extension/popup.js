$(function () {
	var ext = chrome.extension.getBackgroundPage();

	$('#uname').val(localStorage.uname);
	$('#serverState').html(ext.hub.getConnectionState());
	$('#serverIp').val(localStorage.serverIp || "192.168.1.100");
	$('#serverPort').val(localStorage.serverPort || "20003");
	// $('#interval').val(localStorage.interval || 0);
	chrome.tabs.getSelected(null, function (tab) {
		var _timer = ext.timers.get(tab.id);
		_timer ? $('#interval').val(parseFloat(_timer.interval) / 1000) : 0;
	});

	$('#btnSubmit').click(function () {
		var uname = $('#uname').val() || '';
		var interval = $('#interval').val();
		var serverIp = $('#serverIp').val();
		var serverPort = $('#serverPort').val();

		if (uname.length <= 0) {
			send('用户名不能为空');
			return;
		}

		localStorage.interval = interval;
		if (serverIp != localStorage.serverIp) {
			localStorage.serverIp = serverIp;
		}
		if (serverPort != localStorage.serverPort) {
			localStorage.serverPort = serverPort;
		}

		ext.hub.server_uname = uname;
		if (uname != localStorage.uname || !ext.hub.isConnected()) {
			ext.hub.connect(function () {
				ext.hub.registerCallBack = function (msg) {
					send(msg);
				};
				ext.hub.registerUserToServer(uname);
				$('#serverState').html(ext.hub.getConnectionState());
				updateRefreshTimer();
			}, function (err) {
				send('连接服务失败，请启动服务后重试！');
			});
		}else{
			updateRefreshTimer();
		}
	});

	function updateRefreshTimer() {
		chrome.tabs.query({ active: true, currentWindow: true }, function (tabs) {
			var interval = parseFloat(localStorage.interval) * 1000;
			if (interval > 0) {
				ext.timers.set(tabs[0], interval);
			} else {
				ext.timers.remove(tabs[0].id);
			}
			send('保存成功！');
		});
	}

	function send(msg) {
		$('#msg').text(msg + '  '
			+ (new Date()).toLocaleDateString() + ' ' + (new Date()).toLocaleTimeString());
	}
});