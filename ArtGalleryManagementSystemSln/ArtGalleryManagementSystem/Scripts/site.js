$("#btnPlus").click(function (e) {
    e.preventDefault();
    var sk = $(".ExhibitionContainer")
    $.ajax({
        url: "/Arts/AddExhibitions",
        type: "GET",
        success: function (data) {
            sk.append(data)
        }
    })
})
$(document).on("click", "#deleteExhibition", function (e) {
    e.preventDefault();
    $(this).parent().parent().remove()
})

function ReadUrl(i) {
    if (i.files && i.files[0]) {
        var readar = new FileReader();
        readar.onload = function (e) {
            $("#Imgfile").attr('src', e.target.result);
        }
        readar.readAsDataURL(i.files[0])
    }
}