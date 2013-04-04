var viewModel = {
    addNewChart: function () {
        $("#charts").append($("#chartTemplate").tmpl({ Charts_index: viewModel._generateGuid(), Values_index: viewModel._generateGuid() }));
    },

    addNewValue: function ($valuesList, prefix) {
        $valuesList.append($("#valueTemplate").tmpl({ prefix: prefix, Values_index: viewModel._generateGuid() }));
    },

    _generateGuid: function () {
        // Source: http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript/105074#105074
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
};

$.datepicker.setDefaults({
    dateFormat: "M dd yy"
});

$("body").on("click", ".addNewValue", function () {
    var $chartItem = $(this).parentsUntil("ul", "li");
    var $valueList = $(".chartValues", $chartItem);
    var chartIndex = $("input[name='Charts.Index']", $chartItem).val();
    var prefix = "Charts[" + chartIndex + "].";
    viewModel.addNewValue($valueList, prefix);
    return false;
});
$("body").on("focus", "input.date", function () {
    if (!$(this).hasClass("hasDatepicker")) {
        $(this).datepicker();
        $(this).datepicker("show");
    }
});

$("body").on("change", ".objectTypeSelector", function () {
    var $propertyDropDowns = $(this).parent().siblings(".chartValues").children().children(".propertySelector");
    var $stratificationDropDown = $(this).siblings(".stratificationSelector");
    $propertyDropDowns.empty();
    $stratificationDropDown.empty();

    switch($(this).val()) {
        case "Request":
            $.each(requestPropertyOptions, function(i, option) {
                $propertyDropDowns.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            $.each(requestStratificationOptions, function(i, option) {
                $stratificationDropDown.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            break;
        case "Question":
            $.each(questionPropertyOptions, function(i, option) {
                $propertyDropDowns.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            $.each(questionStratificationOptions, function(i, option) {
                $stratificationDropDown.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            break;
    }
});

$("body").on("change", ".propertySelector", function() {
    var $objectTypeSelector = $(this).parents(".chartValues").siblings("span").children(".objectTypeSelector");
    var $statFunctionSelector = $(this).siblings(".statFunctionSelector");
    var statFunction = $statFunctionSelector.val();
    var $newOption;

    $statFunctionSelector.empty();

    switch($objectTypeSelector.val()) {
        case "Request":
            $.each(requestStatFunctionMap[$(this).val()], function(i, option) {
                $newOption = $("<option></option>").attr("value", option.Value).text(option.Text)
                if(option.Value == statFunction)
                    $newOption.attr("selected", "selected");
                $statFunctionSelector.append($newOption);
            });
            break;
        case "Question":
            $.each(questionStatFunctionMap[$(this).val()], function(i, option) {
                $newOption = $("<option></option>").attr("value", option.Value).text(option.Text)
                if(option.Value == statFunction)
                    $newOption.attr("selected", "selected");
                $statFunctionSelector.append($newOption);
            });
            break;
    }
});

$("body").on("change", ".statFunctionSelector", function() {
    var $propertySelector = $(this).siblings(".propertySelector");
    var $valNameSelector = $(this).siblings("input[type='text']");
    if($propertySelector.val() && $(this).val() && $valNameSelector.val() == "") {
        $valNameSelector.val($(this).val() + " " + $propertySelector.val());
    }
});