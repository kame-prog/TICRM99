using System.Web;
using System.Web.Optimization;
using TICRM.Helper;

namespace TICRM
{

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/Readings").Include(
                      "~/TIScripts/JSReadings.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            // metronic master CSS
            bundles.Add(new StyleBundle("~/bundles/MetronicMasterCSS").NonOrdering().Include(
                       "~/Content/Metronic/vendors/perfect-scrollbar/css/perfect-scrollbar.css",
                       "~/Content/Metronic/vendors/tether/dist/css/tether.css",
                       "~/Content/Metronic/vendors/bootstrap-datepicker/dist/css/bootstrap-datepicker3.min.css",
                       "~/Content/Metronic/vendors/bootstrap-datetime-picker/css/bootstrap-datetimepicker.min.css",
                       "~/Content/Metronic/vendors/bootstrap-timepicker/css/bootstrap-timepicker.min.css",
                       "~/Content/Metronic/vendors/bootstrap-daterangepicker/daterangepicker.css",
                       "~/Content/Metronic/vendors/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.css",
                       "~/Content/Metronic/vendors/bootstrap-switch/dist/css/bootstrap3/bootstrap-switch.css",
                       "~/Content/Metronic/vendors/bootstrap-select/dist/css/bootstrap-select.css",
                       "~/Content/Metronic/vendors/select2/dist/css/select2.css",
                       "~/Content/Metronic/vendors/nouislider/distribute/nouislider.css",
                       "~/Content/Metronic/vendors/owl.carousel/dist/assets/owl.carousel.css",
                       "~/Content/Metronic/vendors/owl.carousel/dist/assets/owl.theme.default.css",
                       "~/Content/Metronic/vendors/ion-rangeslider/css/ion.rangeSlider.css",
                       "~/Content/Metronic/vendors/ion-rangeslider/css/ion.rangeSlider.skinFlat.css",
                       "~/Content/Metronic/vendors/dropzone/dist/dropzone.css",
                       "~/Content/Metronic/vendors/summernote/dist/summernote.css",
                       "~/Content/Metronic/vendors/bootstrap-markdown/css/bootstrap-markdown.min.css",
                       "~/Content/Metronic/vendors/animate.css/animate.css",
                       "~/Content/Metronic/vendors/toastr/build/toastr.css",
                       "~/Content/Metronic/vendors/jstree/dist/themes/default/style.css",
                       "~/Content/Metronic/vendors/morris.js/morris.css",
                       "~/Content/Metronic/vendors/chartist/dist/chartist.min.css",
                       "~/Content/Metronic/vendors/sweetalert2/dist/sweetalert2.min.css",
                       "~/Content/Metronic/vendors/socicon/css/socicon.css",
                       "~/Content/Metronic/vendors/vendors/line-awesome/css/line-awesome.css",
                       "~/Content/Metronic/vendors/vendors/flaticon/css/flaticon.css",
                       "~/Content/Metronic/vendors/vendors/metronic/css/styles.css",
                       "~/Content/Metronic/vendors/vendors/fontawesome5/css/all.min.css",
                       "~/Content/Metronic/vendors/select2/dist/css/select2.min.css",

                      "~/Content/Metronic/vendors/graphhopper/css/leaflet.css",
                      "~/Content/Metronic/vendors/graphhopper/css/style.css",
                       "~/Content/Metronic/assets/demo/base/style.bundle.css",
                       "~/Content/Metronic/assets/vendors/custom/fullcalendar/fullcalendar.bundle.css",
                      "~/Content/Metronic/vendors/jvector/jquery-jvectormap.css",
                       "~/Content/Metronic/assets/vendors/custom/datatables/datatables.bundle.css",
                       "~/Content/CustomCSS.css",
                       "~/Content/EasyAutocomplete-1.3.5/easy-autocomplete.min.css",
                       "~/Content/EasyAutocomplete-1.3.5/easy-autocomplete.themes.min.css"
                       ));

