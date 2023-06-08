/*For sliding in story detail
*/
$('.slider-for').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
    fade: true,
    asNavFor: '.slider-nav'
});
$('.slider-nav').slick({
    slidesToShow: 4,
    slidesToScroll: 1,
    asNavFor: '.slider-for',
    dots: true,
    focusOnSelect: true,
    autoplay: true
});

$('a[data-slide]').click(function (e) {
    e.preventDefault();
    var slideno = $(this).data('slide');
    $('.slider-nav').slick('slickGoTo', slideno - 1);
});

$(document).ready(function () {
    $('#search-input').keyup(function () {
        $('.pagination .active').removeClass('active');
        PageSearch();
    });

});
/*$(document).on('click', '.pagination li', function (e) {
    e.preventDefault();
    $('.pagination li').each(function () {
        $(this).removeClass('active');
    })
    $(this).addClass('active');
    console.log($(this).children().attr("id"))
    PageSearch();
});*/
/*function PageSearch() {
    var keyword = $('#search-input').val();
    var pageindex = $('.pagination .active a').attr('id');
    $.ajax({
        url: "StoryListing/storylisting",
        type: "POST",
        data: {
            SearchInputdata: keyword,
            pageIndex: pageindex
        },
        success: function (response) {

            $('#storycards').html($(response).find('#storycards').html());
           
            $('.page').html($(response).find('.page').html());

        }
    })
}
*/

