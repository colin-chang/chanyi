/// <reference path="../jquery-1.10.2.min.js" />
//问题点击效果
$(function () {
    $(".question .pop").click(function () {
        var q = $(this).children().first();
        $(this).addClass("pop-active").siblings().removeClass("pop-active");
        var p = $("p#" + q.attr("href").replace("#", ''));
        p.addClass("active").siblings().removeClass("active");


        if (document.documentElement.clientWidth < 768) {
            $(".answer").remove();
            $("<div class='answer text-info'>" + p.text() + "</div>").insertAfter($(this));
        }

        return false;
    }).first().click();
});

