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



document.getElementById("downloadBtn").addEventListener("click", function () {
    var selectedFormat = document.querySelector('input[name="flexRadioDefault"]:checked').value;
    var zipRadio = document.querySelector('input[name="zipRadio"]:checked').value;
    if (selectedFormat === "pdf") {
        /* 
          To view the pdf
                   window.location.href = '../documents/pdf-Lorem_ipsum.pdf';  */
        /* pdf but  in form
         * var downloadLink = document.createElement("a");
          downloadLink.href = '../documents/pdf-Lorem_ipsum.pdf';
          downloadLink.download = 'storiesData.pdf';
          downloadLink.click();*/
        /*   html2canvas(document.getElementById("storyTable")).then(function (canvas) {
               debugger;
               var imgData = canvas.toDataURL("pdf");
               var pdf = new jsPDF();
               pdf.addImage(imgData, "PDF", 10, 10);
               pdf.save("table.pdf");
           });*/
        /*       
         *       pdf format in img form
         *       html2pdf().from(document.getElementById('storyTable')).save('StoryTable.pdf');
        */
        var doc = new jsPDF();
        var table = document.getElementById('storyTable');

        doc.autoTable({ html: table });
        if (zipRadio == "true") {
            var zip = new JSZip();
            zip.file("");
            // blob is data used to store doc img and others
            zip.file('storyTable.pdf', doc.output('blob'));

            zip.generateAsync({ type: 'blob' }).then(function (content) {
                saveAs(content, 'storyTable.zip');

            });
        }
        else {
            doc.save('storyTable.pdf');
        }


    }
    else if (selectedFormat === "csv") {
       
        downloadCSV(zipRadio);
/*        $("#storyTable").tableToCSV(); //include tableToCSV cdn
*/
    }
});
//Download CSV function
function downloadCSV(zipRadio) {
    var table = document.getElementById('storyTable');
    var rows = table.getElementsByTagName('tr');
    var csvContent = '';

    for (var i = 0; i < rows.length; i++) {
        var rowData = [];
        var cells = rows[i].querySelectorAll('th, td');

        for (var j = 0; j < cells.length; j++) {
            var cellData = cells[j].innerText.replace(',', ''); 
            rowData.push(cellData);
        }

        var row = rowData.join(',');
        csvContent += row + '\r\n';
    }

    var encodedUri = encodeURI(csvContent);
    if (zipRadio == "true") {
        var zip = new JSZip();
        zip.file("");
        // here csv is in text so no need to use blob to store it
        zip.file('StoryData.csv', csvContent);

        zip.generateAsync({ type: 'blob' }).then(function (content) {
            saveAs(content, 'storyTablecsv.zip');

        });
    }
    else {
        var link = document.createElement('a');
        link.setAttribute('href', 'data:text/csv;charset=utf-8,' + encodedUri);
        link.setAttribute('download', 'StoryData.csv');
        document.body.appendChild(link);
        link.click();//Download using a tag
        document.body.removeChild(link);
    }
 
}
