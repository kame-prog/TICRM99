using System.Web;
using System.Web.Optimization;

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
            bundles.Add(new StyleBundle("~/bundles/MetronicMasterCSS").Include(
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
                      "~/Content/Metronic/vendors/jqvmap/dist/jqvmap.css"));


            // metronic master script
            bundles.Add(new ScriptBundle("~/bundles/MetronicMasterScript").Include(
                     "~/Content/Metronic/vendors/jquery/dist/jquery.js",
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
                     //"~/Content/Metronic/vendors/js/framework/components/plugins/forms/jquery-validation.init.js",
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
                     "~/Content/Metronic/vendors/jqvmap/dist/jquery.vmap.js",
                     "~/Content/Metronic/vendors/jqvmap/dist/maps/jquery.vmap.world.js"));



        }
    }
}
