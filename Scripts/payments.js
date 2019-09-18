var BankAccount = function (bank) {
    var self = this;

    self.ID = bank.ID;
    self.Name = bank.Name;
    self.Currency = bank.Currency;
    self.DefaultBank = bank.DefaultBank;
    self.Balance = ko.observable(bank.Balance);//.money('');
    self.MaxOverdraft = ko.observable(bank.MaxOverdraft);//.money('');
    //self.Balance = ko.observable(bank.Balance).extend({numeric : 0});
    //self.MaxOverdraft = ko.observable(bank.MaxOverdraft).extend({ numeric: 0 });
    //self.AvailableCash = ko.observable();
    self.Payments = ko.observable(0).money('');
    self.CashAfterPayment = ko.observable();//.extend({ numeric: 0 });

    // Init computed 
    //self.AvailableCash = getAvailableCash();
    //self.Payments = getPayments();
    self.CashAfterPayment = getCashAfterPayment();

    function getAvailableCash() {
        return ko.pureComputed(function () {
            return (self.Balance() - self.MaxOverdraft());
        }, self);
    }

    function getCashAfterPayment() {
        return ko.pureComputed(function () {
            return (self.Balance() - self.MaxOverdraft() - self.Payments());
        }, self);
    }

    self.SetPayment = function (amount) {
        self.Payments(amount);
    }

    self.SetIncrementPayment = function (amount) {
        var current = self.Payments();
        self.Payments(current + amount);
    }
}


var ViewModel = function () {
    var self = this;
    self.suppliers = ko.observableArray();
    self.BankAccounts = ko.observableArray();
    self.error = ko.observable();

    self.dict = new Object();

    // Computed
    //self.AvailableCash = ko.observable();
    //self.Payments = ko.observable();
   // self.CashAfterPayment = ko.observable();
    

    var suppliersUri = '/api/suppliers/';
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
            // still needed ?!
            var i = 0;
            data.forEach(function (l_supplier) {
                l_supplier.Totals.forEach(function (total) {
                    self.dict[i] = total.BankAccount;
                    i++;
                });
            });

            self.suppliers(data);
            SetDefaultBanks();
            self.ComputeBankPayments();
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

    self.ComputeBankPayments = function() {

        // init all banks payments
        self.BankAccounts().forEach(function (bank) {
            bank.SetPayment(0);
        });

        // go through totals and increment banks' payments
        self.suppliers().forEach(function (l_supplier) {
            l_supplier.Totals.forEach(function(total) {
                var bank = getBankByID(total.BankAccount.ID);
                bank.SetIncrementPayment(total.Amount);
                // Additional currency check ?
            });
        });
    }

    function getBankByID(id) {
        var result = null;
        self.BankAccounts().forEach(function (bank) {
            if (bank.ID === id) {
                result = bank;
            }
        });
        return (result);
    }

    function getBankAccounts() {
        ajaxHelper(banksUri, 'GET').done(function (data) {
        
            data.forEach(function (bank) {
                self.BankAccounts.push(new BankAccount(bank));
            });

        });
    }

    // update an invoice
    //self.updateInvoice = function (invoice) {
    //    ajaxHelper(invoicesUri + '/' + invoice.InvoiceID, 'PUT', invoice).done(function (data) {
    //        getPaymentsBySupplier();
    //        // => if set to not to be paid => self.invoices.remove(invoice); ?
    //    });
    //}


    // Set invoice status to paid for all invoices of this supplier's total to be paid
    self.MarkTotalAsPaid = function (total) {

        // Server supplier's to be paid invoices marked as paid
        ajaxHelper(suppliersUri + 'markpayment/' + total.SupplierID + '/' + total.BankAccount.ID + '/' + total.Currency, 'PUT').done(function (response) {
            if (response == "success") {
                total.Paid = true;
                self.flush();  // remove empty rows (in case several currencies to be paid)
                successNotice('Invoices payment', 'Supplier successfully paid', 3, 'glyphicon glyphicon-trash'); 
            }
            else {
                errorNotice('Invoices payment', 'Server error during payment registration: please try again.', 200, 'glyphicon glyphicon-trash');
            }
        });
    }


    self.DownloadTotalWire = function (total) {
        // Does total has all info ? references, supplier, bank ?
        // to which controller should it go ?
        // Do we need a wire model ?
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



//doc ready function
$(document).ready(function () {

    ko.applyBindings(new ViewModel());

});