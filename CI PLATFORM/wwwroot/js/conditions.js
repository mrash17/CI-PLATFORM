
$(document).on('click', '.pagination li', function (e) {
    e.preventDefault();
    $('.pagination li').each(function () {
        $(this).removeClass('active');
    })
    $(this).addClass('active');
    console.log($(this).children().attr("id"))
    filterMission();
});



$(document).ready(function () {
    getCountry();
    getThemes();
    getSkills();
    $('city').attr('disabled', true);
});

$('#country').change(function () {
    var link = "/LandingPage/City";
    var list = [];
    $('.country:checkbox:checked').each(function () {
        list.push($(this).attr("id"));
    });

    $('#city').empty();
    $.ajax({
        url: link,
        data: {
            id: list
        },
        type: "POST",
        success: function (result) {
            $.each(result, function (i, data) {
                $('#city').append('<div class="form-check ms-3"><input class="form-check-input checkbox city" type="checkbox" value="' + data.name + '" id=' + data.cityId + '><label class="form-check-label" for=' + data.cityId + '>' + data.name + '</label></div>')
            })
        }
    })
    $('.pagination .active').removeClass('active');
    filterMission()
})

$('#search-input').keyup(function () {
    $('.pagination .active').removeClass('active');
    filterMission()
});

$('#city').change(function () {
    $('.pagination .active').removeClass('active');

    filterMission()
});
$('#theme').change(function () {
    $('.pagination .active').removeClass('active');

    filterMission()
});
$('#skill').change(function () {
    $('.pagination .active').removeClass('active');

    filterMission()
});
$('#sort').change(function () {
    $('.pagination .active').removeClass('active');

    filterMission()
});


$('#explore .dropdown-item').click(function () {
    $('#explore .dropdown-item').removeClass('active');
    $(this).addClass('active').siblings().removeClass('active');
    $('.pagination .active').removeClass('active');
    filterMission()
});
/*
function handleDropdownChange() {
    $('.pagination .active').removeClass('active');
    
    filterMission()
}*/
/*$('#explore').change(function () {
    $('.pagination .active').removeClass('active');
    
    filterMission()
});*/
/*
function getTopThemes() {
    $.ajax({
        url: "LandingPage/GetTopThemes",
        success: function (response){
            $('.cardsTResult').html($(response).find('.cardsTResult').html());
            $('#count').html($(response).find('#count').html());
            $('.page').html($(response).find('.page').html());
            $('.footer').html($(response).find('.footer').html());
        }
    })
}*/
function filterMission() {
    var country = [];
    $('.filter-section').empty()
    if ($('.filter-section').empty()) {
        $('.filter-section').append('<div class="filter-list text-danger ms-2" id="clear"><span class="ms-2">ClearAll</span>  <button class="filter-close-button" id="clearbutton">&times;</button>    </div>')
    }

    $('.country:checkbox:checked').each(function () {
        country.push($(this).attr("id"));
        $('.filter-section').prepend('<span class="filter-list ps-3 ms-2">' + $(this).val() + '&nbsp; <button class="filter-close-button cross" onclick="cross(' +"country_"+ + $(this).attr("id") + ');" id=' + "country_" + $(this).attr("id") + ' >  &times;</button> </span>')

    })


    var city = [];
    $('.city:checkbox:checked').each(function () {
        city.push($(this).attr("id"));
        $('.filter-section').prepend('<span class="filter-list ps-3 ms-2">' + $(this).val() + '&nbsp; <button class="filter-close-button cross" onclick="cross(city_' + $(this).attr("id") + ');" id=' + "city_" + $(this).attr("id") + ' >  &times;</button> </span>')

    })

    var theme = [];
    $('.theme:checkbox:checked').each(function () {
        theme.push($(this).attr("id"));
        $('.filter-section').prepend('<span class="filter-list ps-3 ms-2">' + $(this).val() + '&nbsp; <button class="filter-close-button cross" onclick="cross(theme_' + $(this).attr("id") + ');" id=' + "theme_" + $(this).attr("id") + ' >  &times;</button> </span>')

    })

    var skill = [];
    $('.skill:checkbox:checked').each(function () {
        skill.push($(this).attr("id"));
        $('.filter-section').prepend('<span class="filter-list ps-3 ms-2">' + $(this).val() + '&nbsp; <button class="filter-close-button cross" onclick="cross(skill_' + $(this).attr("id") + ');" id=' + "skill_" + $(this).attr("id") + ' >  &times;</button> </span>')
    })

    if ($('.filter-section .filter-list').length == 1) {
        $('.filter-section').empty();
    }
    var sortId = $('#sort').val();
    var pageIndex = $('.pagination .active a').attr('id');
    var keyword = $('#search-input').val();
    var exploreId =  $('#explore .dropdown-item.active').attr('value');
   /* if (keyword.replaceAll(' ', '') == '') {
        keyword = null;
    }
    if (keyword.length < 2) {
        keyword = null;
    }*/

    $.ajax({
        url: "/LandingPage/landingpage",
        type: "POST",
        data: {
            countryids: country,
            cityids: city,
            themeids: theme,
            skillids: skill,
            sortId: sortId,
            SearchInputdata: keyword,
            pageindex: pageIndex,
            exploreId: exploreId

        },
        success: function (response) {
            $('.cardsTResult').html($(response).find('.cardsTResult').html());
            $('#count').html($(response).find('#count').html());
            $('.page').html($(response).find('.page').html());
            $('.footer').html($(response).find('.footer').html());

        },
        Error: function () {
            alert('error');
        }
    })
}

