

function showList(e) {
    document.getElementById("btnlist").classList.add('active');
    document.getElementById("btngrid").classList.remove('active');
    var $gridCont = $('.grid-container');
    e.preventDefault();
    $gridCont.addClass('list-view');
}
function gridList(e) {
  
    document.getElementById("btngrid").classList.add('active');
    document.getElementById("btnlist").classList.remove('active');

  
    var $gridCont = $('.grid-container')
    e.preventDefault();
    $gridCont.removeClass('list-view');
}

$(document).on('click', '.btn-grid', gridList);
$(document).on('click', '.btn-list', showList);

/*
clearbutton.onclick = () => {
    const myNode = document.querySelector(".filters-section");
    while (myNode.lastElementChild) {
        myNode.removeChild(myNode.lastElementChild);
    }


}*/

/*//grid-list activation
var btnContainer = document.getElementById("gird-list");
// Get all buttons with class="btn" inside the container
var btns = btnContainer.getElementsByClassName("btn-grid-list");

// Loop through the buttons and add the active class to the current/clicked button
for (var i = 0; i < btns.length; i++) {
    btns[i].addEventListener("click", function () {
        var current = document.getElementsByClassName("active");
        current[0].className = current[0].className.replace(" active", "");
        this.className += " active";
    });
}


*/