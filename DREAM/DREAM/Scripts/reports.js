var viewModel = {
    addNewChart: function () {
        $("#charts").append($("#chartTemplate").tmpl({ Charts_index: viewModel._generateGuid(), Values_index: viewModel._generateGuid() }));
    },

    addNewValue: function ($valuesList, prefix) {
        var $newValueItem = $("#valueTemplate").tmpl({ prefix: prefix, Values_index: viewModel._generateGuid() });
        $valuesList.append($newValueItem);
        return $newValueItem;
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

    var $newValueItem = viewModel.addNewValue($valueList, prefix);
    var $propertyDropDown = $(".propertySelector", $newValueItem);
    var objectType = $(".objectTypeSelector", $newValueItem.parentsUntil("#charts", "li")).val();

    setPropertyDropDownOptions($propertyDropDown, objectType);

    return false;
});

$("body").on("focus", "input.date", function () {
    if (!$(this).hasClass("hasDatepicker")) {
        $(this).datepicker();
        $(this).datepicker("show");
    }
});

$("body").on("change", ".objectTypeSelector", function () {
    var $chartItem = $(this).parentsUntil("ul", "li");
    var $propertyDropDowns = $(".chartValues .propertySelector", $chartItem);
    var $stratificationDropDown = $(".stratificationSelector", $chartItem);
    var objectType = $(this).val();

    setPropertyDropDownOptions($propertyDropDowns, objectType);
    setStratificationDropDown($stratificationDropDown, objectType);
});

function setPropertyDropDownOptions($propertyDropDowns, objectType) {
    var $listItem = $propertyDropDowns.closest("li");
    var $statFunctionDropDown = $(".statFunctionSelector", $listItem);

    $propertyDropDowns.empty().attr("disabled", null);
    $statFunctionDropDown.empty().attr("disabled", "");

    switch(objectType) {
        case "Request":
            $.each(requestPropertyOptions, function (i, option) {
                $propertyDropDowns.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            break;
        case "Question":
            $.each(questionPropertyOptions, function(i, option) {
                $propertyDropDowns.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            break;
    }
}

function setStratificationDropDown($stratificationDropDown, objectType) {
    $stratificationDropDown.empty().attr("disabled", null);
    switch(objectType) {
        case "Request":
            $.each(requestStratificationOptions, function(i, option) {
                $stratificationDropDown.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            break;
        case "Question":
            $.each(questionStratificationOptions, function(i, option) {
                $stratificationDropDown.append($("<option></option>").attr("value", option.Value).text(option.Text));
            });
            break;
    }
}

$("body").on("change", ".propertySelector", function () {
    var $chartItem = $(this).parentsUntil("#charts", "li");
    var $objectTypeSelector = $(".objectTypeSelector", $chartItem);
    var $valueItem = $(this).closest("li");
    var $statFunctionSelector = $(".statFunctionSelector", $valueItem);
    var statFunction = $statFunctionSelector.val();
    var $newOption;

    $statFunctionSelector.empty().attr("disabled", null);

    switch($objectTypeSelector.val()) {
        case "Request":
            $.each(requestStatFunctionMap[$(this).val()], function (i, option) {
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
    var $valueItem = $(this).closest("li");
    var $propertySelector = $(".propertySelector", $valueItem);
    var $valNameSelector = $("input[type='text']", $valueItem);
    if($propertySelector.val() && $(this).val() && $valNameSelector.val() == "") {
        $valNameSelector.val($(this).val() + " " + $propertySelector.val());
    }
});