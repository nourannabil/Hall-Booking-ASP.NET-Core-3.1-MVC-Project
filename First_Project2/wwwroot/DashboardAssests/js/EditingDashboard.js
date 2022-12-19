$(document).ready(function () {

    $sidebar = $('.sidebar');
    $sidebar_img_container = $sidebar.find('.sidebar-background');

    $full_page = $('.full-page');

    $sidebar_responsive = $('body > .navbar-collapse');
    sidebar_mini_active = false;

    window_width = $(window).width();

    fixed_plugin_open = $('.sidebar .sidebar-wrapper .nav li.active a p').html();

    // if( window_width > 767 && fixed_plugin_open == 'Dashboard' ){
    //     if($('.fixed-plugin .dropdown').hasClass('show-dropdown')){
    //         $('.fixed-plugin .dropdown').addClass('show');
    //     }
    //
    // }

    $('.fixed-plugin a').click(function (event) {
        // Alex if we click on switch, stop propagation of the event, so the dropdown will not be hide, otherwise we set the  section active
        if ($(this).hasClass('switch-trigger')) {
            if (event.stopPropagation) {
                event.stopPropagation();
            } else if (window.event) {
                window.event.cancelBubble = true;
            }
        }
    });

    $('.fixed-plugin .active-color span').click(function () {
        $full_page_background = $('.full-page-background');

        $(this).siblings().removeClass('active');
        $(this).addClass('active');

        var new_color = $(this).data('color');

        if ($sidebar.length != 0) {
            $sidebar.attr('data-active-color', new_color);
        }

        if ($full_page.length != 0) {
            $full_page.attr('data-active-color', new_color);
        }

        if ($sidebar_responsive.length != 0) {
            $sidebar_responsive.attr('data-active-color', new_color);
        }
    });

    $('.fixed-plugin .background-color span').click(function () {
        $(this).siblings().removeClass('active');
        $(this).addClass('active');

        var new_color = $(this).data('color');

        if ($sidebar.length != 0) {
            $sidebar.attr('data-color', new_color);
        }

        if ($full_page.length != 0) {
            $full_page.attr('filter-color', new_color);
        }

        if ($sidebar_responsive.length != 0) {
            $sidebar_responsive.attr('data-color', new_color);
        }
    });

    $('.fixed-plugin .img-holder').click(function () {
        $full_page_background = $('.full-page-background');

        $(this).parent('li').siblings().removeClass('active');
        $(this).parent('li').addClass('active');


        var new_image = $(this).find("img").attr('src');

        if ($sidebar_img_container.length != 0 && $('.switch-sidebar-image input:checked').length != 0) {
            $sidebar_img_container.fadeOut('fast', function () {
                $sidebar_img_container.css('background-image', 'url("' + new_image + '")');
                $sidebar_img_container.fadeIn('fast');
            });
        }

        if ($full_page_background.length != 0 && $('.switch-sidebar-image input:checked').length != 0) {
            var new_image_full_page = $('.fixed-plugin li.active .img-holder').find('img').data('src');

            $full_page_background.fadeOut('fast', function () {
                $full_page_background.css('background-image', 'url("' + new_image_full_page + '")');
                $full_page_background.fadeIn('fast');
            });
        }

        if ($('.switch-sidebar-image input:checked').length == 0) {
            var new_image = $('.fixed-plugin li.active .img-holder').find("img").attr('src');
            var new_image_full_page = $('.fixed-plugin li.active .img-holder').find('img').data('src');

            $sidebar_img_container.css('background-image', 'url("' + new_image + '")');
            $full_page_background.css('background-image', 'url("' + new_image_full_page + '")');
        }

        if ($sidebar_responsive.length != 0) {
            $sidebar_responsive.css('background-image', 'url("' + new_image + '")');
        }
    });

    $('.switch-mini input').on("switchChange.bootstrapSwitch", function () {
        $body = $('body');

        $input = $(this);

        if (paperDashboard.misc.sidebar_mini_active == true) {
            $('body').removeClass('sidebar-mini');
            paperDashboard.misc.sidebar_mini_active = false;
        } else {
            $('body').addClass('sidebar-mini');
            paperDashboard.misc.sidebar_mini_active = true;
        }

        // we simulate the window Resize so the charts will get updated in realtime.
        var simulateWindowResize = setInterval(function () {
            window.dispatchEvent(new Event('resize'));
        }, 180);

        // we stop the simulation of Window Resize after the animations are completed
        setTimeout(function () {
            clearInterval(simulateWindowResize);
        }, 1000);

    });

});

const toggleSettingsGear = document.querySelector(".navbar-toggle .navbar-toggler");

const menu = document.querySelector(".navbar-toggle");

const scrollbar = document.querySelector(".perfect-scrollbar-on");


toggleSettingsGear.onclick = function () {

    menu.classList.toggle("toggled");
    scrollbar.classList.toggle("nav-open");

};

document.addEventListener("click", (e) => {

    if (e.target !== toggleSettingsGear) {

        if (menu.classList.contains("toggled")) {

            menu.classList.toggle("toggled");

        }
        if (scrollbar.classList.contains("nav-open")) {

            scrollbar.classList.toggle("nav-open");

        }
    }
});

const arrow = document.querySelector("#minimizeSidebar .visible-on-sidebar-regular");

const smallMenu = document.querySelector("body");


arrow.onclick = function () {
    smallMenu.classList.toggle("sidebar-mini");
};

document.addEventListener("click", (e) => {

    if (e.target !== arrow) {

        if (smallMenu.classList.contains("sidebar-mini")) {

            smallMenu.classList.toggle("sidebar-mini");

        }
    }
});