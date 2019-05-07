
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


var uri = 'api/invoices';

$(document).ready(function () {
    // Send an AJAX request
    $.getJSON(uri)
        .done(function (data) {
            // On success, 'data' contains a list of products.
            total_amount_EUR = 0;
            total_amount_USD = 0;
            total_amount_GBP = 0;

            $.each(data, function (key, item) {
                // Add a list item for the product.
                _date = new Date(item.DueDate);
                if (_date <= Date.now() && item.Paid === false) {
                    $('<li>', { text: formatItem(item) }).appendTo($('#invoices'));
                    switch (item.Currency) {
                        case "EUR": total_amount_EUR += item.DueAmount;
                            break;

                        case "USD": total_amount_USD += item.DueAmount;
                            break;

                        case "GBP": total_amount_GBP += item.DueAmount;
                            break;
                    }
                }
            });
            $('<p>', { text: '' }).appendTo($('#invoices'));
            $('<li>', { text: 'Total overdue EUR: € ' + total_amount_EUR }).appendTo($('#invoices'));
            $('<li>', { text: 'Total overdue USD: $ ' + total_amount_USD }).appendTo($('#invoices'));
            $('<li>', { text: 'Total overdue GBP: £ ' + total_amount_GBP }).appendTo($('#invoices'));

        });
});

function formatItem(item) {
    return item.SupplierName + ' - ' + item.Currency + ' ' + item.DueAmount + ': - ' + moment(item.DueDate).format('MMM Do YY');
}