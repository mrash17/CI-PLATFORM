var bannerDiv = document.getElementById("bannerNav");
bannerDiv.style.width = "80%";
bannerDiv.style.backgroundColor = "white";
bannerDiv.style.height = "45px";
bannerDiv.style.alignItems = "center";
document.getElementById("bannerDiv").style.color = "#F88634";
/*var bannerNav2 = document.getElementById("bannerNav2");

bannerNav2.style.backgroundColor = "white";
bannerNav2.style.height = "45px";
bannerNav2.style.alignItems = "center";
document.getElementById("bannerDiv2").style.color = "#F88634";*/

$('#search').keyup(function () {
    getBannner();
})
function getBannner() {
    
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    
    $.ajax({
        url: "/Admin/BannerPage",
        type: "POST",
        data: {
            searchkeyword: searchkeyword,
            pageIndex: pageIndex
        },
        success: function (response) {
            console.log(response);

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
    getBannner();
});

function DeleteBannerByAdmin(BannerId) {
    $.ajax({
        url: '/Admin/RemoveBannerByAdmin',
        type: 'POST',
        data: {
            bannerId: BannerId,
        },
        success: function (response) {
            getBannner()
            Swal.fire('Banner Removed!', '', 'success')
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            Swal.fire('This Banner cannot be deleted as it is currently in used', '', 'error')

        }
    })
}

function DeleteDataSwal(BannerId) {
    Swal.fire({
        title: 'Are you Sure you want Delete?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteBannerByAdmin(BannerId);
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};


$('#bannerForm').submit(function (event) {
    event.preventDefault();
    if ($('#showImage').children().length == 0) {
        $('#bannerimgValidate').text("Select Image");
    }

})