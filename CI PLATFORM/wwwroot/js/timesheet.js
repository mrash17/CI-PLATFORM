getAllMissions();
getGoalMissions();
function getAllMissions() {
    var missions = document.getElementById("missiontime").value;
    $.ajax({
        url: "/TimeSheet/GetAllTimeBasedMissions",
        success: function (result) {
            console.log(result);
            var select = $('#missiontime');
            if (result.length == 0) {
                select.html('<option> No Mission Found </option>');
            }
            else {
                $.each(result, function (i, data) {
                    console.log(data);
                   
                        select.append('<option value="' + data.missionId + '" id="' + data.missionId + '">' + data.title + '</option>')
                    
                })
            }
            
        }
      
    })
}
function getGoalMissions() {
    $.ajax({
        url: "/TimeSheet/GetAllGoalBasedMissions",
        success: function (result) {
            console.log(result);
            var select = $('#goalmissions');
            if (result.length == 0) {
                select.html('<option> No Mission Found </option>');
            }
            else {
                $.each(result, function (i, data) {
                    console.log(data);

                    select.append('<option value="' + data.missionId + '" id="' + data.missionId + '">' + data.title + '</option>')

                })
            }

        }

    })
}
$('#TimeSheetMissions').submit(function (event) {
    event.preventDefault();

    var flag = 0;
    document.getElementById("TitleValid").classList.add("d-none");
   
    var timetitlechk = document.getElementById("missiontime").value;
    if (timetitlechk == "-1") {
        flag = 1;
        document.getElementById("TitleValid").classList.remove("d-none");
    }

    document.getElementById("Sheetmin").classList.add("d-none");

    var minchk = document.getElementById("minute").value;
    if (minchk == "") {
        flag = 1;
        document.getElementById("Sheetmin").classList.remove("d-none");
    }
    document.getElementById("Sheethour").classList.add("d-none");

    var hourchk = document.getElementById("hour").value;
    if (hourchk == "") {
        flag = 1;
        document.getElementById("Sheethour").classList.remove("d-none");
    }

    document.getElementById("TimeMessage").classList.add("d-none");
    var messagechk = document.getElementById("message").value;
    if (messagechk == "") {
        flag = 1;
        document.getElementById("TimeMessage").classList.remove("d-none");
    }

    document.getElementById("SheetDate").classList.add("d-none");
    document.getElementById("SheetDateLimit").classList.add("d-none");
    var dateChk = document.getElementById("timeDate").value;
    if (dateChk == "") {
        flag = 1;
        document.getElementById("SheetDate").classList.remove("d-none");
    }
    else if (dateChk > Date.now) {
        flag = 1;
        document.getElementById("SheetDateLimit").classList.remove("d-none");
    }
    var formData = $(this).serialize();
    console.log(formData);
    if (flag == 0) {
        $.ajax({
            url: '/TimeSheet/SaveTimeMission',
            type: 'POST',
            data: formData,
            success: function (response) {

                $('#TimeSheetMissions').html($(response).find('#TimeSheetMissions').html());
                $('#timebasedTable').html($(response).find('#timebasedTable').html());

                $('#btnclose').click();
                DataSaved();
                getAllMissions();
                

            },
            error: function () {
                alert('An error occurred while saving the data.');
            }
        });
    }
});



$('#goalForm').submit(function (event) {
    event.preventDefault();
    /*    document.getElementById("TitleValid").value ='-1';*/

    var flag = 0;
    document.getElementById("TitleValid").classList.add("d-none");

    var goaltitlechk = document.getElementById("goalmissions").value;
    if (goaltitlechk == "-1") {
        flag = 1;
        document.getElementById("GoalTitleValid").classList.remove("d-none");

    } var goaltitlechk = document.getElementById("goalmissions").value;
    if (goaltitlechk == "-1") {
        flag = 1;
        document.getElementById("GoalTitleValid").classList.remove("d-none");

    }
    var actiochk = document.getElementById("goaalAction").value;
    if (actiochk == "") {
        flag = 1;
        document.getElementById("actionvalid").classList.remove("d-none");

    }
    var msgchk = document.getElementById("goalMessage").value;
    if (msgchk == "") {
        flag = 1;
        document.getElementById("MessageValid").classList.remove("d-none");

    }

    var formData = $(this).serialize();
    console.log(formData);
    if (flag == 0) {
        $.ajax({
            url: '/TimeSheet/SaveTimeMission',
            type: 'POST',
            data: formData,
            success: function (response) {

         
                $('#goalbasedTable').html($(response).find('#goalbasedTable').html());
                $('#goalForm').html($(response).find('#goalForm').html());
                $('#goalbtnclose').click();
                DataSaved();
                getGoalMissions();


            },
            error: function () {
                alert('An error occurred while saving the data.');
            }
        });
    }
});


function DeleteDataTimeSheet(timesheetid) {
    debugger;
    $.ajax({
        url: '/TimeSheet/DeleteTimeSheetData',
        type: 'POST',
        data: {
            TimeSheetid: timesheetid,
        },
        success: function (response) {

            $('#timebasedTable').html($(response).find('#timebasedTable').html());
            $('#goalbasedTable').html($(response).find('#goalbasedTable').html());
            DataSaved();
        },
        error: function () {
            console.log(data);
            alert('An error occurred while deleting the data.');
        }
    })
}

function goalediting(timesheetid) {
    $.ajax({
        url: '/TimeSheet/EditTimeSheet',
        type: 'POST',
        data: {
            TimeSheetid: timesheetid,
        },
        success: function (response) {
            var select = $('#EditGoalmissions');
            console.log(response);
            select.empty();
            $('#editGoalData').find('input[name="action"]').val(response.action);
            $('#editGoalData').find('input[type="date"]').val(response.date.substring(0, 10));
            $('#editGoalData').find('textarea[name="message"]').val(response.message);
            select.append('<option value="' + response.missionid + '" id="' + response.missionid + '">' + response.missiontTitle + '</option>')


        },
        error: function () {
            alert('An error occurred while getting the data.');
        }
    })
}
function timeediting(timesheetid) {
    $.ajax({
        url: '/TimeSheet/EditTimeSheet',
        type: 'POST',
        data: {
            TimeSheetid: timesheetid,
        },
        success: function (response) {
            var select = $('#EditTimemissions');
            console.log(response);
            select.empty();
            $('#editTimeData').find('input[name="action"]').val(response.action);
            $('#editTimeData').find('input[name="hour"]').val(response.hour);
            $('#editTimeData').find('input[name="minute"]').val(response.minute);
            $('#editTimeData').find('input[type="date"]').val(response.date.substring(0, 10));
            $('#editTimeData').find('textarea[name="message"]').val(response.message);
            select.append('<option value="' + response.missionid + '" id="' + response.missionid + '">' + response.missiontTitle + '</option>')


        },
        error: function () {
            alert('An error occurred while getting the data.');
        }
    })
}

$('.EditgoalForm').submit(function (event) {
    event.preventDefault();
    var timesheetid = $(this).attr('id'); // Get the ID of the form
    var formData = $(this).serialize();
    $.ajax({
        url: '/TimeSheet/SaveEditedTimeMission',
        type: 'POST',
        data: formData,
        success: function (response) {
            console.log(formData)
            $('#timebasedTable').html($(response).find('#timebasedTable').html());
            $('#goalbasedTable').html($(response).find('#goalbasedTable').html());
            DataSaved();

        },
        error: function () {
            alert('An error occurred while saving the data.');
        }
    });
});