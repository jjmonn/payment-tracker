var ViewModel = function () {
    var self = this;
    self.invoices = ko.observableArray();
    self.currentInvoice = ko.observable(null);
    self.error = ko.observable(); 
    self.availableCurrencies = ko.observableArray(['All', 'EUR', 'USD', 'GBP']);
    self.currencyFilter = ko.observable('All');
    self.intercoFilter = ko.observable();
    self.diplayOverdue = ko.observable(true)
    self.diplayThisWeek = ko.observable(true)
    self.diplayNextWeek = ko.observable(false)
    self.diplayAfterNextWeek = ko.observable(false)

    // overdue amounts
    this.overdueAmountEUR = ko.observable(0).money('€');
    this.overdueAmountUSD = ko.observable(0).money('$');
    this.overdueAmountGBP = ko.observable(0).money('$');

    // this week due amounts
    this.weekAmountEUR = ko.observable(0).money('€');
    this.weekAmountUSD = ko.observable(0).money('$');
    this.weekAmountGBP = ko.observable(0).money('£');

    // Next week due amounts
    this.nextWeekAmountEUR = ko.observable(0).money('€');
    this.nextWeekAmountUSD = ko.observable(0).money('$');
    this.nextWeekAmountGBP = ko.observable(0).money('£');

    // After Next week due amounts
    this.afterNextWeekAmountEUR = ko.observable(0).money('€');
    this.afterNextWeekAmountUSD = ko.observable(0).money('$');
    this.afterNextWeekAmountGBP = ko.observable(0).money('£');

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
        this.toBePaidEUR(computeAmount(data,'EUR'));
        this.toBePaidUSD(computeAmount(data,'USD'));
        this.toBePaidGBP(computeAmount(data,'GBP'));
    }

    function computeAmount(data, currency) {
        l_amount= 0;
        data.forEach(function (item) {
            if (item.Currency === currency) {
                l_amount += item.DueAmount;
            }
        });
        return l_amount;
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

        self.overdueAmountEUR(computeAmount(l_result, 'EUR'));
        self.overdueAmountUSD(computeAmount(l_result, 'USD'));
        self.overdueAmountGBP(computeAmount(l_result, 'GBP'));
        return l_result;
    });

    // This week due
    self.weekDueInvoices = ko.computed(function () {
        var _today = Date.now();
        var _endOfTheWeek = endOfWeek();

        l_result = [];

        if (!self.intercoFilter()) {
            self.invoices().forEach(function (_item) {
                if (Date.parse(_item.DueDate) > _today && Date.parse(_item.DueDate) <= _endOfTheWeek) {
                    l_result.push(_item);
                }
             });
        } else {
            self.invoices().forEach(function (_item) {
                if (_item.IsSupplierInterco != self.intercoFilter()) {
                    if (Date.parse(_item.DueDate) > _today && Date.parse(_item.DueDate) <= _endOfTheWeek) {
                        l_result.push(_item);
                    }
                }
            });
        }

        self.weekAmountEUR(computeAmount(l_result, 'EUR'));
        self.weekAmountUSD(computeAmount(l_result, 'USD'));
        self.weekAmountGBP(computeAmount(l_result, 'GBP'));
        return l_result;
    });

    // Due next week
    self.nextWeekDueInvoices = ko.computed(function () {
        var _endOfThisWeek = endOfWeek();
        var _endOfNextWeek = endOfNextWeek();

        l_result = [];

        if (!self.intercoFilter()) {
            self.invoices().forEach(function (_item) {
                if (Date.parse(_item.DueDate) > _endOfThisWeek && Date.parse(_item.DueDate) <= _endOfNextWeek) {
                    l_result.push(_item);
                }
            });
        } else {
            self.invoices().forEach(function (_item) {
                if (_item.IsSupplierInterco != self.intercoFilter()) {
                    if (Date.parse(_item.DueDate) > _endOfThisWeek && Date.parse(_item.DueDate) <= _endOfNextWeek) {
                        l_result.push(_item);
                    }
                }
            });
        }

        self.nextWeekAmountEUR(computeAmount(l_result, 'EUR'));
        self.nextWeekAmountUSD(computeAmount(l_result, 'USD'));
        self.nextWeekAmountGBP(computeAmount(l_result, 'GBP'));
        return l_result;
    });

    // Due next week
    self.afternextWeek = ko.computed(function () {
        var _endOfNextWeek = endOfNextWeek();

        l_result = [];

        if (!self.intercoFilter()) {
            self.invoices().forEach(function (_item) {
                if (Date.parse(_item.DueDate) > _endOfNextWeek) {
                    l_result.push(_item);
                }
            });
        } else {
            self.invoices().forEach(function (_item) {
                if (_item.IsSupplierInterco != self.intercoFilter()) {
                    if (Date.parse(_item.DueDate) > _endOfNextWeek) {
                        l_result.push(_item);
                    }
                }
            });
        }

        self.afterNextWeekAmountEUR(computeAmount(l_result, 'EUR'));
        self.afterNextWeekAmountUSD(computeAmount(l_result, 'USD'));
        self.afterNextWeekAmountGBP(computeAmount(l_result, 'GBP'));
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
        self.overdueInvoices().forEach(function (l_invoice) {
            l_invoice.ToBePaid = true;
            self.updateInvoice(l_invoice);
        });

    }


    this.toggleInterco = function() {
        self.intercoFilter(!self.intercoFilter());
    }

    this.togglediplayOverdue = function () {
        self.diplayOverdue(!self.diplayOverdue());
    }

    this.togglediplayWeek = function () {
        self.diplayThisWeek(!self.diplayThisWeek());
    }

    this.togglediplayNextWeek = function () {
        self.diplayNextWeek(!self.diplayNextWeek());
    }

    this.togglediplayAfterNextWeek = function () {
        self.diplayAfterNextWeek(!self.diplayAfterNextWeek());
    }


    self.remove = function (l_invoice) {
        // First remove from the server, then from the view-model.
        ajaxHelper(invoicesUri + l_invoice.InvoiceID, 'DELETE').done(function (data) {
            successNotice('Invoice edition', 'Invoice successfuly deleted', '', 'glyphicon glyphicon-trash');
            self.invoices.remove(l_invoice);
            //self.unselect();
        });
    }

    self.showInvoice = function (l_invoice) {
        self.currentInvoice(l_invoice);
        $('#myModal').modal('show');
    };



    // Fetch the initial data.
    self.intercoFilter(true);
    self.currencyFilter = ko.observable('All');
    getNonPaidInvoices();
    

};

ko.applyBindings(new ViewModel());


