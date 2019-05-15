var ViewModel = function () {
    var self = this;
    self.invoices = ko.observableArray();
    self.error = ko.observable(); 
    self.availableCurrencies = ko.observableArray(['All', 'EUR', 'USD', 'GBP']);
    self.currencyFilter = ko.observable('All');
    self.intercoFilter = ko.observable();

    this.overdueAmountEUR = ko.observable(0).money('€');
    this.overdueAmountUSD = ko.observable(0).money('$');
    this.overdueAmountGBP = ko.observable(0).money('$');

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
        l_currency_filter = self.currencyFilter();
        ajaxHelper(invoicesUri + '/nonpaids/' + l_currency_filter, 'GET').done(function (data) {
            self.invoices(data);
            self.computeAmountsToBePaid(data);
        });
    }

    // update the users IsConfirmed status
    self.updateInvoice = function (invoice) {
        ajaxHelper(invoicesUri + '/' + invoice.InvoiceID, 'PUT', invoice).done(function (data) {
            self.computeAmountsToBePaid(self.invoices());
            // getNonPaidInvoices();
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

    self.computeAmountsOverdue = function (data) {

        amountEUR = 0;
        amountUSD = 0;
        amountGBP = 0;

        data.forEach(function (item) {
            switch (item.Currency) {
                case "EUR": amountEUR += item.DueAmount;
                break;

                case "USD": amountUSD += item.DueAmount;
                break;

                case "GBP": amountGBP += item.DueAmount;
                break;
            }
        });

        this.overdueAmountEUR(amountEUR);
        this.overdueAmountUSD(amountUSD);
        this.overdueAmountGBP(amountGBP);
    }

    // Interco Filter
    self.filterInvoices = ko.computed(function () {
        if (!self.intercoFilter()) {
            return self.invoices();
        } else {
            return ko.utils.arrayFilter(self.invoices(), function (l_invoice) {
                return l_invoice.IsSupplierInterco != self.intercoFilter();
            });
        }
    });

    // Overdue Invoices
    self.overdueInvoices = ko.computed(function () {
        _today = Date.now();
        l_result = [];

        if (!self.intercoFilter()) {
            l_result = ko.utils.arrayFilter(self.invoices(), function (l_invoice) {
                return Date.parse(l_invoice.DueDate) <= _today;
            });
        } else {
            var l_temp = ko.utils.arrayFilter(self.invoices(), function (l_invoice) {
                return l_invoice.IsSupplierInterco != self.intercoFilter();
            });
            l_result = ko.utils.arrayFilter(l_temp, function (l_invoice) {
                return Date.parse(l_invoice.DueDate) <= _today;
            });
        }

        self.computeAmountsOverdue(l_result);
        return l_result;
    });

    self.formatNumber = function (_str) {
        return _str.toFixed(0);
    }

    // Currency Filter
    self.filterCurrencies = function (obj, event) {
        test = self.currencyFilter();
        getNonPaidInvoices();
    };
     
    self.setAllOverDueToBePaid = function () {



    }


    this.toggleInterco = function() {
        self.intercoFilter(!self.intercoFilter());
    }
  
    // Fetch the initial data.
    self.intercoFilter(true);
    self.currencyFilter = ko.observable('All');
    getNonPaidInvoices();
    

};

ko.applyBindings(new ViewModel());