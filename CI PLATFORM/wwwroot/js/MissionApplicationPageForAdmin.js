var applicationNav = document.getElementById("applicationNav");
applicationNav.style.width = "80%";
applicationNav.style.backgroundColor = "white";
applicationNav.style.height = "45px";
applicationNav.style.alignItems = "center";
document.getElementById("applicationDiv").style.color = "#F88634";
$('#search').keyup(function () {
    getmissionApplication();
})

function getmissionApplication() {
    var pageIndex = $('.userpagination .active a').attr('id');
    var searchkeyword = $('#search').val();
    $.ajax({
        url: "/Admin/MissionApplicationPage",
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
    getmissionApplication();
});

function ApproveMissionApplication(missionapplicationId) {

    $.ajax({
        url: '/Admin/ApprovalMissionApplicationByAdmin',
        type: 'POST',
        data: {
            MissionApplicationId: missionapplicationId,
            approvalstatus: "approved",
        },
        success: function (response) {
            getmissionApplication()
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            alert('An error occurred while updating the data.');
        }
    })
}function RejectMissionApplication(missionapplicationId) {

    $.ajax({
        url: '/Admin/ApprovalMissionApplicationByAdmin',
        type: 'POST',
        data: {
            MissionApplicationId: missionapplicationId,
            approvalstatus : "rejected",
        },
        success: function (response) {
            console.log(missionapplicationId);
            getmissionApplication()
            $('.userdetails').html($(response).find('.userdetails').html());
            $('.Userpaged').html($(response).find('.Userpaged').html());

        },
        error: function () {
            alert('An error occurred while updating the status.');
        }
    })
}

function ApproveMissionApplicationSwal(missionapplicationId) {
    Swal.fire({
        title: 'Are you Sure you want to Update?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
       
        if (result.isConfirmed) {
            ApproveMissionApplication(missionapplicationId);
            Swal.fire('Approval Status Updated as Approved', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};


function RejectMissionApplicationSwal(missionapplicationId) {
    Swal.fire({
        title: 'Are you Sure you want to Update?',
        showDenyButton: true,
        showCancelButton: true,
        confirmButtonText: 'Save',
        denyButtonText: `Don't save`,
    }).then((result) => {
       
        if (result.isConfirmed) {
            RejectMissionApplication(missionapplicationId);
            Swal.fire('Approval Status Updated as Rejected', '', 'success')
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    });
};