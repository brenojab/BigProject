// site.js
(function () {

    var ele = $("#userName");
    ele.text("Breno Batista");

    var main = $("#main");
    main.on("mouseenter", function () {
        main.css("background-color", "#E6AC00");
    });

    main.on("mouseleave", function () {
        main.css("background-color", "");
    });

  //$(".menu li a").on("click", function () {
  //  alert($(this).text());
  //  return false;
  //});

    var $sidebarAndWrapper = $("#sidebar,#wrapper");

  $("#menuToggle").on("click", function () {
    $sidebarAndWrapper.toggleClass("hide-sidebar");
    if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
      $(this).text("Exibe Menu");
    } else {
      $(this).text("Oculta Menu");
    }
  });

})();