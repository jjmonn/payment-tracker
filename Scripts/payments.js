var ViewModel = function () {
    var self = this;
    self.suppliers = ko.observableArray();
    self.error = ko.observable();

    var suppliersUri = '/api/suppliers/';
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

    function getPaymentsBySupplier() {
        ajaxHelper(suppliersUri + '/tobepaid/', 'GET').done(function (data) {
            self.suppliers(data);
        });
    }

    // update a supplier
    // to be checked should not be usefull here   ----- js to be put in payment.js .....
    //self.updateSupplier = function (supplier) {
    //    ajaxHelper(suppliersUri + '/' + supplier.ID, 'PUT', supplier).done(function (data) {
    //        // code here
    //    });
    //}

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
    getPaymentsBySupplier();
};

ko.applyBindings(new ViewModel());