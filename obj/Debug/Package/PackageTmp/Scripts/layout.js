var LayoutModel = function () {
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
        ajaxHelper(invoicesUri + '/nonpaids/0', 'GET').done(function (data) {
            self.invoices(data); 
        });
    }

    self.computeAmountsToBePaid = function () {

        amountEUR = 0;
        amountUSD = 0;
        amountGBP = 0;

        invoices.forEach(function (item) {
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

   
    self.formatNumber = function (_str) {
        return _str.toFixed(0);
    }

    getNonPaidInvoices();

};

ko.applyBindings(new LayoutModel());