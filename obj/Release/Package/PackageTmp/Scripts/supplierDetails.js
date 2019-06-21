// to be re coded when suppliers and suplliers' detail page are migrated to API2 and ko.js


var table = document.getElementById("invoicesTable");
var sumValOverdue = 0;
var sumValThisWeek = 0;
var sumValNextWeek = 0;
var sumValAfter = 0;

var overdueInvoices = {'EUR':0, 'USD':0, 'GBP':0};
var weekdueInvoices = { 'EUR': 0, 'USD': 0, 'GBP': 0};
var nextWeekDueInvoices = { 'EUR': 0, 'USD': 0, 'GBP': 0};
var afterNextWeekdueInvoices = { 'EUR': 0, 'USD': 0, 'GBP': 0};


// Attention  index [] lié à l'affichage des colonnes ! hyper dangereux 
for (var i = 1; i < table.rows.length; i++) {

    l_strDate = table.rows[i].cells[3].innerText;
    l_strDate = l_strDate.substring(0,10);
    l_strDate = l_strDate.replace("\/", "-");
    l_strDate = l_strDate.replace("\/", "-");
    var l_date = Date.parse(l_strDate);

    var l_currency = table.rows[i].cells[1].innerText;
    var l_invoiceAmount = parseFloat(table.rows[i].cells[4].innerText.replace(",","."));
    
    var _today = Date.now();
    var _endOfThisWeek = endOfWeek();
    var _endOfNextWeek = endOfNextWeek();


    if (l_date <= _today) {
        var l_tmp = overdueInvoices[l_currency];
        l_tmp += l_invoiceAmount;
        overdueInvoices[l_currency] = l_tmp;
    }
    else {
        if (l_date <= _endOfThisWeek) {
            weekdueInvoices[l_currency] += l_invoiceAmount;
        }
        else {
            if (l_date <= _endOfNextWeek) {
                nextWeekDueInvoices[l_currency] += l_invoiceAmount;
            }
            else {
                afterNextWeekdueInvoices[l_currency] += l_invoiceAmount;
            }
        }
    }
}


document.getElementById("overdueEUR").innerHTML = overdueInvoices['EUR'].toFixed(2);
document.getElementById("overdueUSD").innerHTML = overdueInvoices['USD'].toFixed(2);
document.getElementById("overdueGBP").innerHTML = overdueInvoices['GBP'].toFixed(2);

document.getElementById("weekdueEUR").innerHTML = weekdueInvoices['EUR'].toFixed(2);
document.getElementById("weekdueUSD").innerHTML = weekdueInvoices['USD'].toFixed(2);
document.getElementById("weekdueGBP").innerHTML = weekdueInvoices['GBP'].toFixed(2);

document.getElementById("nextWeekdueEUR").innerHTML = nextWeekDueInvoices['EUR'].toFixed(2);
document.getElementById("nextWeekdueUSD").innerHTML = nextWeekDueInvoices['USD'].toFixed(2);
document.getElementById("nextWeekdueGBP").innerHTML = nextWeekDueInvoices['GBP'].toFixed(2);

document.getElementById("afterNextWeekdueEUR").innerHTML = afterNextWeekdueInvoices['EUR'].toFixed(2);
document.getElementById("afterNextWeekdueUSD").innerHTML = afterNextWeekdueInvoices['USD'].toFixed(2);
document.getElementById("afterNextWeekdueGBP").innerHTML = afterNextWeekdueInvoices['GBP'].toFixed(2);

var totalEUR = overdueInvoices['EUR'] + weekdueInvoices['EUR'] + nextWeekDueInvoices['EUR'] + afterNextWeekdueInvoices['EUR'];
document.getElementById("totalEUR").innerHTML = totalEUR.toFixed(2);

var totalUSD = overdueInvoices['USD'] + weekdueInvoices['USD'] + nextWeekDueInvoices['USD'] + afterNextWeekdueInvoices['USD'];
document.getElementById("totalUSD").innerHTML = totalUSD.toFixed(2);

var totalGBP = overdueInvoices['GBP'] + weekdueInvoices['GBP'] + nextWeekDueInvoices['GBP'] + afterNextWeekdueInvoices['GBP'];
document.getElementById("totalGBP").innerHTML = totalGBP.toFixed(2);