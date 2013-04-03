var viewModel = {
    addNewQuestion: function () {
        $("#noQuestions").hide();
        var count = $("input[name=QuestionCount]:hidden");
        var idx = count.val();
        var $obj = $("#questionTemplate").tmpl({ Questions_index: viewModel._generateGuid(), Keywords_index: viewModel._generateGuid(), References_index: viewModel._generateGuid(), QIndex: idx });
        viewModel.addImpactUpdateHandler($obj);
        $("#questionList").append($obj);
        var next = +idx + 1;
        count.val(next);
    },

    addNewKeyword: function ($keywordsList, prefix) {
        var $obj = $("#keywordTemplate").tmpl({ prefix: prefix, Keywords_index: viewModel._generateGuid() });
        viewModel.applyKeywordAutocomplete($obj.find(".keywordAutocomplete"));
        viewModel.addDeleteHandler($obj.find(".deleteKeyword"));
        $keywordsList.append($obj);
    },

    addNewReference: function ($referenceList, prefix) {
        var $obj = $("#referenceTemplate").tmpl({ prefix: prefix, References_index: viewModel._generateGuid() });
        viewModel.addDeleteHandler($obj.find(".deleteReference"));
        $referenceList.append($obj);
    },

    _generateGuid: function () {
        // Source: http://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid-in-javascript/105074#105074
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    },

    applyKeywordAutocomplete: function (node) {
        $(node).autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/Autocomplete/Keyword", type: "POST", dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: "{ \"prefix\": \"" + request.term + "\" }",
                    async: true,
                    success: function (data) {
                        response(data);
                    },
                    error: function (result) {
                        response(null);
                    }
                });
            },
            delay: 500,
            minLength: 2,
            select: function (event, ui) {
                $(node).find("input[name$=Keyword]").val(ui.item.value);
            },
        });
    },

    addDeleteHandler: function (node) {
        $(node).click(function () {
            var par = $(this).closest("li");
            var deleted = par.find("input[name$=Delete]:hidden");
            console.log(par);
            console.log(deleted);
            deleted.val('True');
            par.hide(0.5);
        });
    },

    computeImpact: function (severity, probability) {
        var probableLookup = { "Major": 1, "Moderate": 2, "Minor": 3 };
        var possibleLookup = { "Major": 4, "Moderate": 4, "Minor": 5 };
        var unlikelyLookup = { "Major": 5, "Moderate": 5, "Minor": 5 };
        var problookup = {};
        problookup["Probable"] = probableLookup;
        problookup["Possible"] = possibleLookup;
        problookup["Unlikely"] = unlikelyLookup;
        var l = problookup[probability];
        if (l) {
            return l[severity];
        }
        else
            return 0;
    },

    addImpactUpdateHandler: function (questionItem) {
        var handler = function (obj) {
            var $prob = questionItem.find("select[name$=Probability]");
            var $severity = questionItem.find("select[name$=Severity]");
            var prob = $prob.val();
            var severity = $severity.val();
            var $impact = questionItem.find("input[name$=Impact]");
            var impact = viewModel.computeImpact(severity, prob);
            console.log(prob);
            $impact.val(impact);
        }
        var $prob = questionItem.find("select[name$=Probability]");
        var $severity = questionItem.find("select[name$=Severity]");
        $prob.change(handler);
        $prob.trigger('change');
        $severity.change(handler);
    },

    renewRequestLock: function () {
        var id = $("input[name=RequestID]:hidden").val();
        $.ajax({
            url: "/Requests/RenewRequestLock", type: "POST", dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: "{ \"requestId\": \"" + id + "\" }",
            success: function (msg) {
                //console.log(id + " " + msg);
            },
            error: function (error) {
                //console.log(error);
            }
        });
    }
};

$("body").on("click", ".addNewKeyword", function () {
    var $qi = $(this).closest(".questionItem");
    var questionIdx = $qi.find("input[name$='Questions.Index']:hidden").val();
    var prefix = "Questions[" + questionIdx + "].";
    var $keywordList = $qi.find(".keywordList");
    viewModel.addNewKeyword($keywordList, prefix);
    return false;
});

$("body").on("click", ".addNewReference", function () {
    var $qi = $(this).closest(".questionItem");
    var questionIdx = $qi.find("input[name$='Questions.Index']:hidden").val();
    var prefix = "Questions[" + questionIdx + "].";
    var $referenceList = $qi.find(".referenceList");
    viewModel.addNewKeyword($referenceList, prefix);
    return false;
});

$(function () {
    $("#PatientAgencyID").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Autocomplete/Patient", type: "POST", dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ \"agencyId\": \"" + request.term + "\" }",
                async: true,
                success: function (data) {
                    response($.map(data, function (item) {
                        return item;
                    }))
                },
                error: function (result) {
                    response(null);
                }
            });
        },
        delay: 500,
        minLength: 2,
        select: function (event, ui) {
            $("input[name=PatientAgencyID]").val(ui.item.AgencyID);
            $("input[name=PatientFirstName]").val(ui.item.FirstName);
            $("input[name=PatientLastName]").val(ui.item.LastName);
            $("input[name=PatientGender]").val(ui.item.Gender);
            $("input[name=PatientAge]").val(ui.item.Age);
        },
    });

    $keywordInputs = $(".keywordAutocomplete");
    $.each($keywordInputs, function(i, val) {
        viewModel.applyKeywordAutocomplete(val);
    });

    $keywordDeleters = $(".deleteKeyword");
    $.each($keywordDeleters, function (i, val) {
        viewModel.addDeleteHandler(val);
    });

    $referenceDeleters = $(".deleteReference");
    $.each($referenceDeleters, function (i, val) {
        viewModel.addDeleteHandler(val);
    });

    $questionItems = $(".questionItem");
    $.each($questionItems, function (i, question) {
        var $question = $(question);
        viewModel.addImpactUpdateHandler($question);
        $question.find("input[name$=Probability]").change();
    });
});