// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//function addCommas(nStr) {
//    nStr += '';
//    x = nStr.split('.');
//    x1 = x[0];
//    x2 = x.length > 1 ? '.' + x[1] : '';
//    var rgx = /(\d+)(\d{3})/;
//    while (rgx.test(x1)) {
//        x1 = x1.replace(rgx, '$1' + ',' + '$2');
//    }
//    return x1 + x2;
//}

function addCommas(yourNumber) {
    //Seperates the components of the number
    var n = yourNumber.toString().split(".");

    //Comma-fies the first part
    n[0] = n[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //Combines the two sections

    if (n.length == 1) {
        n.push('0000');
    }
    return n.join(".");
}

