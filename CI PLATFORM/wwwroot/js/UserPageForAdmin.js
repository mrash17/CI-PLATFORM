
$('#search').keyup(function () {
    getusers();
})
var userdiv = document.getElementById("userpagenavNav");
userdiv.style.width = "80%";
userdiv.style.backgroundColor = "white";
userdiv.style.height = "45px";
userdiv.style.alignItems = "center";
document.getElementById("userdivs").style.color = "#F88634";
function getusers() {
   /* $(".userpagenavNav,.missionpageNav,.cmspageNav,.themepageNav,.skillpageNav,.applicationNav,.storypageNav,.bannerNav").removeClass("active");
    $(".missionpageNav").addClass("active");*/
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    $.ajax({
        url: "/Admin/UserPage",
        type: "POST",
        data: {
            searchkeyword: searchkeyword,
            pageIndex: pageIndex
        },
        success: function (response) {

            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function (response) {
            alert(response+" error")
        }
    })
}


$(document).on('click', '.userpagination li', function (e) {
    e.preventDefault();
    $('.userpagination li').each(function () {
        $(this).removeClass('active');
    })
    $(this).addClass('active');
    console.log($(this).children().attr("id"))
    getusers();
});

function DeleteUserByAdmin(Userid) {
    debugger;
    $.ajax({
        url: '/Admin/RemoveUserByAdmin',
        type: 'POST',
        data: {
            userid: Userid,
        },
        success: function (response) {
            getusers()
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());
           /* $('#timebasedTable').html($(response).find('#timebasedTable').html());
            $('#goalbasedTable').html($(response).find('#goalbasedTable').html());*/
        },
        error: function () {
            alert('An error occurred while deleting the data.');
        }
    })
}

function DeleteDataSwal(userid) {
    Swal.fire({
        title: 'Are you Sure you want Delete?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DeleteUserByAdmin(userid);
            Swal.fire('User Removed!', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};



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
var countryList = document.getElementById("CountryList");
var selectedOption = countryList.options[countryList.selectedIndex];
var countryName = selectedOption.getAttribute("data-countryname");
var cId = countryList.value;


console.log("ok" + countryName);
getAllCountries(cId, countryName);
$('#CountryList').on('change', function () {
    $('#CityList').empty();
    getcitylist();

});

function getAllCountries(countryid, countryName) {
    if (countryid != "Select Country") {
        console.log("if con");
        $('#CountryList').empty();
       /* var countryname = $('#CountryList option').data('name');*/
        $('#CountryList').append('<option value="' + countryid + '" id="' + countryid + '">' + countryName + '</option>');
        var cityList = document.getElementById("CityList");
        var selectedOption = cityList.options[cityList.selectedIndex];
        var cityName = selectedOption.getAttribute("data-cityname");
        console.log("city is " + cityName);
        var cityId = cityList.value;

        $('#CityList').empty();
        $('#CityList').append('<option value="' + cityId + '" id="' + cityId + '">' + cityName + '</option>');
        getcitylist();
        
    }
  

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

//User img
document.getElementById("boot-icon").onclick = function () {
    document.getElementById("file-input").click();

}
document.getElementById("file-input").onchange = function (event) {
    const files = event.target.files;
    handleFiles(files);
};

const uploadedFiles = new Set();
function handleFiles(files) {
    $('#showImage').empty();
    const file = files[0];
    console.log(files);
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (!file.type.startsWith("image/")) continue;
        if (uploadedFiles.has(file.name)) {
            continue;
        }
        const reader = new FileReader();
        reader.onload = function (event) {
            $('#profileimg').attr('src', event.target.result);
        }
        reader.readAsDataURL(file);
        uploadedFiles.add(file.name);
    }
}

var img1 = $('#profileimg');
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
/*document.getElementById("file-input").onchange = function () {
    handleSelectedFile(this.files[0]);
}*/