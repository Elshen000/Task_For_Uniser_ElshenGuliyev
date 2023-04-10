$(document).ready(function () {


    //ProductSearch
     $(document).on('keyup', '#inputSearch', function () {
 $.ajax({
        url: "/Home/Search/",
        type: "GET",
        data: {
            "keyword": $("#inputSearch").val()
        },
        success: function (response) {
            if ($("#inputSearch").val().length > 0) {

                $("#myProducts div").slice().remove()
                $("#myProducts").append(response)
            }
            else {
                $("#myProducts").empty(),
                    $("#myProducts").append(response)
            };
        }
    });
})
    //ProductSearch


    //CategoryFilter
    $(document).on('change', '#myCats', function () {
        $.ajax({
            url: "/Home/CategoryFilter/",
            type: "GET",
            data: {
                "mycatId": $("#myCats").val()
            },
            success: function (response) {
                if ($("#myCats").val().length > 0) {

                    $("#myProducts div").slice().remove()
                    $("#myProducts").append(response)
                }
                else {
                    $("#myProducts").empty(),
                        $("#myProducts").append(response)
                };
            }
        });
    })
    //CategoryFilter


    //LoadMoreCategories
    let skip = 8;
    let proCount = $("#categoriesCount").val()
    $(document).on('click', '#LoadMore', function () {
        $.ajax({
            url: "/Categories/LoadMoreCategories/ ",
            type: "Get",
            data: {
                "skip": skip,
            },
            success: function (res) {
                $("#myCats").append(res)
                skip += 8;
                if (skip > proCount) {
                    $("#LoadMore").addClass("d-none")
                }
            }
        })
    })
    //LoadMoreCategories




    //FindProductForPrice
    $(document).on('click', '#myBtn', function () {
        $.ajax({
            url: "/Home/FindProductForPrice/ ",
            type: "GET",
            data: {
                "minimumpr": $("#minimumprice").val(),
                "maximumpr": $("#maximumprice").val(),
            },
            success: function (response) {
                if ($("#maximumprice").val().length > 0) {

                    $("#myProducts div").slice().remove()
                    $("#myProducts").append(response)
                }
                else {
                    $("#myProducts").empty(),
                        $("#myProducts").append(response)
                };


            }
        })
    });
    //FindProductForPrice
})