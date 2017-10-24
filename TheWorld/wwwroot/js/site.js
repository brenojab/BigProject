// site.js
(function () {

    //var ele = $("#userName");
    //ele.text("Breno Batista");

    //var main = $("#main");
    //main.on("mouseenter", function () {
    //    main.css("background-color", "#E6AC00");
    //});

    //main.on("mouseleave", function () {
    //    main.css("background-color", "");
    //});

    //$(".menu li a").on("click", function () {
    //  alert($(this).text());
    //  return false;
    //});

    var $sidebarAndWrapper = $("#sidebar, #wrapper");
    var $icon = $("#sidebarToggle i.fa");

    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        } else {

            $icon.removeClass("fa-angle-right");
	                $icon.addClass("fa-angle-left");
        }
    });

})();