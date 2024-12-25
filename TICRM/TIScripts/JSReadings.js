$document.Ready(function () {

    
        alert('1234');

        var selectedReadingType = $("#ReadingTypeId").val();
        var selectedReadingUnit = $("#ReadingUnitId").val();


        //$("#ReadingUnitId").children('option:gt(0)').hide();


        $("#ReadingTypeId").click(function () {
            alert($('#ReadingTypeId').val());
            $("#ReadingUnitId").children('option').hide();
            $("#ReadingUnitId").children("option[value^=" + $(this).val() + "]").show()
        });
});