$(document).ready(function () {
    $('#search-input').keyup(function () {
        $('.pagination .active').removeClass('active');
        PageSearch();
    });

});

$('.filterCheckbox').addClass('d-none');
$('#filterbtn').addClass('d-none');

$(document).on('click', '.pagination li', function (e) {
    e.preventDefault();
    $('.pagination li').each(function () {
        $(this).removeClass('active');
    })
    $(this).addClass('active');
    console.log($(this).children().attr("id"))
    PageSearch();
});
function PageSearch() {
    var keyword = $('#search-input').val();
    
    var pageindex = $('.pagination .active a').attr('id');
    $.ajax({
        url: "/StoryListing/storylisting",
        type: "POST",
        data: {
            SearchInputdata: keyword,
            pageIndex: pageindex
        },
        success: function (response) {
            console.log(response);
            $('#storycards').html($(response).find('#storycards').html());

            $('.footer').html($(response).find('.footer').html());

        },
       
    })
};