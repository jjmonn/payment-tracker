$(document).ready(function () {

    var uri = "api/invoices/"

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

            $('<li>', { text: 'Total overdue EUR: € ' + Math.round(total_amount_EUR).toString() }).appendTo($('#total_overdue'));
            $('<li>', { text: 'Total overdue USD: $ ' + Math.round(total_amount_USD).toString() }).appendTo($('#total_overdue'));
            $('<li>', { text: 'Total overdue GBP: £ ' + Math.round(total_amount_GBP).toString() }).appendTo($('#total_overdue'));
            $('<p>', { text: '' }).appendTo($('#total_overdue'));
        });
});


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
        ajaxHelper(invoicesUri + '/nonpaids/', 'GET').done(function (data) {
            self.invoices(data);
            self.computeAmountsToBePaid(data);
        });
    }


    self.UploadInvoices = function (invoice) {
        ajaxHelper(invoicesUri + '/' + invoice.InvoiceID, 'PUT', invoice).done(function (data) {
            getNonPaidInvoices();
        });
        //$.ajax({ type: "PUT", url: invoicesUri + '/' + invoice.InvoiceId, data: invoice });
    }

    self.computeAmountsToBePaid = function (data) {

        amountEUR = 0;
        amountUSD = 0;
        amountGBP = 0;

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
        });

        this.toBePaidEUR(amountEUR);
        this.toBePaidUSD(amountUSD);
        this.toBePaidGBP(amountGBP);
    }



    // Fetch the initial data.
    getNonPaidInvoices();
};

ko.applyBindings(new ViewModel());