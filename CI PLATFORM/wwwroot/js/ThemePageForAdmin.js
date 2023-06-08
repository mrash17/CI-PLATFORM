var ThemeNav = document.getElementById("ThemeNav");
ThemeNav.style.width = "80%";
ThemeNav.style.backgroundColor = "white";
ThemeNav.style.height = "45px";
ThemeNav.style.alignItems = "center";
document.getElementById("ThemeDiv").style.color = "#F88634";

$('#search').keyup(function () {
    getThemes();
})

function getThemes() {
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    debugger;
    $.ajax({
        url: "/Admin/ThemePage",
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
    getThemes();
});

function DeleteThemeByAdmin(themeid) {
    debugger;
    $.ajax({
        url: '/Admin/RemoveThemeByAdmin',
        type: 'POST',
        data: {
            ThemeId: themeid,
        },
        success: function (response) {
            getThemes()
            Swal.fire('Theme Removed!', '', 'success')
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            Swal.fire('This Theme cannot be deleted as it is being used by a mission', '', 'error')

        }
    })
}

function DeleteDataSwal(themeid) {
    Swal.fire({
        title: 'Are you Sure you want Delete?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteThemeByAdmin(themeid);
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};

