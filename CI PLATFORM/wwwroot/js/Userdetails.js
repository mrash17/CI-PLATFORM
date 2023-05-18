
$(document).ready(function () {

    $('#UserDetails').submit(function (event) {
        event.preventDefault();
        var skills = [];
        $('.skill-selected div').each(function () {
            var id = $(this).attr('id');
            skills.push(id);
        })
        var formData = $(this).serializeArray();
        $.each(skills, function (index, value) {
            formData.push({ name: "userselectedSkills[" + index + "]", value: value });
        });
        var flag = 0;
        console.log(formData);
        var firstName = document.getElementById("firstname");
        document.getElementById("firstnameValid").classList.add("d-none");
        var SecondName = document.getElementById("secondname");
        document.getElementById("secondnameValid").classList.add("d-none");
        if (firstName.value == "") {
            flag = 1;
            document.getElementById("firstnameValid").classList.remove("d-none");
        }
        if (SecondName.value == "") {
            flag = 1;
            document.getElementById("secondnameValid").classList.remove("d-none");
        }

        var availability = document.getElementById("AvailabilityId");
        document.getElementById("AvailabilityValid").classList.add("d-none");
        if (availability.value == "null") {
            flag = 1;
            document.getElementById("AvailabilityValid").classList.remove("d-none");
        }

        var linkedin = document.getElementById("LinkedinID");
        document.getElementById("LinkedinValid").classList.add("d-none");
        const regex = /^(https?:\/\/)?(in\.)?linkedin\.com\/[a-zA-Z0-9\-]+(\/)?$/;

        if (!regex.test(linkedin.value)) {
            flag = 1;
            document.getElementById("LinkedinValid").classList.remove("d-none");
        }

        if (flag == 0) {
            $.ajax({
                url: '/User/SaveUserDetails',
                type: 'POST',
                data: formData,
                success: function (response) {

                    $('#UserDetails').html($(response).find('#UserDetails').html());
                    $('#userbasics').html($(response).find('#userbasics').html());
                    GetUserData();
                    DataSaved();

                },
                error: function () {
                    alert('An error occurred while saving the data.');
                }
            });
        }

    });




    function handleSelectedFile(file) {


        var formData = new FormData();
        formData.append("Image", file);
        $.ajax({
            type: 'POST',
            url: '/User/AddImage',
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                console.log(result);
                GetUserData();
                editImg();

                DataSaved();

            }
        });
        // Read the file as a data URL
    }
    editImg();
    function editImg()
        {


        var img1 = $('#profileimg');
        debugger;
        var img = document.getElementById("profileimg");
        img.src = document.getElementById("profileimg").getAttribute('src');
        $('.img-wrap').mouseover(function () {
            $('#boot-icon').removeClass("d-none")
        })
        $('.img-wrap').mouseout(function () {
            $('#boot-icon').addClass("d-none")
        })

        document.getElementById("profileimg").onclick = function () {
            document.getElementById("file-input").click();
        }
        document.getElementById("boot-icon").onclick = function () {
            document.getElementById("file-input").click();
        }
        document.getElementById("file-input").onchange = function () {
            handleSelectedFile(this.files[0]);
        }

    }





    function GetUserData() {
        $.ajax({
            url: '/User/GetUserData',
            type: 'POST',

            success: function (response) {
                console.log(response);
                document.getElementById("profileimg").setAttribute("src", response.avatar)
                document.getElementById('FirstNameforUser').innerHTML = response.firstName +" "+ response.secondName;
                console.log($('#FirstNameforUser'));
                /*$('#FirstNameforUser').val(response.secondName);*/
                $('input[name="FirstName"]').val(response.firstName);
                $('input[name="SecondName"]').val(response.secondName);
                $('input[name="EmployeeId"]').val(response.employeeId);
                $('input[name="Manager"]').val(response.manager);
                $('input[name="Title"]').val(response.title);
                $('input[name="Department"]').val(response.department);
                $('input[name="Availability"]').val(response.availability);
                $('textarea[name="ProfileText"]').val(response.profileText);
                $('textarea[name="WhyIVolunteer"]').val(response.whyIVolunteer);
                $('input[name="LinkedInUrl"]').val(response.linkedInUrl);
                $('input[name="CityName"]').val(response.cityName);
               
                var selectAva = $('#AvailabilityId');
                if (response.availability != "null") {
                    selectAva.empty();
                    selectAva.append('<option value="' + response.availability + '" id="' + response.availability + '">' + response.availability + '</option>');

                }
                
                if (response.availability == "Weekly") {
                    selectAva.append('<option value="Daily" id="Daily">Daily</option>');
                    selectAva.append('<option value="Monthly" id="Monthly">Monthly</option>');
                }
                else if(response.availability == "Daily")
                {
                    selectAva.append('<option value="Weekly" id="Weekly">Weekly</option>');
                    selectAva.append('<option value="Monthly" id="Monthly">Monthly</option>');
                }
                else if (response.availability == "Monthly")
                {
                    selectAva.append('<option value="Daily" id="Daily">Daily</option>');
                    selectAva.append('<option value="Weekly" id="Weekly">Weekly</option>');
                }
                var select = $('#CountryList');
                select.empty();
                select.append('<option value="' + response.countryid + '" id="' + response.countryid + '">' + response.countryName + '</option>');
                getAllCountries(response.countryid);
                console.log("Select is " + select.val())
                var Cityselect = $('#CityList');
                Cityselect.empty();
                $('#CityList').append('<option value="' + response.cityid + '" id="' + response.cityId + '">' + response.cityName + '</option>');
                getcitylist();
            },
            error: function () {
                alert('An error occurred while getting the data.');
            }
        })
    }
  
    
    GetUserData();

    function getcitylist() {
        var countryid = $('#CountryList').val();
        console.log("countryid is ", countryid);
        var cityId = $('#CityList').val();

        $.ajax({
            url: '/User/GetAllCity',
            data: {
                id: countryid,
                cityid: cityId,
            },
            traditional: true,
            success: function (result) {
                if (result.length === 0) {
                    $('#CityList').html('<option>No City Found</option>');
                }
                else {
                    $.each(result, function (i, data) {
                        $('#CityList').append('<option value="' + data.cityId + '" id="' + data.cityId + '">' + data.name + '</option>');
                    })
                }
            }
        })

    }
    $('#CountryList').on('change', function () {
        $('#CityList').empty();

        getcitylist();

    });
    
    function getAllCountries(countryid) {
        
        var missions = document.getElementById("CountryList").value;
        debugger;
        
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
});

