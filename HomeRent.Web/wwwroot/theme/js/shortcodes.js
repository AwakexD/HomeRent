/**
 * Counter
 * Preloader
 * Show Pass
 * Button Quantity
 * Input file
 * Delete image
 * Handle Search Form
 * Datepicker
 * Handle dashboard
 * Go Top
 * Cursor
 */

(function ($) {
  "use strict";

  var isMobile = {
    Android: function () {
      return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function () {
      return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function () {
      return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function () {
      return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function () {
      return navigator.userAgent.match(/IEMobile/i);
    },
    any: function () {
      return (
        isMobile.Android() ||
        isMobile.BlackBerry() ||
        isMobile.iOS() ||
        isMobile.Opera() ||
        isMobile.Windows()
      );
    },
  };

  /* Counter
  -------------------------------------------------------------------------------------*/
  var flatCounter = function () {
    if ($(document.body).hasClass("counter-scroll")) {
      var a = 0;
      $(window).scroll(function () {
        var oTop = $(".tf-counter").offset().top - window.innerHeight;
        if (a === 0 && $(window).scrollTop() > oTop) {
          if ($().countTo) {
            $(".tf-counter")
              .find(".number")
              .each(function () {
                var to = $(this).data("to"),
                  speed = $(this).data("speed"),
                  dec = $(this).data("dec");
                $(this).countTo({
                  to: to,
                  speed: speed,
                  decimals: dec,
                });
              });
          }
          a = 1;
        }
      });
    }
  };

  new WOW().init();

  /* check active 
  -------------------------------------------------------------------------*/
  var checkClick = function () {
    $(".box-faq").on("click", ".faq-item", function () {
      var isActive = $(this).hasClass("active");
      $(this).closest(".box-faq").find(".faq-item").removeClass("active");
      if (!isActive) {
        $(this).addClass("active");
      }
    });
  };

  /* Preloader
  -------------------------------------------------------------------------------------*/
  var preloader = function () {
    setTimeout(function () {
      $(".preload").fadeOut("slow", function () {
        $(this).remove();
      });
    }, 200);
  };

  /* Show Pass
  -------------------------------------------------------------------------------------*/
  var showPass = function () {
    $(".show-pass").on("click", function () {
      $(this).toggleClass("active");
      if ($(".password-field").attr("type") === "password") {
        $(".password-field").attr("type", "text");
      } else if ($(".password-field").attr("type") === "text") {
        $(".password-field").attr("type", "password");
      }
    });

    $(".show-pass2").on("click", function () {
      $(this).toggleClass("active");
      if ($(".password-field2").attr("type") === "password") {
        $(".password-field2").attr("type", "text");
      } else if ($(".password-field2").attr("type") === "text") {
        $(".password-field2").attr("type", "password");
      }
    });
    $(".show-pass3").on("click", function () {
      $(this).toggleClass("active");
      if ($(".password-field3").attr("type") === "password") {
        $(".password-field3").attr("type", "text");
      } else if ($(".password-field3").attr("type") === "text") {
        $(".password-field3").attr("type", "password");
      }
    });
  };
  /* Button Quantity
  -------------------------------------------------------------------------------------*/
  var btnQuantity = function () {
    $(".minus-btn").on("click", function (e) {
      e.preventDefault();
      var $this = $(this);
      var $input = $this.closest("div").find("input");
      var value = parseInt($input.val());

      if (value > 0) {
        value = value - 1;
      }

      $input.val(value);
    });

    $(".plus-btn").on("click", function (e) {
      e.preventDefault();
      var $this = $(this);
      var $input = $this.closest("div").find("input");
      var value = parseInt($input.val());

      if (value > -1) {
        value = value + 1;
      }

      $input.val(value);
    });
  };

  /* Input file 
  -------------------------------------------------------------------------------------*/
  var customInput = function () {
    $("input[type=file]").change(function (e) {
      $(this)
        .parents(".uploadfile")
        .find(".file-name")
        .text(e.target.files[0].name);
    });
  };

  /* Delete image 
  -------------------------------------------------------------------------------------*/
  var deleteImg = function (e) {
    $(".remove-file").on("click", function (e) {
      e.preventDefault();
      var $this = $(this);
      $this.closest(".file-delete").remove();
    });
  };
  /* Handle Search Form   
  -------------------------------------------------------------------------------------*/
  var clickSearchForm = function () {
    const widgetSearchForm = $(".wd-search-form");
    if (widgetSearchForm.length) {
      $(".pull-right").on("click", function () {
        widgetSearchForm.toggleClass("show");
      });
      $(document).on(
        "click.pull-right, click.offcanvas-backdrop",
        function (a) {
          if (
            $(a.target).closest(".pull-right, .wd-search-form").length === 0
          ) {
            widgetSearchForm.removeClass("show");
          }
        }
      );
    }
  };
  /* Handle Search popup
  -------------------------------------------------------------------------------------*/
  var handleSearchPopup = function () {
    var headerHeight = $("#header").outerHeight();

    $(".btn-search-popup").on("click", function () {
      if ($(".search-popup-wrapper").hasClass("open")) {
        $(".search-popup-wrapper").removeClass("open");
        $(".overlay2").fadeOut();
        $(".search-popup-wrapper").css("top", "-200%");
      } else {
        $(".search-popup-wrapper").addClass("open");
        $(".overlay2").fadeIn();
        $(".search-popup-wrapper").css("top", headerHeight + "px");
      }
    });

    $(".close-btn, .overlay2").on("click", function () {
      $(".search-popup-wrapper").removeClass("open");
      $(".overlay2").fadeOut();
      $(".search-popup-wrapper").css("top", "-200%");
    });
  };

  /* Datepicker  
  -------------------------------------------------------------------------------------*/
  var datePicker = function () {
    if ($("#datepicker1").length > 0) {
      $("#datepicker1").datepicker({
        firstDay: 1,
        dateFormat: "dd/mm/yy",
      });
    }
    if ($("#datepicker2").length > 0) {
      $("#datepicker2").datepicker({
        firstDay: 1,
        dateFormat: "dd/mm/yy",
      });
    }
    if ($("#datepicker3").length > 0) {
      $("#datepicker3").datepicker({
        firstDay: 1,
        dateFormat: "dd/mm/yy",
      });
    }
    if ($("#datepicker4").length > 0) {
      $("#datepicker4").datepicker({
        firstDay: 1,
        dateFormat: "dd/mm/yy",
      });
    }
  };

  /* Handle dashboard
  -------------------------------------------------------------------------------------*/
  var showHideDashboard = function () {
    $(".button-show-hide").on("click", function () {
      $(".layout-wrap").toggleClass("full-width");
    });
    $(".mobile-nav-toggler,.overlay-dashboard").on("click", function () {
      $(".layout-wrap").removeClass("full-width");
    });
  };

  /* Go Top
  -------------------------------------------------------------------------------------*/
  var goTop = function () {
    if ($("div").hasClass("progress-wrap")) {
      var progressPath = document.querySelector(".progress-wrap path");
      var pathLength = progressPath.getTotalLength();
      progressPath.style.transition = progressPath.style.WebkitTransition =
        "none";
      progressPath.style.strokeDasharray = pathLength + " " + pathLength;
      progressPath.style.strokeDashoffset = pathLength;
      progressPath.getBoundingClientRect();
      progressPath.style.transition = progressPath.style.WebkitTransition =
        "stroke-dashoffset 10ms linear";
      var updateprogress = function () {
        var scroll = $(window).scrollTop();
        var height = $(document).height() - $(window).height();
        var progress = pathLength - (scroll * pathLength) / height;
        progressPath.style.strokeDashoffset = progress;
      };
      updateprogress();
      $(window).scroll(updateprogress);
      var offset = 200;
      var duration = 0;
      jQuery(window).on("scroll", function () {
        if (jQuery(this).scrollTop() > offset) {
          jQuery(".progress-wrap").addClass("active-progress");
        } else {
          jQuery(".progress-wrap").removeClass("active-progress");
        }
      });
      jQuery(".progress-wrap").on("click", function (event) {
        event.preventDefault();
        jQuery("html, body").animate({ scrollTop: 0 }, duration);
        return false;
      });
    }
  };
  /* RTL
  ------------------------------------------------------------------------------------- */
  var RTL = function () {
    if (localStorage.getItem("dir") === "rtl") {
      $("html").attr("dir", "rtl");
      $("body").addClass("rtl");
      $('#toggle-rtl').text('ltr');
    } else {
      $("html").attr("dir", "ltr");
      $("body").removeClass("rtl");
      $('#toggle-rtl').text('rtl');      
      
    }
    $("#toggle-rtl").on("click", function() {
      if ($("html").attr("dir") === "rtl") {
        localStorage.setItem("dir", "ltr"); 
        $('#toggle-rtl').text('rtl');      

      } else {
        localStorage.setItem("dir", "rtl");
        $('#toggle-rtl').text('ltr');      
      }
      location.reload();
    });
  };



  // Dom Ready
  $(function () {
    flatCounter();
    customInput();
    btnQuantity();
    deleteImg();
    clickSearchForm();
    showHideDashboard();
    goTop();
    showPass();
    datePicker();
    checkClick();
    handleSearchPopup();
    RTL();
    preloader();
  });
})(jQuery);
