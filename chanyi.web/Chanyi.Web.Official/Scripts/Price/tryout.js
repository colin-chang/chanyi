$(function () {
    //下载文件
    (function () {
        $(".btn-download").click(function () {
            //防止表单提交
            $(".form-customer").submit(function () { return false });

            //过滤第一次自动提交
            if (autoClick) {
                autoClick = false;
                return;
            }

            //校验是否通过
            var errors = [];
            $(".validationSummary .field-validation-error").each(function () {
                if (!!$(this).html())
                    errors[errors.length] = $(this);
            });
            if (errors.length > 0) {
                errors[0].show();
                return;
            }

            var cw = $(".frame-download")[0].contentWindow;
            cw.download($(".form-customer").serializeArray());
            if (!!browser.Safari)//解决Safari兼容问题
                return;
            showProgress();
            var timer = setInterval(function () {
                try {
                    var result = $.cookie("tryoutResult");
                    console.log(result);
                    if (!result)
                        return;

                    hideProgress();
                    tryoutFail(JSON.parse(result));
                    clearInterval(timer);
                }
                catch (ex) {
                    hideProgress();
                    clearInterval(timer);
                    console.log(ex.message);
                }
            }, 500);
        });

        //自动点击触发校验，解决页面首次加载完成直接提交时校验结果尚未返回先提交表单的问题
        var autoClick = true;
        $(".btn-download").click();
    })();
});

//进度条控制
function showProgress() {
    var progress = $(".progress");
    if (!progress)
        return;
    progress.removeClass("hidden");
}

function hideProgress() {
    var progress = $(".progress");
    if (!progress)
        return;
    if (progress.hasClass("hidden"))
        return;
    progress.addClass("hidden");
}

//试用下载失败
function tryoutFail(data) {
    this.hideProgress();
    var msg = "抱歉，试用版产品下载失败，请联系客服人员获取试用版产品！";
    switch (data.Code) {
        case 100://成功
        case 101: return;//重下
        case 120: msg = "抱歉，您的使用时间已经结束，购买正版程序请联系客服人员！"; break;//过期
        case 300: msg = "用户信息填写错误，请检查后重试！"; break;//校验错误
        case 110://异常
        case 111: break;//未知错误
        default: break;
    }

    msgBox(msg);
}