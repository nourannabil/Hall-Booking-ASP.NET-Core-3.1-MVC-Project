$(document).ready(function () {

    Category();

    $('#category').change(function () {
        var id = $(this).val();
        $('#Hall').empty();
        $('#Hall').append('<Option> --Select Hall--</Option>');
        $.ajax({
            url: '/CasscadingDropDownList/Hall?id=' + id,
            success: function (result) {
                $.each(result, function (i, data) {
                    $('#Hall').append('<option value=' + data.id + '>' + data.name + '</option>');
                });
            }
        });
    });



    $('#Hall').change(function () {
        var id = $(this).val();
        $('#see').click(function () {
            $.ajax({
                success: function (data) {
                    window.location.href = '/Features/SeeFeature?id=' + id;
                },
            });

        });
    });


    $('#Hall').change(function () {
        var id = $(this).val();
        $('#look').click(function () {
            $.ajax({
                success: function (data) {
                    window.location.href = '/HallPhotoes/SeePhoto?id=' + id;
                },
            });

        });
    });
});

function Category() {
    $.ajax({
        url: '/CasscadingDropDownList/Category',
        success: function (result) {
            $.each(result, function (i, data) {
                $('#category').append('<option value=' + data.id + ' >' + data.categoryName + '</option>');
            });
        }
    });
}


