var uri = "api/invoices/nonpaids/All"

var invoices;


function formatItem(item) {
    return item.SupplierName + '  -  ' + item.Currency + '  -  ' + item.DueAmount + '  -  ' + moment(item.DueDate).format('MMM Do YY');
}


var ViewModel = function () {
    var self = this;
    self.invoices = ko.observableArray();
    self.error = ko.observable();

    // Amounts to be paid
    this.toBePaidEUR = ko.observable(0).money('€');
    this.toBePaidUSD = ko.observable(0).money('$');
    this.toBePaidGBP = ko.observable(0).money('£');

    // Amounts overdue
    this.OverdueEUR = ko.observable(0).money('€');
    this.OverdueUSD = ko.observable(0).money('$');
    this.OverdueGBP = ko.observable(0).money('£');


    var invoicesUri = '/api/invoices/';

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message
        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    function getNonPaidInvoices() {
        ajaxHelper(invoicesUri + '/nonpaids/All', 'GET').done(function (data) {
            self.invoices(data);
            self.computeAmountsToBePaid(data);

            self.loadCharts()
        });
    }

  // use new method in invoices.js + transfer it in util.js
    self.computeAmountsToBePaid = function (data) {

        amountEUR = 0;
        amountUSD = 0;
        amountGBP = 0;

        totalEUR = 0;
        totalUSD = 0;
        totalGBP = 0;

        data.forEach(function (item) {
            if (item.Paid === false && item.ToBePaid === true) {
                switch (item.Currency) {
                    case "EUR": amountEUR += item.DueAmount;
                        break;

                    case "USD": amountUSD += item.DueAmount;
                        break;

                    case "GBP": amountGBP += item.DueAmount;
                        break;
                }
            }
            if (item.IsSupplierInterco === false) {
                switch (item.Currency) {
                    case "EUR": totalEUR += item.DueAmount;
                        break;

                    case "USD": totalUSD += item.DueAmount;
                        break;

                    case "GBP": totalGBP += item.DueAmount;
                        break;
                }
            }
        });

        this.toBePaidEUR(amountEUR);
        this.toBePaidUSD(amountUSD);
        this.toBePaidGBP(amountGBP);

        this.OverdueEUR(totalEUR);
        this.OverdueUSD(totalUSD);
        this.OverdueGBP(totalGBP);
    }

    function drawAnnotations() {
 
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'date');
        data.addColumn('number', 'Overdue');
        data.addColumn('number', 'Due');
        data.addColumn('number', 'No due yet');

        var data_usd = new google.visualization.DataTable();
        data_usd.addColumn('string', 'date');
        data_usd.addColumn('number', 'Overdue');
        data_usd.addColumn('number', 'Due');
        data_usd.addColumn('number', 'No due yet');

        var data_gbp = new google.visualization.DataTable();
        data_gbp.addColumn('string', 'date');
        data_gbp.addColumn('number', 'Overdue');
        data_gbp.addColumn('number', 'Due');
        data_gbp.addColumn('number', 'No due yet');

        var _today = new Date();
        var l_today_plus_four = new Date();

        self.invoices().forEach(function (l_invoice) {
            var _date = new Date(l_invoice.DueDate);
            l_today_plus_four.setDate(_today.getDate() + 4)
            var l_overdue =0;
            var l_due= 0;
            var l_not_due = 0;
            if (_date.getFullYear() >= _today.getFullYear() && l_invoice.IsSupplierInterco === false) {
                // data.addRow([new Date(l_invoice.DueDate), l_invoice.DueAmount]);
                if (_date <= _today) {
                    l_overdue = l_invoice.DueAmount;
                } else if (_date > l_today_plus_four) {
                    l_not_due = l_invoice.DueAmount;
                } else {
                    l_due = l_invoice.DueAmount;
                }
                var yearWeek = moment(new Date(l_invoice.DueDate)).year() + '-' + moment(new Date(l_invoice.DueDate)).week();
                if (l_invoice.Currency === "EUR") {
                    data.addRow([yearWeek, l_overdue, l_due, l_not_due]);
                } else if (l_invoice.Currency === "USD") {
                    data_usd.addRow([yearWeek, l_overdue, l_due, l_not_due]);
                } else {
                    data_gbp.addRow([yearWeek, l_overdue, l_due, l_not_due]);
                }

            }
        });

       
        var result = google.visualization.data.group(
            data,
            [{
                column: 0,
                label: 'Week',
                type: 'string'
            }],[{column: 1, aggregation: google.visualization.data.sum, label: 'Over due', type: 'number'},
            {column: 2,aggregation: google.visualization.data.sum,label: 'Due', type: 'number'},
            {column: 3,aggregation: google.visualization.data.sum,label: 'Not due yet', type: 'number'}]
        );

        var result_usd = google.visualization.data.group(
            data_usd,
            [{
                column: 0,
                label: 'Week',
                type: 'string'
            }], [{ column: 1, aggregation: google.visualization.data.sum, label: 'Over due', type: 'number' },
            { column: 2, aggregation: google.visualization.data.sum, label: 'Due', type: 'number' },
            { column: 3, aggregation: google.visualization.data.sum, label: 'Not due yet', type: 'number' }]
        );

        var result_gbp = google.visualization.data.group(
            data_gbp,
            [{
                column: 0,
                label: 'Week',
                type: 'string'
            }], [{ column: 1, aggregation: google.visualization.data.sum, label: 'Over due', type: 'number' },
            { column: 2, aggregation: google.visualization.data.sum, label: 'Due', type: 'number' },
            { column: 3, aggregation: google.visualization.data.sum, label: 'Not due yet', type: 'number' }]
        );

        var options = {
            colors: ['#FF0066', '#0033FF', "#00FFCC"],
            title: 'Due Invoices EUR',
            'height': 400,
            annotations: {
                alwaysOutside: true,
                textStyle: {fontSize: 12,color: '#000',auraColor: 'none'}
            },hAxis: {
                title: 'Week', gridlines: {color: 'transparent'}
            },vAxis: {title: '€'}
        };

        var options_usd = {
            colors: ['#D11D29', '#1F5AA6', "#00FFCC"],
            title: 'Due Invoices USD',
            'height': 400,
            annotations: {
                alwaysOutside: true,
                textStyle: { fontSize: 12, color: '#000', auraColor: 'none' }
            }, hAxis: {
                title: 'Week', gridlines: { color: 'transparent' }
            }, vAxis: { title: '$' }
        };

        var options_gbp = {
            colors: ['#D11D29', '#1F5AA6', "#00FFCC"],
            title: 'Due Invoices GBP',
            'height': 400,
            annotations: {
                alwaysOutside: true,
                textStyle: { fontSize: 12, color: '#000', auraColor: 'none' }
            }, hAxis: {
                title: 'Week', gridlines: { color: 'transparent' }
            }, vAxis: { title: '£' }
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
        var chart_usd = new google.visualization.ColumnChart(document.getElementById('chart_div_usd'));
        var chart_gbp = new google.visualization.ColumnChart(document.getElementById('chart_div_gbp'));
        chart.draw(result, options);
        chart_usd.draw(result_usd, options_usd);
        chart_gbp.draw(result_gbp, options_gbp);
    }

    self.loadCharts = function () {
        google.charts.load('current', { packages: ['corechart', 'bar'] });
        google.charts.setOnLoadCallback(drawAnnotations);
    }

    // Fetch the initial data.
    getNonPaidInvoices();

};



