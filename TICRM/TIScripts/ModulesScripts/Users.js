/**
 * Code By Akhtar Zaman
 * 24/7/2020
 * Users script file Containing all the scripts it use
 */
//index Page Script Starts
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