function getCountry() {
    $.ajax({
        url: '/LandingPage/Country',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#country').append('<div class="form-check ms-3"><input class="form-check-input checkbox country" type="checkbox" value="' + data.name + '" id="' + data.countryId + '"><label class="form-check-label" for="' + data.countryId + '">' + data.name + '</label></div>')
            })
        }
    })
}

function getThemes() {
    $.ajax({
        url: '/LandingPage/Theme',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#theme').append('<div class="form-check ms-3"><input class="form-check-input checkbox theme" type="checkbox" value="' + data.title + '" id="' + data.themeId + '"><label class="form-check-label" for="' + data.themeId + '">' + data.title + '</label></div>')
            })
        }
    })
}

function getSkills() {
    $.ajax({
        url: '/LandingPage/Skill',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#skill').append('<div class="form-check ms-3"><input class="form-check-input checkbox skill" type="checkbox" value="' + data.skillName + '" id=' + data.skillId + '><label class="form-check-label" for=' + data.ski + '>' + data.skillName + '</label></div>')
            })
        }
    })
}


// Clear section


$(document).on('click', '#clear', function () {
    console.log("clear")
    $('.country:checkbox:checked').each(function () {
        $(this).prop('checked', false)
    })
    $('.city:checkbox:checked').each(function () {
        $(this).prop('checked', false)
    })
    $('.theme:checkbox:checked').each(function () {
        $(this).prop('checked', false)
    })
    $('.skill:checkbox:checked').each(function () {
        $(this).prop('checked', false)
    })
    $('#search-input').val('');
    $('.filters-section').empty();
    filterMission();
});

function cross(btnid) {
    var lst = $(btnid).attr("id").split("_");
    $('#' + lst[0] + ' #' + lst[1]).prop("checked", false);
    $('.pagination .active').removeClass('active');
    $('.pagination .active').find('#1').parent().addClass('active');
    filterMission();
}









function cancel(event) {
    event.stopPropagation();
    $('.noti').removeClass('d-none');
    $('.check').addClass('d-none');
}
function messagenable(event) {
    event.stopPropagation();
    $('.noti').addClass('d-none');
    $('.check').removeClass('d-none');
    $('.titleswithcheck').empty();
    $.ajax({
        type: "GET",
        url: "/User/GetTitles",

        success: function (result) {

            $.each(result.titles, function (i, data) {
               
                
                if ($.inArray(data.notificationId, result.ids) !== -1) {
                  
                    $('.titleswithcheck').append('<div class="form-check ms-3 fs-5"><label class="form-check-label" for=' + data.notificationId + ' >' + data.title + '</label><input class="form-check-input checkbox title" checked type="checkbox" value="' + data.notificationId + '" id=' + data.notificationId + '></div>')

                }
                else {
                    $('.titleswithcheck').append('<div class="form-check ms-3 fs-5"><label class="form-check-label" for=' + data.notificationId + '>' + data.title + '</label><input class="form-check-input checkbox title " type="checkbox" value="' + data.notificationId + '" id=' + data.notificationId + '></div>')

                }
            })

        }
    });

}


