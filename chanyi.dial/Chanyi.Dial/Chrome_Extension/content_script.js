$(function () {
	var bindObject = [
		'.img_list a',			// 58图片
		'.tbimg a.t',			// 58标题
		'a.img-box',			// 赶集图片
		'a.list-info-title'	// 赶集标题
	];

	var _debug = false;

	function domainURI(str) {
		var durl = /http:\/\/([^\/]+)\//i;
		domain = str.match(durl);

		if (domain == null) {
			str = 'http://' + location.host + str;
		}
		return str;
	}

	for (var i = 0; i < bindObject.length; i++) {
		$(document).on('click', bindObject[i], function () {
			var host = location.host;
			var href = $(this).attr('href');

			href = domainURI(href);

			var data = { cmd: 'dial', data: href };
			_debug && console.log('Content Script Clicked :' + href);
			chrome.runtime.sendMessage(data, function (res) {
				_debug && console.log(res);
			});
		});
	}
});