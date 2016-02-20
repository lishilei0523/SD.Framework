/*
    jquery漂浮广告插件，
    调用方法：
    把广告内空（如图片）放在div里面，如<div id="div_float">这里是广告内容</div>，在页面里采用如下方法调用
    $(function() {
        $("#div_float").adFloat({ width: 228, height: 88, top: 30, left: 20 });
    })
*/
(function($) {

    $.fn.adFloat = function(options) {
        return new AdFloat(this, options);
    };

    var AdFloat = function(element, options) {
        this.element = $(element);
        this.options = $.extend({
            width: 100,         //广告容器的宽度
            height: 100,        //广告容器的高度
            top: 0,            //起始top位置
            left: 0,            //超始left位置
            delay: 30,          //延迟
            step: 1             //每次移多少
        }, options || {});

        this.interval = null;
        this.xPos = this.options.left;
        this.yPos = this.options.top;
        this.yon = 0;
        this.xon = 0;
        this.isPause = false;

        this.init();
    };
    AdFloat.prototype = {
        init: function() {
            var me = this;
            
            me.element.css("display", "block");
            me.element.css({ position: "absolute", left: me.options.left + "px", top: me.options.top + "px", width: me.options.width + "px", height: me.options.height + "px", overflow: "hidden" });
            me.element.hover(
                    function() { clearInterval(me.interval) },
                    function() { me.interval = setInterval(function() { me.changePos(); }, me.options.delay); }
            );
            $(document).ready(function() { me.start(); });
        },

        changePos: function() {
            var me = this;

            var clientWidth = document.documentElement.clientWidth;
            var clientHeight = document.documentElement.clientHeight;
            var Hoffset = me.element.attr("offsetHeight");
            var Woffset = me.element.attr("offsetWidth");

            me.element.css({ left: me.xPos + document.documentElement.scrollLeft, top: me.yPos + document.documentElement.scrollTop });

            if (me.yon)
                me.yPos = me.yPos + me.options.step;
            else
                me.yPos = me.yPos - me.options.step;

            if (me.yPos < 0) {
                me.yon = 1; me.yPos = 0;
            }
            if (me.yPos >= (clientHeight - Hoffset)) {
                me.yon = 0; me.yPos = (clientHeight - Hoffset);
            }
            if (me.xon)
                me.xPos = me.xPos + me.options.step;
            else
                me.xPos = me.xPos - me.options.step;

            if (me.xPos < 0) {
                me.xon = 1; me.xPos = 0;
            }
            if (me.xPos >= (clientWidth - Woffset)) {
                me.xon = 0; me.xPos = (clientWidth - Woffset);
            }
        },
        start: function() {
            var me = this;
            me.element.css("top", me.yPos);
            me.interval = setInterval(function() { me.changePos(); }, me.options.delay);
        }
    };
})(jQuery)