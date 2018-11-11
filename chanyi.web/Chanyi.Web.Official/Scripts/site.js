/// <reference path="jquery-1.11.3.min.js" />
//------------------------------------- 浏览器兼容性调整 ----------------------------------------------------
//判断浏览器
var browser = {};
var ua = navigator.userAgent.toLowerCase();
try {
    if (window.ActiveXObject || /trident/.test(ua))
        browser.IE = /msie/.test(ua) ? ua.match(/msie ([\d.]+)/)[1] : ua.match(/trident\/([\d.]+)/)[1] * 1 + 4;
    else if (/firefox\/([\d.]+)/.test(ua) || document.getBoxObjectFor)
        browser.Firefox = ua.match(/firefox\/([\d.]+)/)[1]
    else if (window.MessageEvent && !document.getBoxObjectFor && /chrome\/([\d.]+)/.test(ua))
        browser.Chrome = ua.match(/chrome\/([\d.]+)/)[1]
    else if (window.opera)
        browser.Opera = ua.match(/opera.([\d.]+)/)[1]
    else if (window.openDatabase)
        browser.Safari = ua.match(/safari\/([\d.]+)/)[1];
}
catch (err) {
    console.log(err.message);
}

//兼容移动端IE 10.0
if (navigator.userAgent.match(/IEMobile\/10\.0/)) {
    var msViewportStyle = document.createElement('style');
    msViewportStyle.appendChild(document.createTextNode('@-ms-viewport{width:auto!important}'));
    document.querySelector('head').appendChild(msViewportStyle);
}

$(function () {
    //兼容Android自带浏览器
    var nua = navigator.userAgent;
    var isAndroid = (nua.indexOf('Mozilla/5.0') > -1 && nua.indexOf('Android ') > -1 && nua.indexOf('AppleWebKit') > -1 && nua.indexOf('Chrome') === -1);
    if (isAndroid)
        $('select.form-control').removeClass('form-control').css('width', '100%');

    //适配IE9以下placeholder兼容问题
    if (!!browser.IE && browser.IE * 1 <= 9)
        $(":text").each(function () {
            var placeholder = $(this).attr("placeholder");
            $(this).focus(function () {
                $(this).removeClass("placeholder");
                if (!$(this).val() || $(this).val() == placeholder)
                    $(this).val("").removeClass("placeholder");
            }).blur(function () {
                if (!$(this).val())
                    $(this).val(placeholder).addClass("placeholder");
                else
                    $(this).removeClass("placeholder");
            }).blur();
        });
});



//------------------------------------- 初始化参数 ----------------------------------------------------
$(function () {
    //初始化Headroom
    if (document.documentElement.clientWidth < 768)
        $(".navbar[data-headroom='']").headroom();

    //初始化导航状态
    (function () {
        var localUrl = getLocalUrl().toLocaleLowerCase();
        $(".nav-main a").each(function () {
            if ($(this).attr("href").toLocaleLowerCase() == localUrl)
                $(this).parent().addClass("active").siblings().removeClass("active");
        });
    })();
});

//------------------------------------- 辅助功能函数 ----------------------------------------------------
//获取当前Url的站内地址
function getLocalUrl() {
    var url = location.href.replace(location.search, '');
    var parts = /^https?:\/\/(\w+(\.\w+)*)(:\d{1,5})?(\/(\w+))?(\/(\w+))?.*$/.exec(url);
    var controller = !parts[5] ? "Home" : parts[5];
    var action = !parts[7] ? "Index" : parts[7];
    return "/" + controller + "/" + action;
}

//消息提示框
function msgBox(message, type) {
    Messenger.options = {
        extraClasses: 'messenger-fixed messenger-on-top',
    }
    //type = !type ? 'error' : type;
    Messenger().post({
        message: message,
        type: !type ? 'error' : type,
        showCloseButton: true
    })
}

//------------------------------------- 第三方插件 ----------------------------------------------------
//百度分享
window._bd_share_config={"common":{"bdSnsKey":{},"bdText":"我发现这个网站[http://chanyikeji.com]还不错，分享给大家看下！","bdMini":"2","bdMiniList":false,"bdPic":"我发现这张图片还不错，分享给大家看下！","bdStyle":"0","bdSize":"16"},"slide":{"type":"slide","bdImg":"6","bdPos":"left","bdTop":"100"},"image":{"viewList":["qzone","tsina","tqq","weixin","sqq","renren"],"viewText":"分享到：","viewSize":"16"},"selectShare":{"bdContainerClass":null,"bdSelectMiniList":["qzone","tsina","tqq","weixin","sqq","renren"]}};with(document)0[(getElementsByTagName('head')[0]||body).appendChild(createElement('script')).src='http://bdimg.share.baidu.com/static/api/js/share.js?v=89860593.js?cdnversion='+~(-new Date()/36e5)];