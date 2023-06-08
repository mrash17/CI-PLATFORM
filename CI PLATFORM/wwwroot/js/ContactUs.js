
$('#Contactus').submit(function (event) {
    event.preventDefault();
    var contactflag = 1;
    debugger;
    document.getElementById("Conatctsubject").classList.add("d-none");
    document.getElementById("ConatctMessage").classList.add("d-none");

    var formData = $(this).serializeArray();
    if (formData[0].value == "") {
        document.getElementById("Conatctsubject").classList.remove("d-none");
        contactflag = 0;
    }if (formData[1].value == "") {
        document.getElementById("ConatctMessage").classList.remove("d-none");
        contactflag = 0;
    }
   
    if (contactflag == 1) {
        $.ajax({
            url: '/User/ContactusSubmit',
            type: 'POST',
            data: formData,
            success: function (response) {
                
                MessageSent();
                $('#contactClose').click();

            },
            error: function (response) {
                alert('An error occurred while saving the data.');
            }
        });
    }
});