//doc ready function
$(document).ready(function () {

    ko.applyBindings(new ViewModel());

});







function getMonday(d) {
    d = new Date(d);
    var day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
    return new Date(d.setDate(diff));
}


// Put this in view model - simplify
//$(document).ready(function () {

//    // Send an AJAX request
//    $.getJSON(uri)
//        .done(function (data) {
//            // On success, 'data' contains a list of invoices.
//            total_amount_EUR = 0;
//            total_amount_USD = 0;
//            total_amount_GBP = 0;

//            invoices = [];

//            $.each(data, function (key, item) {
//                if (item.IsSupplierInterco == false) {
//                    invoices.push(item);

//                    var _date = new Date(item.DueDate);
//                    if (_date <= Date.now() && item.Paid === false) {
//                        $('<li>', { text: formatItem(item) }).appendTo($('#invoices'));
//                        switch (item.Currency) {
//                            case "EUR": total_amount_EUR += item.DueAmount;
//                                break;

//                            case "USD": total_amount_USD += item.DueAmount;
//                                break;

//                            case "GBP": total_amount_GBP += item.DueAmount;
//                                break;
//                        }
//                    }
//                }
//            });

//            $('<li>', { text: 'Total overdue EUR: € ' + Math.round(total_amount_EUR).toString() }).appendTo($('#total_overdue'));
//            $('<li>', { text: 'Total overdue USD: $ ' + Math.round(total_amount_USD).toString() }).appendTo($('#total_overdue'));
//            $('<li>', { text: 'Total overdue GBP: £ ' + Math.round(total_amount_GBP).toString() }).appendTo($('#total_overdue'));
//            $('<p>', { text: '' }).appendTo($('#total_overdue'));
//        });
//});
