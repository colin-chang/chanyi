(function () {
	var _debug = false;

	function Hub() {
		this.proxy;
		this.connection;

		this.registerCallBack = function () { };

		this.server_uname;

		this.getConnectUrl = function () {
			return "http://" + localStorage.serverIp + ":" + localStorage.serverPort;
		}

		this.getDomain = function () {
			return "http://" + localStorage.serverIp + "/";
		}

		this.init = function () {
			this.connection = $.hubConnection(this.getConnectUrl());

			//创建并设置代理
			this.proxy = this.connection.createHubProxy("Transmitter");
			this.proxy.state.UserAgent = "web";
			this.connection.logging = true;

			this.connection && this.connection.stateChanged(this.stateChanged);
			this.connection && this.connection.reconnected(this.reconnected);
		}

		this.connect = function (successCallBack, failCallBack) {
			this.init();
			this.connection.start().done(successCallBack).fail(failCallBack);
		}

		this.isConnected = function () {
			_debug && console.log('hub connection state = ' + (hub.connection && hub.connection.state));

			return (this.connection && this.connection.state == $.signalR.connectionState.connected);
		}

		this.getConnectionState = function () {
			return this.isConnected() == true ? "<span style='color:green'>已连接</span>" : "<span style='color:red'>未连接</span>";
		}

		this.stateChanged = function (change) {
			if (change.newState === $.signalR.connectionState.reconnecting) {
				_debug && console.log('Re-connecting');
			} else if (change.newState === $.signalR.connectionState.connected) {
				_debug && console.log('The server is online');
			}
		};

		this.reconnected = function () {
			console.log('Reconnected');

			hub.server_uname = localStorage.uname;
			hub.registerUserToServer(hub.server_uname);
		}

		this.registerUserToServer = function () {
			// this是hub对象
			this.proxy.invoke("isUserNameEnabled", hub.server_uname).done(this.addWebUser).fail(this.proxyWorkFail);
		}

		this.addWebUser = function (result) {
			if (!!result) {
				// this是proxy对象
				this.invoke("addWebUser", hub.server_uname).done(hub.setUserCookie).fail(hub.proxyWorkFail);
			} else {
				var msg = "用户名已被占用，请修改后重试~";

				hub.registerCallBack(msg);
			}
		}

		this.setUserCookie = function (result) {
			chrome.cookies.set({
				"name": "UserId", "url": hub.getDomain(), "value": hub.server_uname
			}, function (cookie) {
				if (!cookie) {
					var msg = "向服务端注册后写入cookie失败，请检查manifest是否配置写入domain权限";
					hub.registerCallBack(msg);
				} else {
					localStorage.uname = hub.server_uname;
					hub.registerCallBack("保存成功");

					_debug && console.log('hub.js Set Cookie Ok When Add Web User : ' + JSON.stringify(cookie));
				}
			});
		}

		this.proxyWorkFail = function (err) {
			this.connection.log(err);
		}

		this.dial = function (message) {
			if (!this.proxy) {
				this.connect(function () {
					hub.proxy.invoke("Dial", message).done(function (result) {	//抓取电话号码
						console.log("Dial invoke result :" + result);
					}).fail(function (err) {
						_debug && console.log(err);
					});
				}, function (err) {
					_debug && console.log(err);
				});
			} else {
				hub.proxy.invoke("Dial", message).done(function (result) {	//抓取电话号码
					_debug && console.log("Dial invoke result :" + result);
				}).fail(function (err) {
					_debug && console.log(err);
				});
			}

		}
	}

	window.hub = new Hub();
})();
