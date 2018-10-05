(function ($) {
    $.fn.imagebanner = function () {
        var x = $(this);

        var trans = 1000;
        var delay = 6000;

        var itemnext;
        var radnext;
        var itemact;
        var radact;

        var banner = x.find($('.banner'));
        var rad = x.find($('.banner__radial'));
        var controls = x.find($('.banner__controls'));


        if (!x.find($('.banner__item')).hasClass('banner__item--active')) {
            x.find($('.banner__item:first-child')).addClass('banner__item--active');
        }

        if (!x.find($('.banner__radial')).hasClass('banner__radial--active')) {
            x.find($('.banner__radial:first-child')).addClass('banner__radial--active');
        }

        var timer = setInterval(function () { transition() }, delay);

        function transition() {
            itemact = x.find($('.banner__item--active'));
            radact = x.find($('.banner__radial--active'));

            if (itemact.is(':last-child')) {
                itemnext = x.find($('.banner__item:first-child'));
                radnext = x.find($('.banner__radial:first-child'));
            }
            else {
                itemnext = itemact.next();
                radnext = radact.next();
            }
            clearInterval(timer);
            itemnext.addClass('banner__item--next');
            animatebanner();
        }

        x.find($('.banner__radial')).click(function () {
            if(!controls.hasClass('.banner__controls--na')){
                controls.addClass('.banner__controls--na');
                if (!banner.hasClass('.banner--animate')) {
                    var a = x.find($('.banner__item--active')).attr('data-val');
                    var b = x.find($(this)).attr('data-val');
                    if (a != b) {
                        clearInterval(timer);

                        itemact = x.find($('.banner__item--active'));
                        radact = x.find($('.banner__radial--active'));

                        itemnext = x.find($('.banner__item[data-val="' + b + '"]'));
                        radnext = $(this);

                        itemnext.addClass('banner__item--next');
                        animatebanner();
                    }
                }
            }
        });

        x.find($('.banner__nav')).click(function () {
            if (!controls.hasClass('.banner__controls--na')) {
                controls.addClass('.banner__controls--na');
                if (!banner.hasClass('.banner--animate')) {
                    itemact = x.find($('.banner__item--active'));
                    radact = x.find($('.banner__radial--active'));

                    if($(this).hasClass('banner__nav--next')){
                        if (itemact.is(':last-child')) {
                            itemnext = x.find($('.banner__item:first-child'));
                            radnext = x.find($('.banner__radial:first-child'));
                        }
                        else {
                            itemnext = itemact.next();
                            radnext = radact.next();
                        }
                    }
                    else{
                        if (itemact.is(':first-child')) {
                            itemnext = x.find($('.banner__item:last-child'));
                            radnext = x.find($('.banner__radial:last-child'));
                        }
                        else {
                            itemnext = itemact.prev();
                            radnext = radact.prev();
                        }
                    }
                    clearInterval(timer);
                    itemnext.addClass('banner__item--next');
                    animatebanner();
                }
            }
        });

        function animatebanner() {
            setTimeout(function () {

                

                banner.addClass('banner--animate');
                rad.removeClass('banner__radial--active');
                radnext.addClass('banner__radial--active');

                setTimeout(function () {
                    itemact.removeClass('banner__item--active');
                    itemnext.removeClass('banner__item--next').addClass('banner__item--active');
                    banner.removeClass('banner--animate');
                    controls.removeClass('.banner__controls--na');
                    timer = setInterval(function () { transition() }, delay);
                }, trans);

            }, 100);
        }

    }
})(jQuery);

$(document).ready(function () {
    $('.banner__wrap').imagebanner();
});