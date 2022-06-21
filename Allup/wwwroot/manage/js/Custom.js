$(document).ready(function () {
    $(document).on('click', '.deleteBtn', function (e) {
        e.preventDefault();


        Swal.fire({
            title: 'Eminsen?',
            text: "Bunu Geri Qaytara Bilmiyecen!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: "Silme",
            confirmButtonText: 'Beli, Sil!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => { $('.tblContent').html(data) });

                Swal.fire(
                    'Silindi!',
                    'Secdiyniz Brand Silindi.',
                    'success'
                )
            }
        })
    })

    $(document).on('click', '.restoreBtn', function (e) {
        e.preventDefault();

        Swal.fire({
            title: 'Eminsen?',
            text: "Bunu Geri Qaytara Bilmiyecen!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: "Silme",
            confirmButtonText: 'Beli, Sil!'
        }).then((result) => {
            if (result.isConfirmed) {

                let url = $(this).attr('href');

                fetch(url)
                    .then(res => res.text())
                    .then(data => { $('.tblContent').html(data) });

                Swal.fire(
                    'Silindi!',
                    'Secdiyniz Brand Silindi.',
                    'success'
                )
            }
        })
    })

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
})