/*
* Form Validation
* This script will set Bootstrap error classes when form.submit is called.
* The errors are produced by the MVC unobtrusive validation.
*/
/*
$(function () {
    $('form').submit(function () {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length == 0) {
                $(this).removeClass('error');
            }
        });

        if (!$(this).valid()) {
            $(this).find('div.control-group').each(function () {
                if ($(this).find('span.field-validation-error').length > 0) {
                    $(this).addClass('error');
                }
            });
        }
    });
    $('form').each(function () {
        $(this).find('div.control-group').each(function () {
            if ($(this).find('span.field-validation-error').length > 0) {
                $(this).addClass('error');
            }
        });
    });
});
var page = function () {
    //Update that validator
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest(".control-group").addClass("error");
        },
        unhighlight: function (element) {
            $(element).closest(".control-group").removeClass("error");
        }
    });
}();
/* End Form Validation */

$(function () {
    var validator = $('form').validate();
    var aspErrorPlacement = validator.settings.errorPlacement;
    validator.settings.errorPlacement = function (error, element) {
        aspErrorPlacement(error, element);
        if (error.length > 0 && error[0].innerHTML === "") {
            error.closest('div.control-group').removeClass('error');
        }
    };
    $('span.field-validation-valid, span.field-validation-error').each(function () {
        $(this).addClass('help-inline');
    });

    $('form').submit(function () {
        if ($(this).valid()) {
            $(this).find('div.control-group').each(function () {
                if ($(this).find('span.field-validation-error').length == 0) {
                    $(this).removeClass('error');
                }
            });
        } else {
            $(this).find('div.control-group').each(function () {
                var control = $(this).find('span.field-validation-error')
                if (control.length > 0) {
                    $(this).addClass('error');
                }
            });
        }
    });
});