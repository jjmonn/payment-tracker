
var invoicesUri = '/api/BankLoan';


self.remove = function (bankLoan) {
    // First remove from the server, then from the view-model.
    ajaxHelper(bankLoansUri + '/' + invoice.ID, 'UPDATE').done(function (data) {
        successNotice('Payment management', 'invoice now in payment');
        self.unselect();
    });
}



function getCurrentPaymentAmount() {
    var table = document.getElementById("Table1"), sumVal = 0;

    for (var i = 1; i < table.rows.length; i++) {
        if (table.rows[i].cells[0].innerHTML === true) {
            sumVal = sumVal + parseInt(table.rows[i].cells[4].innerHTML);
        }
    }

    document.getElementById("toBePaidAmount").innerHTML = "Current payment: " + sumVal + " €";
    console.log(sumVal);
}