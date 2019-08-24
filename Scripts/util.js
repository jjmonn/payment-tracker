


// ko sorting columns (add to <th)
ko.bindingHandlers.sort = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var asc = false;
        element.style.cursor = 'pointer';

        element.onclick = function () {
            var value = valueAccessor();
            var prop = value.prop;
            var data = value.arr;

            asc = !asc;

            data.sort(function (left, right) {
                var rec1 = left;
                var rec2 = right;

                if (!asc) {
                    rec1 = right;
                    rec2 = left;
                }

                var props = prop.split('.');
                for (var i in props) {
                    var propName = props[i];
                    var parenIndex = propName.indexOf('()');
                    if (parenIndex > 0) {
                        propName = propName.substring(0, parenIndex);
                        rec1 = rec1[propName]();
                        rec2 = rec2[propName]();
                    } else {
                        rec1 = rec1[propName];
                        rec2 = rec2[propName];
                    }
                }

                return rec1 == rec2 ? 0 : rec1 < rec2 ? -1 : 1;
            });
        };
    }
};


// ko .money();
(function () {
    var format = function (value, symbol) {
        toks = value.toFixed(2).replace('-', '').split('.');
        var display = symbol + ' ' + $.map(toks[0].split('').reverse(), function (elm, i) {
            return [(i % 3 === 0 && i > 0 ? ',' : ''), elm];
        }).reverse().join('') + '.' + toks[1];

        return value < 0 ? '-' + display : display;
    };

    ko.subscribable.fn.money = function (symbol) {
        var target = this;

        var writeTarget = function (value) {
            var stripped = value.toString().replace(/[^0-9\.]+/g, '');

            target(parseFloat(stripped));
        };

        var result = ko.computed({
            read: function () {
                return target();
            },
            write: writeTarget
        });

        result.formatted = ko.computed({
            read: function () {
                return format(target(), symbol);
            },
            write: writeTarget
        });

        result.isNegative = ko.computed(function () {
            return target() < 0;
        });

        return result;
    };
})();




//success notice
function successNotice(p_title, p_text, p_time, p_icon) {
    $.gritter.add({
        // (string | mandatory) the heading of the notification
        title: p_title,
        // (string | mandatory) the text inside the notification
        text: p_text,
        // (int | optional) the time you want it to be alive for before fading out
        time: p_time,
        // (string) specify font-face icon  class for close message
        close_icon: 'l-arrows-remove s16',
        // (string) specify font-face icon class for big icon in left. if are specify image this will not show up.
        icon: p_icon,
        // (string | optional) the class name you want to apply to that specific message
        class_name: 'success-notice'
    });
}


function endOfWeek() {

    var _today = new Date(Date.now());
    var d = _today.getDate();
    var _day = _today.getDay();
    var lastday = d - (_day - 1) + 6;
    return new Date(_today.setDate(lastday));

}


function endOfNextWeek() {

    var _today = new Date(Date.now());
    var d = _today.getDate();
    var _day = _today.getDay();
    var lastday = d - (_day - 1) + 6;
    return new Date(_today.setDate(lastday + 7));

}


function wait(ms) {
    var d = new Date();
    var d2 = null;
    do { d2 = new Date(); }
    while (d2 - d < ms);
}