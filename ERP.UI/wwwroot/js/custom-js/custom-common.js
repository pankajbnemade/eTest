/************************************************************************************************************ 
DROPDOWN
*************************************************************************************************************/
// function for cascade dropdown list.
function CascadeDropDownList(actionPath, parameterList, childControl, errorMsgControl, errorMessage, appendDefaultValue, appendDefaultText, selectedValue) {
    AjaxCommunication.CreateRequest(
        this.window, "Post",
        actionPath,
        '',
        parameterList,
        function (data) {
            if (data.AuthorizationFail) {
                window.location.href = data.AuthorizationFailUrl;
            }

            $(childControl).empty();
            if ((appendDefaultValue != 'NA') && (appendDefaultText != 'NA')) {
                $(childControl).append("<option value='" + appendDefaultValue + "'>" + appendDefaultText + "</option>");
            }

            if (data.length > 0) {
                $.each(data, function (i, item) {
                    $(childControl).append("<option value='" + item.Value + "'>" + item.Text + "</option>");
                });
                if (selectedValue != 'NA') {
                    $(childControl).val(selectedValue);
                }
            }
            else {
                // if data not available fill only default value.
                $(childControl).empty();
                if ((appendDefaultValue != 'NA') && (appendDefaultText != 'NA')) {
                    $(childControl).append("<option value='" + appendDefaultValue + "'>" + appendDefaultText + "</option>");
                }
            }
        },
        function () {
            $(errorMsgControl).text(errorMessage);
            $(childControl).empty();
            if ((appendDefaultValue != 'NA') && (appendDefaultText != 'NA')) {
                $(childControl).append("<option value='" + appendDefaultValue + "'>" + appendDefaultText + "</option>");
            }
        },
        true, null, false);
}

/************************************************************************************************************ 
NOTIFICATION ALERTS.
*************************************************************************************************************/
// danger notification
function fnDangerNotify(alertMessage) {
    $.notify({
        // options
        message: alertMessage
    },
        {
            // settings
            type: 'danger',
        });
}

// success notification
function fnSuccessNotify(alertMessage) {
    $.notify({
        // options
        message: alertMessage
    },
        {
            // settings
            type: 'success',
            //placement: {
            //    from: "top",
            //    align: "center"
            //},
            //delay: 2000000,
            //timer: 6000,
        });
}

// warning notification
function fnWarningNotify(alertMessage) {
    $.notify({
        // options
        message: alertMessage
    },
        {
            // settings
            type: 'warning'
        });
}

// modal popup show.
$(document).on('show.bs.modal', '.modal', function (event) {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});