// Notification
function selecttitles() {
    titles = [];
    $('.title:checkbox:checked').each(function () {
        titles.push($(this).attr("id"));
    })
    console.log('titles', titles);
    $.ajax({
        type: "POST",
        url: "/User/SetStatus",
        data: {
            titles: titles
        },
        success: function (result) {
            getusernotification();
            toastr.success("Now for selected titles Notification will be shown!!");
        }
    });
}
getusernotification();

function getusernotification() {
    $('.usernoti').empty();
    $.ajax({
        type: "GET",
        url: "/User/GetNotification",
        success: function (result) {
            var dictionary = result;
            var count = Object.keys(dictionary).length;
            $('.circletext').text(count);
            $.each(result, function (i, data) {

                switch (data.item2) {
                    case 5:
                        console.log("success");
                        var img = '<img src="/images/add.png" class=" mx-2">';
                        if (data.item6 == 1) {
                            icon = '<i class="bi bi-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                        }
                        else {
                            icon = '<i class="bi bi-check-circle-fill col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                        }
                        datestring = '<span>' + data.item3 + '</span>'
                        $('.usernoti').append('<div class="form-check p-0 d-flex align-items-center border-bottom" onclick="notify(\'' + data.item4 + '\', ' + data.item5 + ')" >' + img + '<div class="d-flex flex-column ms-3">' + datestring + '<span class="form-check-label" for=' + data.item2 + '>' + data.item1 + '</span></div> ' + icon + '</div>');
                        break;
                    case 1:
                        console.log("success recomended");
                        var img = '<img src="' + data.item7 + '" class="sizeimg mx-2">';
                        if (data.item6 == 1) {
                            icon = '<i class="bi bi-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                        }
                        else {
                            icon = '<i class="bi bi-check-circle-fill col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                        }
                        datestring = '<span>' + data.item3 + '</span>'
                        $('.usernoti').append('<div class="form-check p-0 d-flex align-items-center border-bottom" onclick="notify(\'' + data.item4 + '\', ' + data.item5 + ')" >' + img + '<div class="d-flex flex-column ms-3">' + datestring + '<span class="form-check-label " for=' + data.item2 + '>' + data.item1 + '</span></div> ' + icon + '</div>');
                        break;
                    case 8:
                        console.log("success");
                        var img = '<img src="/CI Platform Assets/right.png" class=" mx-2 col-2">';
                        if (data.item6 == 1) {
                            icon = '<i class="bi bi-circle-fill text-warning col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                        }
                        else {
                            icon = '<i class="bi bi-circle-fill col-2 text-end me-2" id="message-' + data.item5 + '"></i>';
                        }
                        datestring = '<span>' + data.item3 + '</span>'
                        $('.usernoti').append('<div class="form-check p-0 d-flex align-items-center border-bottom">' + img + '<div class="d-flex flex-column ms-3">' + datestring + '<span class="form-check-label " for=' + data.item2 + '>' + data.item1 + '</span></div>' + icon + '</div>');
                        break;
                    default:
                        console.log("Value is not 1, 2, or 3.");
                }
            })
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("Error: " + errorThrown);
        }
    });
}


function notify(url, id) {
    var icon = $('#message-' + id);
    icon.removeClass('bi-circle-fill');
    icon.addClass('bi-check-circle-fill');

    $.ajax({
        type: "POST",
        url: "/User/ChangeStatus",
        data: {
            messageid: id
        },
        success: function (result) {
            if (url != null) {
                window.location.href = url;
            }
        }

    });
}