            bundles.Add(new StyleBundle("~/bundles/DashboardCSS").NonOrdering().Include(
                      "~/Content/Metronic/vendors/jvector/jquery-jvectormap.css",
                      "~/Content/Metronic/vendors/graphhopper/css/leaflet.css"
                ));
            bundles.Add(new StyleBundle("~/bundles/AdminCSS").NonOrdering().Include(
                      "~/Content/Metronic/vendors/jvector/jquery-jvectormap.css",
                      "~/Content/Metronic/vendors/graphhopper/css/leaflet.css"
                ));
            bundles.Add(new StyleBundle("~/bundles/DevicesCSS").NonOrdering().Include(

                      "~/Content/Metronic/vendors/jvector/jquery-jvectormap.css",
                      "~/Content/Metronic/vendors/graphhopper/css/leaflet.css",
                      "~/Content/Metronic/vendors/graphhopper/css/style.css"
                ));
            // metronic master script
            bundles.Add(new ScriptBundle("~/bundles/MetronicMasterScript").NonOrdering().Include(
                     "~/Content/Metronic/vendors/jquery/dist/jquery.js",
                    "~/Content/Metronic/vendors/jvector/jquery-jvectormap.js",
                   
                "~/Content/Metronic/vendors/jvector/tests/assets/jquery-jvectormap-world-mill-en.js",
                     "~/Content/Metronic/vendors/popper.js/dist/umd/popper.js",
                     "~/Content/Metronic/vendors/bootstrap/dist/js/bootstrap.min.js",
                     "~/Content/Metronic/vendors/js-cookie/src/js.cookie.js",
                     "~/Content/Metronic/vendors/moment/min/moment.min.js",
                     "~/Content/Metronic/vendors/tooltip.js/dist/umd/tooltip.min.js",
                     "~/Content/Metronic/vendors/perfect-scrollbar/dist/perfect-scrollbar.js",
                     "~/Content/Metronic/vendors/wnumb/wNumb.js",
                     "~/Content/Metronic/vendors/jquery.repeater/src/lib.js",
                     "~/Content/Metronic/vendors/jquery.repeater/src/jquery.input.js",
                     "~/Content/Metronic/vendors/jquery.repeater/src/repeater.js",
                     "~/Content/Metronic/vendors/jquery-form/dist/jquery.form.min.js",
                     "~/Content/Metronic/vendors/block-ui/jquery.blockUI.js",
                     "~/Content/Metronic/vendors/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/forms/bootstrap-datepicker.init.js",
                     "~/Content/Metronic/vendors/bootstrap-datetime-picker/js/bootstrap-datetimepicker.min.js",
                     "~/Content/Metronic/vendors/bootstrap-timepicker/js/bootstrap-timepicker.min.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/forms/bootstrap-timepicker.init.js",
                     "~/Content/Metronic/vendors/bootstrap-daterangepicker/daterangepicker.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/forms/bootstrap-daterangepicker.init.js",
                     "~/Content/Metronic/vendors/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.js",
                     "~/Content/Metronic/vendors/bootstrap-maxlength/src/bootstrap-maxlength.js",
                      "~/Content/Metronic/vendors/bootstrap-switch/dist/js/bootstrap-switch.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/forms/bootstrap-switch.init.js",
                    "~/Content/Metronic/vendors/vendors/bootstrap-multiselectsplitter/bootstrap-multiselectsplitter.min.js",
                     "~/Content/Metronic/vendors/bootstrap-select/dist/js/bootstrap-select.js",
                     "~/Content/Metronic/vendors/select2/dist/js/select2.full.js",
                     "~/Content/Metronic/vendors/typeahead.js/dist/typeahead.bundle.js",
                     "~/Content/Metronic/vendors/handlebars/dist/handlebars.js",
                     "~/Content/Metronic/vendors/inputmask/dist/jquery.inputmask.bundle.js",
                     "~/Content/Metronic/vendors/inputmask/dist/inputmask/inputmask.date.extensions.js",
                     "~/Content/Metronic/vendors/inputmask/dist/inputmask/inputmask.numeric.extensions.js",
                     "~/Content/Metronic/vendors/inputmask/dist/inputmask/inputmask.phone.extensions.js",
                     "~/Content/Metronic/vendors/nouislider/distribute/nouislider.js",
                     "~/Content/Metronic/vendors/owl.carousel/dist/owl.carousel.js",
                     "~/Content/Metronic/vendors/autosize/dist/autosize.js",
                     "~/Content/Metronic/vendors/clipboard/dist/clipboard.min.js",
                     "~/Content/Metronic/vendors/ion-rangeslider/js/ion.rangeSlider.js",
                     "~/Content/Metronic/vendors/dropzone/dist/dropzone.js",
                     "~/Content/Metronic/vendors/summernote/dist/summernote.js",
                     "~/Content/Metronic/vendors/markdown/lib/markdown.js",
                     "~/Content/Metronic/vendors/bootstrap-markdown/js/bootstrap-markdown.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/forms/bootstrap-markdown.init.js",
                     "~/Content/Metronic/vendors/jquery-validation/dist/jquery.validate.js",
                     "~/Content/Metronic/vendors/jquery-validation/dist/additional-methods.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/forms/jquery-validation.init.js",
                     "~/Content/Metronic/vendors/bootstrap-notify/bootstrap-notify.min.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/base/bootstrap-notify.init.js",
                     "~/Content/Metronic/vendors/toastr/build/toastr.min.js",
                     "~/Content/Metronic/vendors/jstree/dist/jstree.js",
                     "~/Content/Metronic/vendors/raphael/raphael.js",
                     "~/Content/Metronic/vendors/morris.js/morris.js",
                     "~/Content/Metronic/vendors/chartist/dist/chartist.js",
                     "~/Content/Metronic/vendors/chart.js/dist/Chart.bundle.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/charts/chart.init.js",
                      "~/Content/Metronic/vendors/vendors/bootstrap-session-timeout/dist/bootstrap-session-timeout.min.js",
                     "~/Content/Metronic/vendors/vendors/jquery-idletimer/idle-timer.min.js",
                     "~/Content/Metronic/vendors/waypoints/lib/jquery.waypoints.js",
                     "~/Content/Metronic/vendors/counterup/jquery.counterup.js",
                     "~/Content/Metronic/vendors/es6-promise-polyfill/promise.min.js",
                     "~/Content/Metronic/vendors/sweetalert2/dist/sweetalert2.min.js",
                     "~/Content/Metronic/vendors/js/framework/components/plugins/base/sweetalert2.init.js",
                     "~/Content/Metronic/vendors/select2/dist/js/select2.full.min.js",
                     
                     "~/Content/Metronic/vendors/scrollview/touchSwipe.js",
                     "~/Content/Metronic/vendors/scrollview/horizonScroll.js",

                     "~/Content/Metronic/assets/demo/base/scripts.bundle.js",
                     "~/Content/EasyAutocomplete-1.3.5/jquery.easy-autocomplete.js",
                     "~/Content/Metronic/assets/demo/custom/crud/forms/validation/form-controls.js",
                     "~/Content/Metronic/assets/demo/custom/crud/forms/widgets/bootstrap-touchspin.js",
                     "~/Content/Metronic/assets/demo/custom/crud/forms/widgets/bootstrap-daterangepicker.js",
                     "~/Content/Metronic/assets/demo/custom/crud/forms/widgets/bootstrap-maxlength.js",
                     "~/Content/Metronic/assets/vendors/custom/datatables/datatables.bundle.js",
                     "~/Content/Metronic/vendors/graphhopper/js/leaflet.js",
                     "~/Content/Metronic/vendors/graphhopper/js/bouncemarker.js",
                     "~/Content/Metronic/vendors/graphhopper/dist/graphhopper-client.js",
                     "~/Scripts/customJs/master.js"


     

            ));