$('.available-skills div').click(function () {
   
    $(this).hasClass("bg-secondary") ? $(this).removeClass("bg-secondary") : $(this).addClass("bg-secondary");
})

function selectSkill() {
    if ($('.available-skills div').hasClass("bg-secondary")) {
        $('.selected-skills').empty();

    }
    $('.available-skills div').each(function () {
        if ($(this).hasClass("bg-secondary")) {
          
            $('.selected-skills').append($(this).clone());
        }
    })
}


$(document).on('click', '.selected-skills div', function () {
    $(this).hasClass("bg-secondary") ? $(this).removeClass("bg-secondary") : $(this).addClass("bg-secondary");
});



function deselectSkill() {
    $('.selected-skills div').each(function () {
        if ($(this).hasClass('bg-secondary')) {
            $(this).remove();
        }
    })
}

function addskill() {
    console.log("Skill Recv")
    $('.skill-selected').empty();
    $('.selected-skills div').each(function () {
        $(this).removeClass('bg-secondary');
        $('.skill-selected').append($(this).clone());

    })
}



/*
$('#changepassword').submit(function (event) {
    var formData = $('#changepassword').serialize();
    console.log(formData)
    $.ajax({
        url: '/User/Changepassword',
        data: formData,
        success: function (response) {
            console.log('done');
        }
    })
})*/

