chrome.runtime.onMessage.addListener(function (req, sender, sendResponse) {
	if (req.cmd == "dial") {
		hub.dial(req.data);
	}
});

// chrome.extension.onRequest.addListener(function (req, sender, sendResponse) {
// 	if (req.cmd == 'getConf') {
// 		chrome.tabs.getSelected(null, function (tab) {
// 			var task = customTabs[tab.id];
// 			_debug && console.log('getConf', tab.id, task);
// 			if (!task) {
// 				task = refreshCreate(tab.id, tab.url);
// 			}
// 			task && sendResponse({
// 				// 'stat': task.stat,
// 				'elapsed': task.elapsed,
// 				'interval': task.interval
// 			});
// 		});
// 	}
// });


function showOrHideIcon(tabID) {
	chrome.tabs.getSelected(null, function (tab) {
		if (tab.url.indexOf('58.com') > -1 || tab.url.indexOf('ganji.com') > -1) {
			chrome.pageAction.show(tabID);
		} else {
			chrome.pageAction.hide(tabID);
		}
	})
}

chrome.tabs.onUpdated.addListener(function (tabID, undefined, tab) {
	showOrHideIcon(tabID);
});

chrome.tabs.onSelectionChanged.addListener(function (tabID, undefined, tab) {
	showOrHideIcon(tabID);
});
