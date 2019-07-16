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
        data.addColumn('date', 'date');
        data.addColumn('number', 'Due invoices');
        data.addColumn({ type: 'number', role: 'annotation' });

        var _today = new Date();

        self.invoices().forEach(function (l_invoice) {
            var _date = new Date(l_invoice.DueDate);
            if (_date.getFullYear() >= _today.getFullYear()) {
                data.addRow([new Date(l_invoice.DueDate), l_invoice.DueAmount, l_invoice.DueAmount]);
            }
        });

        var result = google.visualization.data.group(
            data,
            [{ column: 0, modifier: getWeeks, type: 'date' }],
            [{ 'column': 1, 'aggregation': google.visualization.data.sum, 'type': 'number' }]
        );

        function getWeeks(p_date) {
            //d = new Date(someDate);
            var day = p_date.getDay(),
                diff = p_date.getDate() - day + (day == 0 ? -6 : 1) + 2; // adjust when day is sunday
            var _wednesday = new Date(p_date.setDate(diff));
            return { v: new Date(_wednesday), f: _wednesday.toDateString() };
        }

        function getMonths(someDate) {
            var month = someDate.getMonth();
            var year = someDate.getFullYear();
            return { v: new Date(year, month), f: (month + 1) + '/' + year };
        }

        var options = {
            title: 'Due Invoices Planning',
            'height': 400,
            annotations: {
                alwaysOutside: true,
                textStyle: {
                    fontSize: 12,
                    color: '#000',
                    auraColor: 'none'
                }
            },
            hAxis: {
                title: 'Date',
                //format: 'h:mm a',
                //viewWindow: {
                //    min: [7, 30, 0],
                //    max: [17, 30, 0]
                //},
                gridlines: {
                    color: 'transparent'
                }
            },
            vAxis: {
                title: '€'
            }
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('chart_div'));
        chart.draw(result, options);
    }

    // Fetch the initial data.
    getNonPaidInvoices();
    //drawAnnotations();

    wait(2500);
    google.charts.load('current', { packages: ['corechart', 'bar'] });
    google.charts.setOnLoadCallback(drawAnnotations);

};



ko.applyBindings(new ViewModel());

function wait(ms) {
    var d = new Date();
    var d2 = null;
    do { d2 = new Date(); }
    while (d2 - d < ms);
}




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
