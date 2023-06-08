var skillNav = document.getElementById("skillNav");
skillNav.style.width = "80%";
skillNav.style.backgroundColor = "white";
skillNav.style.height = "45px";
skillNav.style.alignItems = "center";
document.getElementById("skillDiv").style.color = "#F88634";
$('#search').keyup(function () {
    getSkills();
})

function getSkills() {
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    $.ajax({
        url: "/Admin/SkillPage",
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
    getSkills();
});

function DeleteSkillByAdmin(skillid) {
    debugger;
    $.ajax({
        url: '/Admin/RemoveSkillByAdmin',
        type: 'POST',
        data: {
            skillId: skillid,
        },
        success: function (response) {
            getSkills()
            Swal.fire('Skill Removed!', '', 'success')
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            Swal.fire('This Skill cannot be deleted as it is currently in use', '', 'error')

        }
    })
}

function DeleteDataSwal(skillid) {
    Swal.fire({
        title: 'Are you Sure you want Delete?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteSkillByAdmin(skillid);
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};

