var ViewModel = function () {
    var self = this;
    self.suppliers = ko.observableArray();
    self.BankAccounts = ko.observableArray();
    self.error = ko.observable();

    //self.BankAccountsDict = new Object();
    //self.BankAccountsNames = [];
    self.dict = new Object();

    var suppliersUri = '/api/suppliers/';
    var invoicesUri = '/api/invoices/';
    var banksUri = '/api/BankAccounts/';

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

    function getPaymentsBySupplier() {
        ajaxHelper(suppliersUri + '/tobepaid/', 'GET').done(function (data) {

            // register default bank account because of issues with knockout select binding
            var i = 0;
            data.forEach(function (l_supplier) {
                l_supplier.Totals.forEach(function (total) {
                    self.dict[i] = total.BankAccount;
                    i++;
                });
            });

            self.suppliers(data);
            SetDefaultBanks();
        });
    }

    function SetDefaultBanks() {
        // Put back default bank for each total
        var i = 0;
        self.suppliers().forEach(function (supplier) {
            supplier.Totals.forEach(function (total) {
                total.BankAccount = self.dict[i];
                i++;
            });
        });
    }

    function getBankAccounts() {
        ajaxHelper(banksUri, 'GET').done(function (data) {
            self.BankAccounts(data);

            //data.forEach(function (l_bankAccount) {
            //    self.BankAccountsDict[l_bankAccount.Name] = l_bankAccount;
            //    self.BankAccountsNames.push(l_bankAccount.Name);   
            //});
        });
    }

  

    // update an invoice
    self.updateInvoice = function (invoice) {
        ajaxHelper(invoicesUri + '/' + invoice.InvoiceID, 'PUT', invoice).done(function (data) {
            getPaymentsBySupplier();
            // => if set to not to be paid => self.invoices.remove(invoice); ?
        });
    }

    // Set invoice to status "paid"
    self.updatePaidInvoice = function (invoice) {
        invoice.Paid = true;
        invoice.ToBePaid = false;
        var l_date = new Date(Date.now());
        invoice.PaymentDate = l_date; 
        ajaxHelper(invoicesUri + '/' + invoice.InvoiceID, 'PUT', invoice).done(function (data) {
            //getPaymentsBySupplier();
        });
    }

    // Set invoice status to paid for all invoices of this suppliers to be paid
    self.updatePaidSupplier = function (total) {

        total.Invoices.forEach(function (l_invoice) {
            self.updatePaidInvoice(l_invoice)
            // or put the loop on the server side if too slow
        });

        total.Paid = true;
        self.flush();
    }

    // Remove empty rows
    self.flush = function () {
        self.suppliers().forEach(function (supplier) {
            supplier.Totals.forEach(function (item, index, object) {
                if (item.Paid === true) {
                    object.splice(index, 1);
                }
            });
            if (supplier.Totals.length === 0) {
                self.suppliers.remove(supplier);
            }
        });
    }


    // Fetch the initial data.
    getBankAccounts();
    getPaymentsBySupplier();
};

ko.applyBindings(new ViewModel());