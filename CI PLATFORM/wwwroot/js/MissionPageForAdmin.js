var missionpagenav = document.getElementById("missionpagenav");
missionpagenav.style.width = "80%";
missionpagenav.style.backgroundColor = "white";
missionpagenav.style.height = "45px";
missionpagenav.style.alignItems = "center";
document.getElementById("missiondiv").style.color = "#F88634";

$('#search').keyup(function () {
    getmissions();
})

function getmissions() {
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    $.ajax({
        url: "/Admin/MissionPage",
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
            alert(response + " error")
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
    getmissions();
});

function DeleteUserByAdmin(missionid) {
    $.ajax({
        url: '/Admin/RemoveMissionByAdmin',
        type: 'POST',
        data: {
            Missionid: missionid,
        },
        success: function (response) {
            getmissions()
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());
        
        },
        error: function () {
            alert('An error occurred while deleting the data.');
        }
    })
}

function DeleteDataSwal(missionid) {
    Swal.fire({
        title: 'Are you Sure you want Delete?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DeleteUserByAdmin(missionid);
            Swal.fire('Mission Removed!', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};




// ADD and  Edit Missions




$('#countryselect').on('change', function () {
    var countryId = $("#countryselect").val();
    
    $.ajax({
        url: "/User/GetAllCity",
        type: "GET",
        data: {
            id: countryId,
            
        },
        success: function (result) {
            $('#cityselect').empty();
            if (result.length === 0) {
                $('#cityselect').html('<option value="">No City Found</option>');
            } else {
                $('#cityselect').prepend('<option value="" selected>Select City</option>');
                $.each(result, function (i, data) {
                    / if (missionid != data.missionId) {/
                    $('#cityselect').append('<option value="' + data.cityId + '" id="' + data.cityId + '">' + data.name + '</option>');
                    /*}*/

                })
            }
        }
    })
});

$('.selectskills:checkbox:checked').each(function () {
    var skillname = $(this).next('label').text();
    var id = $(this).val();
    $('.skills-selected').append('<span class="filter-list ps-3 pe-3 me-2 rounded-pill border btn disabled btn-dark text-white">' + skillname + '</span>')
})
if ($('.skills-selected .filter-list').length > 0) {
    $('.skills-selected').prepend('<span class="filter-list ps-3 pe-3 me-2 p-2 text-dark fw-bold">Selected Skills</span>');
}
function Addskill() {
    $('.skills-selected').empty();
    $('.selectskills:checkbox:checked').each(function () {
        var skillname = $(this).next('label').text();
        var id = $(this).val();
        $('.skills-selected').append('<span class="filter-list ps-3 pe-3 me-2 rounded-pill border btn disabled btn-dark text-white">' + skillname + '</span>')
    })
    if ($('.skills-selected .filter-list').length > 0) {
        $('.skills-selected').prepend('<span class="filter-list ps-3 pe-3 me-2 p-2 text-dark fw-bold">Selected Skills</span>');
    }
}



function getThemes() {

    $.ajax({
        url: "/Admin/GetThemes",
        type: "GET",

        success: function (result) {

            if (result.length === 0) {
                $('#themeselect').html('<option>No countries selected</option>');
            } else {
                $.each(result, function (i, data) {
                    $('#themeselect').append('<option value="' + data.themeId + '" id="' + data.themeId + '">' + data.title + '</option>');


                })
            }
        }
    })
}

if ($("#missionhide").val() == 0) {
    getcountry();
    getThemes();
}

function getcountry() {

    $.ajax({
        url: "/User/GetAllCountries",
        type: "GET",

        success: function (result) {

            if (result.length === 0) {
                $('#mission').html('<option>No countries selected</option>');
            } else {
                $.each(result, function (i, data) {

                    $('#countryselect').append('<option value="' + data.countryId + '" id="' + data.countryId + '">' + data.name + '</option>');

                })
            }
        }
    })
}
if ($("#missionhide").val() != 0) {
    Mission();
}
Mission();

function Mission() {

    var type = $('#avail12').val();
    if (type == "true") {
        $('#timefields').removeClass('d-none');
        $('#goalfields').addClass('d-none');
    }
    else {
        $('#goalfields').removeClass('d-none');
        $('#timefields').addClass('d-none');
    }
};


$('#Images').change(function () {
    const files = $('#Images').prop('files');
    handleFiles(files);
})
const uploadedFiles = new Set();
function handleFiles(files) {
    $('#showImage').empty();
    console.log(files);
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (!file.type.startsWith("image/")) continue;
        if (uploadedFiles.has(file.name)) {
            alert(`File "${file.name}" has already been uploaded.`);
            continue;
        }
        uploadedFiles.add(file.name);
        const image = document.createElement("img");
        image.classList.add("image-preview");
     /*   $(".image-preview").style.height = "100px";
        $(".image-preview").style.width = "100px";*/
        image.style.height = "120px";
        image.style.width = "120px"
        image.style.margin="5px"
        const imageContainer = document.createElement("div");
        imageContainer.classList.add("image-container");
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            image.src = reader.result;
            imageContainer.appendChild(image);
            $('#showImage').append(imageContainer);
        };
    }
}
$('#missionForm').submit(function (event) {  
    var textarea = tinymce.get("storytext").getContent();
    event.preventDefault();
    var flag = 1;
    if ($('#showImage').children().length == 0) {
        $('#imgValidate').text("Select Images");
        flag = 0;
    }    if ($('#showImage').children().length < 2) {
        $('#imgValidate').text("Select atleast 2 Images");
        flag = 0;
    }
    if ($('#showImage').children().length > 6) {
        $('#imgValidate').text("More than 6 images cannot be uploaded");
        flag = 0;
    }
   if ($('#sdate').val() > $('#edate').val()) {
        $('#edatevalid').text("End date must be greater than Start date");
       flag = 0;
    }
    if ($('#avail12').val() == false) {
        if ($('#edate').val() < $('#rdead').val() || $('#rdead').val() < $('#sdate').val()) {
            $('#rdeadvalid').text("Deadline must be between End date and Start date");
            flag = 0;
        }
    }
    

     if ($('#showDocument').children().length == 0) {
       $('#docValidate').text("Select a document");
       flag = 0;
    }
     if (textarea == "") {
       $('#textValidate').text("Enter Mission Description");
       flag = 0;
    }

    if ($('#missionForm').valid() && flag ==1) {
        $('#missionForm')[0].submit();
    }
})



// Get the input element
const input = document.querySelector('#document');

// Add event listener for when a file is selected
input.addEventListener('change', () => {
    // Clear the contents of the showDocument div
    document.querySelector('#showDocument').innerHTML = '';

    // Loop through each selected file
    for (let i = 0; i < input.files.length; i++) {
        const file = input.files[i];

        // Check if the file is an image
        if (file.type.startsWith('image/')) {
            // Create an image element for the file
            const img = document.createElement('img');
            img.src = URL.createObjectURL(file);

            // Add some styles to the image
            img.style.maxWidth = '120px';
            img.style.maxHeight = '120px';
            img.style.margin = '5px';

            // Add the image element to the showDocument div
            document.querySelector('#showDocument').appendChild(img);
        }
        else {
            // Create a div element for the file name
            const div = document.createElement('div');
            div.innerText = file.name;

            // Add some styles to the div
            /* div.style.width = '100px';
            div.style.margin = '5px';*/

            // Add the div element to the showDocument div
            document.querySelector('#showDocument').appendChild(div);

        }
    }
});
tinymce.init({
    selector: '#storytext',
    plugins: 'link image code',
    toolbar: 'undo redo | bold italic | fontsizeselect | alignleft aligncenter alignright alignjustify | superscript subscript ',
    height: 300
});

function handleKeyDown(event) {
    const allowedKeys = ['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight'];
    if (allowedKeys.includes(event.code)) {
        return true;
    }
    if (!isNaN(Number(event.key)) && event.code !== 'Space') {
        return true;
    }
    return false;
}