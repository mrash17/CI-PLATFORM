var CMSpageNav = document.getElementById("CMSpageNav");
CMSpageNav.style.width = "80%";
CMSpageNav.style.backgroundColor = "white";
CMSpageNav.style.height = "45px";
CMSpageNav.style.alignItems = "center";
document.getElementById("CMSDiv").style.color = "#F88634";
$('#search').keyup(function () {
    getCMS();
})

function getCMS() {
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    $.ajax({
        url: "/Admin/CMSPage",
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
    getCMS();
});

function DeleteCMSByAdmin(cmsId) {
    $.ajax({
        url: '/Admin/RemoveCMSByAdmin',
        type: 'POST',
        data: {
            cmsId: cmsId,
        },
        success: function (response) {
            getCMS()
            Swal.fire('CMS Removed!', '', 'success')
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            Swal.fire('This CMS cannot be deleted as it is currently in use', '', 'error')

        }
    })
}

function DeleteDataSwal(cmsid) {
    Swal.fire({
        title: 'Are you Sure you want Delete?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteCMSByAdmin(cmsid);
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};

/*
tinymce.init({
    selector: '#cmsEdit',
    plugins: 'link image code',
    toolbar: 'undo redo | bold italic | fontsizeselect | alignleft aligncenter alignright alignjustify | superscript subscript ',
    height: 300
});
       */