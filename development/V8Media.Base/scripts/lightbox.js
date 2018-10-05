(function ($) {

    /********** CLICK MODAL BUTTON **********/
    $(document).on("click", ".lightbox", function (e) {
        e.preventDefault();

        /********** CREATE LIGHTBOX **********/
        if ($('.lb-ol').length < 1) { //checks if lightbox has been created
            $('body').append(
				'<div class="lb-ol">'
					+ '<div class="lb-close"><span></span></div>'
					+ '<div class="lb-wrap">'
						+ '<div class="lb-inner">'
							+ '<div class="lb-content">'
								+ '<img src=""/>'
							+ '</div>'
						+ '</div>'
					+ '</div>'
					+ '<div class="lb-nav lb-prev"><span></span></div>'
					+ '<div class="lb-nav lb-next"><span></span></div>'
				+ '</div>'
			);
        }
        /*** ***/

        var data = $(this).attr('data-rel');
        var id = $(this).closest('body').find('[data-rel="' + data + '"]').index(this);
        var image = $(this).attr("href");

        var count = $('[data-rel="' + data + '"]').length; // number of items in collection	


        /********** PRELOAD PREV & NEXT SLIDES **********/
        var pre; var nxt;
        if (id == 0) { pre = count - 1; }
        else { pre = id - 1; }

        if (id == count - 1) { nxt = count - 1; }
        else { nxt = id + 1; }

        $('<img/>')[0].src = $('[data-rel="' + data + '"]').eq(pre).attr("href");
        $('<img/>')[0].src = $('[data-rel="' + data + '"]').eq(nxt).attr("href");
        /*** ***/

        $('.lb-content img').attr('src', image); // add image to lightbox

        $('body').addClass('lb-open'); //add class to trigger css animation
        $('.lb-ol').css("opacity", "1");
        //delay required to allow elements to be added to DOM first

        /********** CLOSE LIGHTBOX **********/
        $(document).on("click", ".lb-close", function () {
            $('body').removeClass('lb-open');
            $(document).unbind('keydown');
            $('.lb-ol').removeAttr("style");
        });
        $(document).keydown(function (x) {
            x.preventDefault();
            if (x.which == 27) {
                //esc 27
                $('body').removeClass('lb-open');
                $(document).unbind('keydown');
                $('.lb-ol').removeAttr("style");
            }
        });
        /*** ***/

        if (data != "") {
            /********** PREV & NEXT CONTROLS **********/
            $(document).on("click", ".lb-nav", function () {
                if ($(this).hasClass('lb-next')) { idnext(); }
                else { idprev(); }
                updateimage();
            });
            /*** ***/

            /********** KEY CONTROLS **********/
            $(document).keydown(function (k) {
                k.preventDefault();
                //right 39 //enter 13 //space 32 //down 40 //tab 9
                if (k.which == 39 || k.which == 13 || k.which == 32 || k.which == 40 || k.which == 9) { idnext(); }
                //left 37 //up 38 //backspace 8
                if (k.which == 37 || k.which == 38 || k.which == 8) { idprev(); }
                updateimage();
            });
            /*** ***/

            /********** ADD SWIPE FUNCTIONALITY **********
			$(document).swipe( {
				//Generic swipe handler for all directions
				swipe:function(event, direction, distance, duration, fingerCount, fingerData) {
					if (direction == "left")	{ idnext(); }
					if (direction == "right")	{ idprev();	}
					updateimage();
				}
			});
			/*** ***/

            var preload;

            function idprev() {
                if (id == 0) { id = count - 1; }
                else { id = id - 1; }
                preload = id - 1;
            }

            function idnext() {
                if (id == count - 1) { id = 0; }
                else { id = id + 1; }
                preload = id + 1;
            }

            function updateimage() {
                $('<img/>')[0].src = $('[data-rel="' + data + '"]').eq(preload).attr("href"); //preloads next image
                var imagenew = $('[data-rel="' + data + '"]').eq(id).attr('href');
                $('.lb-content img').attr('src', imagenew); // add image to lightbox
            }
        }

    });

})(jQuery);