            bundles.Add(new ScriptBundle("~/bundles/ActivitiesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Activities.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ActivitiesTemplatesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/ActivityTemplates.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/AddressesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Addresses.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/AlertsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Alerts.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/BulkDevicesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/BulkDevices.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/CalendersScript").NonOrdering().Include(
                      "~/Content/TagInput/taginput.js",
                      "~/TIScripts/ModulesScripts/Calenders.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/CategoriesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Categories.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/CloudConfigurationScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/CloudConfiguration.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ContactsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Contacts.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/CustomerAssetsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/CustomerAssets.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/EmailConfigurationsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/EmailConfigurations.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/EmailTemplatesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/EmailTemplates.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/GlobalSearchScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/GlobalSearch.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/LeadsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Leads.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/LocationsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Locations.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/OpportunitiesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Opportunities.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ProductCatelogsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/ProductCatelogs.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ReadingsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Readings.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ReadingTypesScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/ReadingTypes.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ReadingUnitsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/ReadingUnits.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ResourcesScript").NonOrdering().Include(
                     "~/Content/Metronic/assets/vendors/custom/datatables/datatables.bundle.js",
                      "~/TIScripts/ModulesScripts/Resources.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/ServiceCallsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/ServiceCalls.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/UsersScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/Users.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/WorkFlowMappingsScript").NonOrdering().Include(
                     "~/Content/Metronic/assets/vendors/custom/datatables/datatables.bundle.js",

                      "~/TIScripts/ModulesScripts/WorkFlowMappings.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/WorkFlowReportsScript").NonOrdering().Include(
                                      "~/TIScripts/ModulesScripts/WorkFlowReports.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/WorkFlowsScript").NonOrdering().Include(
                      "~/TIScripts/ModulesScripts/WorkFlows.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/WorkOrdersScript").NonOrdering().Include(
                "~/TIScripts/ModulesScripts/WorkOrders.js"

                      ));
            bundles.Add(new ScriptBundle("~/bundles/AdminScript").NonOrdering().Include(

                "~/Content/Metronic/vendors/graphhopper/js/leaflet.js",
                "~/Content/Metronic/vendors/graphhopper/js/bouncemarker.js",
                "~/Content/Metronic/vendors/graphhopper/dist/graphhopper-client.js",
                "~/Content/Metronic/vendors/jvector/jquery.jvectormap.min.js",
                "~/Content/Metronic/vendors/jvector/tests/assets/jquery-jvectormap-world-mill-en.js",
                "~/Content/Metronic/vendors/canvasjs/canvasjs.min.js",
                "~/TIScripts/ModulesScripts/Admin.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/DevicesScript").NonOrdering().Include(
                    // "~/Content/Metronic/assets/vendors/custom/datatables/datatables.bundle.js",

                //"~/Content/Metronic/vendors/graphhopper/js/leaflet.js",
                    // "~/Content/Metronic/vendors/graphhopper/js/bouncemarker.js",
                     //"~/Content/Metronic/vendors/graphhopper/dist/graphhopper-client.js",
                       "~/Content/Metronic/vendors/mqtt/Mqtt.js",
                       "~/TIScripts/ModulesScripts/Devices.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/DashboardScript").NonOrdering().Include(
                        "~/Content/Metronic/vendors/jvector/jquery.jvectormap.min.js",
                       "~/Content/Metronic/vendors/jvector/tests/assets/jquery-jvectormap-world-mill-en.js",
                       "~/Content/Metronic/vendors/canvasjs/canvasjs.min.js",
                     "~/Content/Metronic/vendors/graphhopper/js/leaflet.js",
                     "~/Content/Metronic/vendors/graphhopper/js/bouncemarker.js",
                     "~/Content/Metronic/vendors/graphhopper/dist/graphhopper-client.js",
                       
                       "~/TIScripts/ModulesScripts/Dashboard.js"
                      ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
