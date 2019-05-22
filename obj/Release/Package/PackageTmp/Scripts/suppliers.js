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
    self.updateSupplier = function (supplier) {
        ajaxHelper(suppliersUri + '/' + supplier.ID, 'PUT', supplier).done(function (data) {
            // code here
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
        invoice.ToBePaid = false;
        var l_date = new Date(Date.now());
        invoice.PaymentDate = l_date; 
        ajaxHelper(invoicesUri + '/' + invoice.InvoiceID, 'PUT', invoice).done(function (data) {
            getPaymentsBySupplier();

        });
    }

    // Fetch the initial data.
    getPaymentsBySupplier();
};

ko.applyBindings(new ViewModel());