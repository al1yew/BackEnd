
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
        let inputvalue = $(this).val();

        //------ Got the value of input type search. Do not forget to add asp-append-version="true" in layout script tag of custom.js

        let url = $(this).data('url');
        //------ Got the data-url of input to make fetch method by joining url to input value

        url = url + '?search=' + inputvalue;
        //------ Url must be like http://localhost:35130/product/Search + our new url 
        //------ If(inputvalue) proves the fact that if imputvalue is 0, it returns FALSE, if inputvalue is 1 or more, it returns TRUE
        //------ Now we fetch it to small ul li absolute item with update page without page reload

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

    //-------------------- Basket

    $(document).on("click", ".addtobasket", function (e) {
        e.preventDefault();
        let url = $(this).attr('href');
        console.log('slammsmamssm')
        console.log(url)
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".header-cart").html(data);

                //fetch(/Basket/GetBasket)
                //    .then(res => res.text())
                //    .then(data => {
                //        $('.basketindexcontainer').html(data);
                //    });
            })
    })


    $(document).on('click', '.deletefrombasket', function (e) {
        e.preventDefault();
        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $(".header-cart").html(data);

                //fetch(/Basket/GetBasket)
                //    .then(res => res.text())
                //    .then(data => {
                //        $('.basketindexcontainer').html(data);
                //    });
            })
    })


    $(document).on('click', '.subCount', function (e) {
        e.preventDefault();
        let inputCount = $(this).next().val();

        if (inputCount >= 1) {
            inputCount--;
            $(this).next().val(inputCount);
            let url = $(this).attr('href') + '/?count=' + inputCount;
            console.log('sub');

            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $('.basketindexcontainer').html(data);

                    fetch('/basket/getbasket')
                        .then(res => res.text())
                        .then(data => {
                            $('.header-cart').html(data);
                        });
                });
        }
    })


    $(document).on('click', '.addCount', function (e) {
        e.preventDefault();
        let inputCount = $(this).prev().val();

        if (inputCount > 0) {
            inputCount++;
        } else {
            inputCount = 1;
        }

        $(this).prev().val(inputCount);

        let url = $(this).attr('href') + '/?count=' + inputCount;
        console.log('add');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.basketindexcontainer').html(data);

                fetch('/basket/getbasket')
                    .then(res => res.text())
                    .then(data => {
                        $('.header-cart').html(data);
                    });
            });
    })


    $(document).on('click', '.deletefromcartbtn', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.basketindexcontainer').html(data);

                fetch('/basket/getbasket')
                    .then(res => res.text())
                    .then(data => {
                        $('.header-cart').html(data);
                    });
            })
    })

    //-------------------- Tabmenu in homepage

    let path = window.location.pathname
    path = path.split('/')
    let links = $('.header-menu-for-tabmenu')

    for (var i = 0; i < links.length; i++) {
        let hrefpath = links[i].children[0].getAttribute('href').split('/')
        if (hrefpath[1].toLowerCase() == path[1].toLowerCase()) {
            links[i].classList.add('active')
        } else {
            links[i].classList.remove('active')
        }
    }

    //-------------------- Toastr

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    if ($('#successInput').val().length) {
        toastr["success"]($('#successInput').val(), $('#successInput').val().split(' ')[0])
    }

    //-------------------- Account Orders toggle table


    $(document).on('show.bs.collapse', '.accordian-body', function () {
        $(this).closest("table")
            .find(".collapse.in")
            .not(this)
        //.collapse('toggle')
    })
})

