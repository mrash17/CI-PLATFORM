GetUserData();

function GetUserData() {
    $.ajax({
        url: '/User/GetUserData',
        type: 'POST',
        success: function (response) {
            console.log(response);
            populateForm("#UserDetails", response);
          
            var select = $('#CountryList');
            $('.js-example-basic-single').select2();

            select.empty();
            select.append('<option value="' + response.countryid + '" id="' + response.countryid + '">' + response.countryName + '</option>');
            
            getAllCountries(response.countryid);
          
        },
        error: function () {
            alert('An error occurred while getting the data.');
        }
    });
}

/*function populateForm(form, data) {
    $.each(data, function (key, value) {
        var $ctrl = $(form).find('[name=' + key + ']');
        if ($ctrl.length > 0) {

            switch ($ctrl.prop("type")) {

                case "text":
                case "hidden":
                case "textarea":
                    $ctrl.val(value);
                    break;
                case "select-one": //For select tag
                    $ctrl.val(value);
                    break;
                case "radio":
                    $ctrl.each(function () {
                    if ($(this).val() == value) {
                        $(this).prop("checked", true);
                    }
                });
                    break;
                case "checkbox":
                    debugger;
                    $ctrl.each(function () {
                        
                        if (value!= null) {
                            $(this).attr("checked", true);
                        }
                      *//*  if ($(this).attr('value') == value) {
                            $(this).attr("checked", true);
                        }*//*
                    });
                    break;

            }
        }
    });
}*/
function populateForm(form, data) {
    $.each(data, function (key, value) {
        var $ctrl = $(form).find('[name="' + key + '"]');
        if ($ctrl.length > 0) {
            if ($ctrl.is("input[type='text'], input[type='hidden'], textarea, select")) {
                $ctrl.val(value);
            } else if ($ctrl.is(":checkbox")) {
                $ctrl.prop("checked", value);
            } else if ($ctrl.is(":radio")) {
                $ctrl.filter('[value="' + value + '"]').prop("checked", true);
            } else if ($ctrl.is("label")) {
                $ctrl.text(value);
            }
        }
    });
}



function getAllCountries(countryid) {

    var missions = document.getElementById("CountryList").value;

    console.log("countryid is : : :  ", countryid);
    $.ajax({
        url: "/User/GetAllCountries",
        data: {
            cid: countryid,
        },
        success: function (result) {



            var select = $('#CountryList');

            $.each(result, function (i, data) {

                select.append('<option value="' + data.countryId + '" id="' + data.countryId + '">' + data.name + '</option>')

            })


        }

    });
}

$(document).ready(function () {
    $('.js-example-basic-single').select2();
});