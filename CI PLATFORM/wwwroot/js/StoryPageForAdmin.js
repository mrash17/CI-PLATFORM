var storyNav = document.getElementById("storyNav");
storyNav.style.width = "80%";
storyNav.style.backgroundColor = "white";
storyNav.style.height = "45px";
storyNav.style.alignItems = "center";
document.getElementById("storyDiv").style.color = "#F88634";

$('#search').keyup(function () {
    getStories();
})

function getStories() {
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    $.ajax({
        url: "/Admin/StoryPage",
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
    getStories();
});

function ApproveStoryStatus(storyId) {
    $.ajax({
        url: '/Admin/ApprovalStoryByAdmin',
        type: 'POST',
        data: {
            StoryId: storyId,
            approvalstatus: 1,
        },
        success: function (response) {
            getStories()
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            alert('An error occurred while updating the data.');
        }
    })
}
function RejectStoryStatus(storyId) {
    $.ajax({
        url: '/Admin/ApprovalStoryByAdmin',
        type: 'POST',
        data: {
            StoryId: storyId,
            approvalstatus: 3,
           
        },
        success: function (response) {
            getStories()
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            alert('An error occurred while updating the status.');
        }
    })
}

function ApproveStorySwal(storyId) {
    Swal.fire({
        title: 'Are you Sure you want to Update?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
       
        if (result.isConfirmed) {
            ApproveStoryStatus(storyId);
            Swal.fire('Approval Status Updated as Approved', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
}; function RejectStorySwal(storyId) {
    Swal.fire({
        title: 'Are you Sure you want to Update?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
       
        if (result.isConfirmed) {
            RejectStoryStatus(storyId);
            Swal.fire('Approval Ststus Updated as Rejected', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};


function DeleteDataSwal(storyId) {
    Swal.fire({
        title: 'Are you Sure you want Delete?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {
            DeleteStoryByAdmin(storyId);
            Swal.fire('Story Removed!', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};
function DeleteStoryByAdmin(storyId) {
    $.ajax({
        url: '/Admin/RemoveStoryByAdmin',
        type: 'POST',
        data: {
            StoryId: storyId,
        },
        success: function (response) {
            getStories()
          
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            alert('An error occurred while deleting the data.');
        }
    })
}
