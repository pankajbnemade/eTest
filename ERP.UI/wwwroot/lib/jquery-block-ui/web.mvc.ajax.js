// Method used for all server communication via jquery
function AjaxCommunication() {
    this.DefaultTimeOut = 1200000;
    var message = '<span id="blockUIMessage">Please wait...</span>'
    this.WaitMessage = message;
    this.WaitMessageZindex = 1005;

    /* blockUI - optional parameter sets whether the window must be block till the ajax request is done; default is set always to true; 
    Use the parameter when you want to switch off the blocking. */
    this.CreateRequest = function (TokenWindow, Protocol, Url, DataType, GetData, SuccessCallBack, ErrorCallback, blockUI, timeOut, IsCache, blockAreaId) {
        if (typeof (blockUI) == "undefined") {
            blockUI = true;
        }

        if (ErrorCallback == null)
            ErrorCallback = AjaxCommunication.DefaultErrorCallBack;

        var data;
        if (DataType == "html") {
            // form.serialize()
            data = GetData;
        }
        else {
            data = typeof (GetData) === "object" ? GetData : new Object();
            if (data == null) {
                data = new Object();
            }

            if (DataType === 'json') {
                data = JSON.stringify(data);
            }
            else if (typeof (GetData) === "object") {
                data = $.param(data);
            }
        }

        if ('undefined' !== typeof (IsCache) && null !== IsCache) {
            IsCache = false;
        }

        if (blockUI) {
            $(document).ajaxStart(function () {
                $.blockUI({
                    message: AjaxCommunication.WaitMessage,
                    css: {
                        top: "50%",
                        left: "38%",
                        width: "22%",
                        textAlign: "center",
                        color: "#000",
                        "font-size": "30px",
                        border: "0px",
                        backgroundColor: "transparent",
                        padding: "8px",
                        margin: 0,
                        opacity: 1,
                        cursor: "wait",
                        "z-index": AjaxCommunication.WaitMessageZindex
                    },
                    overlayCSS: {
                        opacity: "0.25",
                        "z-index": parseInt(AjaxCommunication.WaitMessageZindex) - 1
                    }
                });
            }).ajaxStop(function () {
                $.unblockUI();
                // Reset the wait message in case any consumer has updated it with the custom message
                this.WaitMessage = message;
            });
        }

        var ajaxTimeOut = typeof (timeOut) == "undefined" ? AjaxCommunication.DefaultTimeOut : timeOut;
        if (DataType === 'json') {
            $.ajax({
                cache: IsCache,
                type: Protocol,
                url: Url,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: data,
                timeout: ajaxTimeOut,
                success: SuccessCallBack,
                error: ErrorCallback,
                beforeSend: function () {
                    if (blockAreaId != "undefined") {
                        $(blockAreaId).show();
                    }
                },
                complete: function () {
                    if (blockAreaId != "undefined") {
                        $(blockAreaId).hide();
                    }
                },
            });
        }
        else {
            $.ajax({
                cache: IsCache,
                type: Protocol,
                url: Url,
                data: data,
                timeout: ajaxTimeOut,
                success: SuccessCallBack,
                error: ErrorCallback,
                beforeSend: function () {
                    if (blockAreaId != "undefined") {
                        $(blockAreaId).show();
                    }
                },
                complete: function () {
                    if (blockAreaId != "undefined") {
                        $(blockAreaId).hide();
                    }
                },
            });
        }
    }

    this.DefaultErrorCallBack = function (request, errorThrown) {
        var obj = null;
        if (errorThrown == "timeout") { // ajax timed out
            window.location.replace("/Error/TimeoutException");
            return;
        }

        if (request.responseText.length > 0) {
            try {
                obj = JSON.parse(request.responseText);
            }
            catch (e) {
                // [TP]: This extra alert message is required because when html has been passed to the error callback, JSON.parse can not parse the content to an object and
                // throws an exception - if the exception is not catch then the error handler method will no finish properly and the end user will not be redirected 
                // to the error page as is excepted.
                // Now the information from the response text will be shown in alert and when user pressed "OK" the site will redirect him automatically to the error page
                // alert(request.responseText);
            }
        }
        if (request.status == 408) { // server time-out
            window.location.replace("/Error/TimeoutException");
            return;
        }
        else if (request.status == 503) { // service unavailable
            window.location.replace("/Error/ServiceUnavailable");
            return;
        }
        else if (request.status == 501) { // security violation exception
            if (obj == null) {
                window.location.replace("/Error/AccessDenied");
            }
            else {
                window.location.replace("/Error/AjaxException?p=" + obj.queryString);
            }
            return;
        }
        else if (request.status == 404) { // not found
            window.location.replace("/Error/NotFound");
            return;
        }
        else { // internal server error
            if (obj == null) {
                window.location.replace("/Error/AjaxException");
                return;
            }
            else {
                window.location.replace("/Error/AjaxException?p=" + obj.message);
            }
            return;
        }
    }
};

AjaxCommunication = new AjaxCommunication();

/* Function for creating of namespace for methods and global variables in JQuery */
jQuery.namespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = a[i].split(".");
        o = window;
        for (j = 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};

// block ui progress loader.
function fnBlockUI() {
    $.blockUI({
        message: '<span id="blockUIMessage">Please wait...</span>',
        css: {
            top: "50%",
            left: "38%",
            width: "22%",
            textAlign: "center",
            color: "#000",
            "font-size": "30px",
            border: "0px",
            backgroundColor: "transparent",
            padding: "8px",
            margin: 0,
            opacity: 1,
            cursor: "wait",
            "z-index": 1005
        },
        overlayCSS: {
            opacity: "0.25",
            "z-index": 1004
        }
    });
}

// unblock ui progress loader.
function fnUnblockUI() {
    $.unblockUI();
}