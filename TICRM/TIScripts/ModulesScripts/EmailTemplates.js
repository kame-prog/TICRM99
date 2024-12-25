/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Email Templates script file Containing all the scripts it use
 */
var url = window.location.pathname.toLowerCase();

//index Page Script Starts
if (url == "/emailtemplates/index" || url == "/emailtemplates") {
    $(document).ready(function () {
        $('#m_table_1').DataTable({
            responsive: true,
            scrollY: false,
            scrollX: true,
            scrollCollapse: true,
            //== Pagination settings
            dom: `<'row'<'col-sm-6 text-left'f><'col-sm-6 text-right'B>>
                                <'row'<'col-sm-12'tr>>
                                <'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7 dataTables_pager'lp>>`,

            buttons: [
                'print',
                'pdfHtml5',
            ]
        });
    });

    //NOTE: don't rename it LoadModalForDetails and its Get URL in Ajax
    var LoadModalForDetails = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch
        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/EmailTemplates/PartialDetailsOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                //$('.modal-content').html('').html(response);
                $('.modal-content-rightside').html('').html(response);
                $("#loader").css("display", "none");
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });
    }

    var LoadModalForDelete = function (id) {
        // not remove this function because it use for open details of account form search click it use in easy auto complte funtion contact to development branch
        // in master page use in EasyautoCompleteSearch
        $('#RightSideModal').modal('show');
        $("#loader").css("display", "block");

        $.ajax({
            type: "GET",
            url: "/EmailTemplates/PartialDeleteOnId",
            data: { id: id },
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            success: function (response) {
                $('.modal-content-rightside').html('').html(response);
                $("#loader").css("display", "none");
            },
            failure: function () {
                alert("Failed!");
                $('body').removeClass('m-page--loading');
            }
        });
    }
}
//index Page Script Ends

//Edit Page Script Starts
if (url == "/emailtemplates/edit") {
    $(document).on('click', '#btnSubmit', function () {

        $('#Body').val(CKEDITOR.instances['editor'].getData());

        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        });
        $('#EditEmailTemplate').submit();
    });

    $(document).ready(function () {
        initSample();
        CKEDITOR.instances['editor'].setData($('#Body').val());
    });

    if (CKEDITOR.env.ie && CKEDITOR.env.version < 9)
        CKEDITOR.tools.enableHtml5Elements(document);

    // The trick to keep the editor in the sample quite small
    // unless user specified own height.
    CKEDITOR.config.height = 150;
    CKEDITOR.config.width = 'auto';

    var initSample = (function () {
        var wysiwygareaAvailable = isWysiwygareaAvailable(),
            isBBCodeBuiltIn = !!CKEDITOR.plugins.get('bbcode');

        return function () {
            var editorElement = CKEDITOR.document.getById('editor');

            // :(((
            if (isBBCodeBuiltIn) {
                editorElement.setHtml(
                    'Hello world Pakistan!\n\n' +
                    'I\'m an instance of [url=https://ckeditor.com]CKEditor[/url].'
                );
            }

            // Depending on the wysiwygarea plugin availability initialize classic or inline editor.
            if (wysiwygareaAvailable) {
                CKEDITOR.replace('editor');
            } else {
                editorElement.setAttribute('contenteditable', 'true');
                CKEDITOR.inline('editor');

                // TODO we can consider displaying some info box that
                // without wysiwygarea the classic editor may not work.
            }
        };

        function isWysiwygareaAvailable() {
            // If in development mode, then the wysiwygarea must be available.
            // Split REV into two strings so builder does not replace it :D.
            if (CKEDITOR.revision == ('%RE' + 'V%')) {
                return true;
            }

            return !!CKEDITOR.plugins.get('wysiwygarea');
        }
    })();
}
//Edit Page Script Ends

//Details Page Script Starts
if (url == "/emailtemplates/details") {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EmailTemplates/Index">EmailTemplates</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Details", "EmailTemplates", new { id = Model.EmailTemplateId })">@Model.Subject</a></li>');
    });
}
//Details Page Script Ends

//Delete Page Script Starts
if (url == "/emailtemplates/delete") {
    $(document).ready(function () {
        $('#searchNavigationList').append('<li><a href="/EmailTemplates/Index">Email Templates</a></li>');
        $('#searchNavigationList').append('<li><a href="@Url.Action("Delete", "EmailTemplates", new { id = Model.EmailTemplateId })">@Model.Subject</a></li>');
    });
}
//Delete Page Script Ends

//Create Page Script Starts
if (url == "/emailtemplates/create") {
    $(document).on('click', '#btnSubmit', function () {
        $('#Body').val(CKEDITOR.instances.editor1.getData());
        mApp.blockPage({
            overlayColor: "#000000",
            type: "loader",
            state: "primary",
            message: "Processing..."
        });
        $('#CreateNewEmailTemplate').submit();
    });

    if (CKEDITOR.env.ie && CKEDITOR.env.version < 9)
        CKEDITOR.tools.enableHtml5Elements(document);
    // The trick to keep the editor in the sample quite small
    // unless user specified own height.
    CKEDITOR.config.height = 150;
    CKEDITOR.config.width = 'auto';
    var initSample = (function () {
        var wysiwygareaAvailable = isWysiwygareaAvailable(),
            isBBCodeBuiltIn = !!CKEDITOR.plugins.get('bbcode');
        return function () {
            var editorElement = CKEDITOR.document.getById('editor');
            // :(((
            if (isBBCodeBuiltIn) {
                editorElement.setHtml(
                    'Hello world Pakistan!\n\n' +
                    'I\'m an instance of [url=https://ckeditor.com]CKEditor[/url].'
                );
            }
            // Depending on the wysiwygarea plugin availability initialize classic or inline editor.
            if (wysiwygareaAvailable) {
                CKEDITOR.replace('editor');
            } else {
                editorElement.setAttribute('contenteditable', 'true');
                CKEDITOR.inline('editor');
                // TODO we can consider displaying some info box that
                // without wysiwygarea the classic editor may not work.
            }
        };
        function isWysiwygareaAvailable() {
            // If in development mode, then the wysiwygarea must be available.
            // Split REV into two strings so builder does not replace it :D.
            if (CKEDITOR.revision == ('%RE' + 'V%')) {
                return true;
            }
            return !!CKEDITOR.plugins.get('wysiwygarea');
        }
    })();
}
//Create Page Script Ends

//Delete item from modal
var Delete_Record = function () {
    $('body').addClass('m-page--loading');
    $('#DeleteForm').submit();
}