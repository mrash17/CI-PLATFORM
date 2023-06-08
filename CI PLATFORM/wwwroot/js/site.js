

function Sendmail() {
    Swal.fire({
        icon: 'success',
        showCloseButton: true,
        title: 'Invitation Sent Successfully',
        showConfirmButton: false,
        timer: 1500
    });
};
function usernotselected() {
    // trigger the alert message using Swal.fire
    Swal.fire({
        icon: 'error',
        showCloseButton: true,
        title: 'Please Select a user',
        showConfirmButton: false,
        timer: 1500

    });
};
function missionappliedalert() {
    Swal.fire({
        icon: 'success',
        showCloseButton: true,
        title: 'Mission Applied Successfully',
        showConfirmButton: true,
        timer: 1500
    });
};
function DataSaved() {
    Swal.fire({
        icon: 'success',
        showCloseButton: true,
        title: 'Data Saved Successfully',
        showConfirmButton: true,
        timer: 2500
    });
};
function CommentDone() {
    Swal.fire({
        icon: 'success',
        showCloseButton: true,
        title: 'Comment Posted Successfully',
        showConfirmButton: true,
        timer: 2500
    });
};
function BlankAlert() {
    Swal.fire({
        icon: 'error',
        showCloseButton: true,
        title: 'Comment cannot be blank',
        showConfirmButton: true,
        timer: 2500
    });
};
function MessageSent() {
    Swal.fire({
        icon: 'success',
        showCloseButton: true,
        title: 'Message Sent Successfully',
        showConfirmButton: true,
        timer: 2500
    });
};



/*For preventing e, + , - in number field*/
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