
$(document).ready(function () {
    //-------------------- Product Modal 

    $(".detailmodal").click(function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        console.log(url);

        fetch(url).then(response => {
            return response.text();
        })
            .then(data => {
                $(".modal-content").html(data);

                //slider product page
                $('.quick-view-image').slick({
                    slidesToShow: 1,
                    slidesToScroll: 1,
                    arrows: false,
                    dots: false,
                    fade: true,
                    asNavFor: '.quick-view-thumb',
                    speed: 400,
                });

                $('.quick-view-thumb').slick({
                    slidesToShow: 4,
                    slidesToScroll: 1,
                    asNavFor: '.quick-view-image',
                    dots: false,
                    arrows: false,
                    focusOnSelect: true,
                    speed: 400,
                });

            })

    });

    //-------------------- Search Navigation

    $(".input-search").keyup(function () {
        let inputvalue = $(this).val().trim();

        //------ Got the value of input type search. Do not forget to add asp-append-version="true" in layout script tag of custom.js

        let url = $(this).data('url');
        //------ Got the data-url of input to make fetch method by joining url to input value

        url = url + "?search=" + inputvalue;

        if (inputvalue) {
            console.log(inputvalue)
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $(".search-body .list-group").html(data);
                })
        }
        else {
            $(".search-body .list-group").html('');
        }
    });
})
