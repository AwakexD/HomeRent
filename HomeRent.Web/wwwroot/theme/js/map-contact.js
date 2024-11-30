/*var infoBox_ratingType='star-rating';*/

(function ($) {
  "use strict";
  function mainMap() {
    var ib = new InfoBox();
    function locationData(img, location, phone, mail) {
      return (
        "" +
        '<div class="contact-map-item">' +
        '<div class="inner-box">' +
        '<div class="infoBox-close"><i class="icon icon-close2"></i></div>' +
        '<div class="image-box">' +
        '<img src="' +
        img +
        '" alt="">' +
        "</div>" +
        '<div class="content">' +
        '<div class="title">Office address' +
        "</div>" +
        '<ul class="list-info">' +
        '<li><span class="icon icon-map-trifold"></span>' +
        location +
        "</li>" +
        '<li><span class="icon icon-phone-line"></span>' +
        phone +
        "</li>" +
        '<li><span class="icon icon-mail-line"></span>' +
        mail +
        "</li>" +
        "</ul>" +
        "</div>" +
        "</div>" +
        "</div>"
      );
    }

    var locations = [
      [
        locationData(
          "images/banner/contact.jpg",
          "101 E 129th St, East Chicago, IN 46312, US",
          "1-333-345-6868",
          "themesflat@gmail.com"
        ),
        40.709327,
        -74.004815,
        1,
        "<div></div>",
      ],
    ];

    function numericalRating(ratingElem) {
      $(ratingElem).each(function () {
        var dataRating = $(this).attr("data-rating");
        if (dataRating >= 4.0) {
          $(this).addClass("high");
        } else if (dataRating >= 3.0) {
          $(this).addClass("mid");
        } else if (dataRating < 3.0) {
          $(this).addClass("low");
        }
      });
    }
    numericalRating(".numerical-rating");

    function starRating(ratingElem) {
      $(ratingElem).each(function () {
        var dataRating = $(this).attr("data-rating");

        function starsOutput(
          firstStar,
          secondStar,
          thirdStar,
          fourthStar,
          fifthStar
        ) {
          return (
            "" +
            '<span class="' +
            firstStar +
            '"></span>' +
            '<span class="' +
            secondStar +
            '"></span>' +
            '<span class="' +
            thirdStar +
            '"></span>' +
            '<span class="' +
            fourthStar +
            '"></span>' +
            '<span class="' +
            fifthStar +
            '"></span>'
          );
        }
        var fiveStars = starsOutput("star", "star", "star", "star", "star");
        var fourHalfStars = starsOutput(
          "star",
          "star",
          "star",
          "star",
          "star half"
        );
        var fourStars = starsOutput(
          "star",
          "star",
          "star",
          "star",
          "star empty"
        );
        var threeHalfStars = starsOutput(
          "star",
          "star",
          "star",
          "star half",
          "star empty"
        );
        var threeStars = starsOutput(
          "star",
          "star",
          "star",
          "star empty",
          "star empty"
        );
        var twoHalfStars = starsOutput(
          "star",
          "star",
          "star half",
          "star empty",
          "star empty"
        );
        var twoStars = starsOutput(
          "star",
          "star",
          "star empty",
          "star empty",
          "star empty"
        );
        var oneHalfStar = starsOutput(
          "star",
          "star half",
          "star empty",
          "star empty",
          "star empty"
        );
        var oneStar = starsOutput(
          "star",
          "star empty",
          "star empty",
          "star empty",
          "star empty"
        );
        if (dataRating >= 4.75) {
          $(this).append(fiveStars);
        } else if (dataRating >= 4.25) {
          $(this).append(fourHalfStars);
        } else if (dataRating >= 3.75) {
          $(this).append(fourStars);
        } else if (dataRating >= 3.25) {
          $(this).append(threeHalfStars);
        } else if (dataRating >= 2.75) {
          $(this).append(threeStars);
        } else if (dataRating >= 2.25) {
          $(this).append(twoHalfStars);
        } else if (dataRating >= 1.75) {
          $(this).append(twoStars);
        } else if (dataRating >= 1.25) {
          $(this).append(oneHalfStar);
        } else if (dataRating < 1.25) {
          $(this).append(oneStar);
        }
      });
    }
    starRating(".star-rating");

    var mapZoomAttr = $("#map-contact").attr("data-map-zoom");
    var mapScrollAttr = $("#map-contact").attr("data-map-scroll");
    if (typeof mapZoomAttr !== typeof undefined && mapZoomAttr !== false) {
      var zoomLevel = parseInt(mapZoomAttr);
    } else {
      var zoomLevel = 5;
    }
    if (typeof mapScrollAttr !== typeof undefined && mapScrollAttr !== false) {
      var scrollEnabled = parseInt(mapScrollAttr);
    } else {
      var scrollEnabled = false;
    }
    var map = new google.maps.Map(document.getElementById("map-contact"), {
      zoom: zoomLevel,
      scrollwheel: false,
      center: new google.maps.LatLng(40.709295, -74.003099),
      mapTypeId: google.maps.MapTypeId.ROADMAP,
      zoomControl: true,
      zoomControlOptions: {
        position: google.maps.ControlPosition.TOP_LEFT,
      },
      mapTypeControl: false,
      scaleControl: false,
      panControl: false,
      navigationControl: false,
      streetViewControl: false,
      gestureHandling: "cooperative",
      styles: [
        {
          featureType: "all",
          elementType: "geometry.fill",
          stylers: [
            {
              weight: "2.00",
            },
          ],
        },
        {
          featureType: "all",
          elementType: "geometry.stroke",
          stylers: [
            {
              color: "#9c9c9c",
            },
          ],
        },
        {
          featureType: "all",
          elementType: "labels.text",
          stylers: [
            {
              visibility: "on",
            },
          ],
        },
        {
          featureType: "landscape",
          elementType: "all",
          stylers: [
            {
              color: "#f2f2f2",
            },
          ],
        },
        {
          featureType: "landscape",
          elementType: "geometry.fill",
          stylers: [
            {
              color: "#ffffff",
            },
          ],
        },
        {
          featureType: "landscape.man_made",
          elementType: "geometry.fill",
          stylers: [
            {
              color: "#ffffff",
            },
          ],
        },
        {
          featureType: "poi",
          elementType: "all",
          stylers: [
            {
              visibility: "off",
            },
          ],
        },
        {
          featureType: "road",
          elementType: "all",
          stylers: [
            {
              saturation: -100,
            },
            {
              lightness: 45,
            },
          ],
        },
        {
          featureType: "road",
          elementType: "geometry.fill",
          stylers: [
            {
              color: "#eeeeee",
            },
          ],
        },
        {
          featureType: "road",
          elementType: "labels.text.fill",
          stylers: [
            {
              color: "#7b7b7b",
            },
          ],
        },
        {
          featureType: "road",
          elementType: "labels.text.stroke",
          stylers: [
            {
              color: "#ffffff",
            },
          ],
        },
        {
          featureType: "road.highway",
          elementType: "all",
          stylers: [
            {
              visibility: "simplified",
            },
          ],
        },
        {
          featureType: "road.arterial",
          elementType: "labels.icon",
          stylers: [
            {
              visibility: "off",
            },
          ],
        },
        {
          featureType: "transit",
          elementType: "all",
          stylers: [
            {
              visibility: "off",
            },
          ],
        },
        {
          featureType: "water",
          elementType: "all",
          stylers: [
            {
              color: "#46bcec",
            },
            {
              visibility: "on",
            },
          ],
        },
        {
          featureType: "water",
          elementType: "geometry.fill",
          stylers: [
            {
              color: "#c8d7d4",
            },
          ],
        },
        {
          featureType: "water",
          elementType: "labels.text.fill",
          stylers: [
            {
              color: "#070707",
            },
          ],
        },
        {
          featureType: "water",
          elementType: "labels.text.stroke",
          stylers: [
            {
              color: "#ffffff",
            },
          ],
        },
      ],
    });

    $(".listing-item-container").on("mouseover", function () {
      var listingAttr = $(this).data("marker-id");
      if (listingAttr !== undefined) {
        var listing_id = $(this).data("marker-id") - 1;
        var marker_div = allMarkers[listing_id].div;
        $(marker_div).addClass("clicked");
        $(this).on("mouseout", function () {
          if ($(marker_div).is(":not(.infoBox-opened)")) {
            $(marker_div).removeClass("clicked");
          }
        });
      }
    });

    var boxText = document.createElement("div");
    boxText.className = "map-box";
    var currentInfobox;
    var boxOptions = {
      content: boxText,
      disableAutoPan: false,
      alignBottom: true,
      maxWidth: 0,
      pixelOffset: new google.maps.Size(-134, -55),
      zIndex: null,
      boxStyle: { width: "360px" },
      closeBoxMargin: "0",
      closeBoxURL: "",
      infoBoxClearance: new google.maps.Size(25, 25),
      isHidden: false,
      pane: "floatPane",
      enableEventPropagation: false,
    };
    var markerCluster, overlay, i;
    var allMarkers = [];
    var clusterStyles = [
      { textColor: "white", url: "", height: 50, width: 50 },
    ];
    var markerIco;
    for (i = 0; i < locations.length; i++) {
      markerIco = locations[i][4];
      var overlaypositions = new google.maps.LatLng(
          locations[i][1],
          locations[i][2]
        ),
        overlay = new CustomMarker(
          overlaypositions,
          map,
          { marker_id: i },
          markerIco
        );
      allMarkers.push(overlay);
      google.maps.event.addDomListener(
        overlay,
        "click",
        (function (overlay, i) {
          return function () {
            ib.setOptions(boxOptions);
            boxText.innerHTML = locations[i][0];
            ib.close();
            ib.open(map, overlay);
            currentInfobox = locations[i][3];
            google.maps.event.addListener(ib, "domready", function () {
              $(".infoBox-close").click(function (e) {
                e.preventDefault();
                ib.close();
                $(".map-marker-container").removeClass(
                  "clicked infoBox-opened"
                );
              });
            });
          };
        })(overlay, i)
      );
    }
    var options = {
      imagePath: "images/",
      styles: clusterStyles,
      minClusterSize: 2,
    };
    markerCluster = new MarkerClusterer(map, allMarkers, options);
    google.maps.event.addDomListener(window, "resize", function () {
      var center = map.getCenter();
      google.maps.event.trigger(map, "resize");
      map.setCenter(center);
    });
    var zoomControlDiv = document.createElement("div");
    var zoomControl = new ZoomControl(zoomControlDiv, map);
    function ZoomControl(controlDiv, map) {
      zoomControlDiv.index = 1;
      map.controls[google.maps.ControlPosition.RIGHT_CENTER].push(
        zoomControlDiv
      );
      controlDiv.style.padding = "5px";
      controlDiv.className = "zoomControlWrapper";
      var controlWrapper = document.createElement("div");
      controlDiv.appendChild(controlWrapper);
      var zoomInButton = document.createElement("div");
      zoomInButton.className = "custom-zoom-in";
      controlWrapper.appendChild(zoomInButton);
      var zoomOutButton = document.createElement("div");
      zoomOutButton.className = "custom-zoom-out";
      controlWrapper.appendChild(zoomOutButton);
      google.maps.event.addDomListener(zoomInButton, "click", function () {
        map.setZoom(map.getZoom() + 1);
      });
      google.maps.event.addDomListener(zoomOutButton, "click", function () {
        map.setZoom(map.getZoom() - 1);
      });
    }
    var scrollEnabling = $("#scrollEnabling");
    $(scrollEnabling).click(function (e) {
      e.preventDefault();
      $(this).toggleClass("enabled");
      if ($(this).is(".enabled")) {
        map.setOptions({ scrollwheel: true });
      } else {
        map.setOptions({ scrollwheel: false });
      }
    });
    $("#geoLocation, .input-with-icon.location a").click(function (e) {
      e.preventDefault();
      geolocate();
    });

    function geolocate() {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
          var pos = new google.maps.LatLng(
            position.coords.latitude,
            position.coords.longitude
          );
          map.setCenter(pos);
          map.setZoom(12);
        });
      }
    }
  }
  var map = document.getElementById("map-contact");
  if (typeof map != "undefined" && map != null) {
    google.maps.event.addDomListener(window, "load", mainMap);
  }

  function CustomMarker(latlng, map, args, markerIco) {
    this.latlng = latlng;
    this.args = args;
    this.markerIco = markerIco;
    this.setMap(map);
  }
  CustomMarker.prototype = new google.maps.OverlayView();
  CustomMarker.prototype.draw = function () {
    var self = this;
    var div = this.div;
    if (!div) {
      div = this.div = document.createElement("div");
      div.className = "map-marker-container";
      div.innerHTML =
        '<div class="marker-container">' +
        '<div class="marker-card">' +
        '<div class="front face">' +
        self.markerIco +
        "</div>" +
        '<div class="back face">' +
        self.markerIco +
        "</div>" +
        '<div class="marker-arrow"></div>' +
        "</div>" +
        "</div>";
      google.maps.event.addDomListener(div, "click", function (event) {
        $(".map-marker-container").removeClass("clicked infoBox-opened");
        google.maps.event.trigger(self, "click");
        $(this).addClass("clicked infoBox-opened");
      });
      if (typeof self.args.marker_id !== "undefined") {
        div.dataset.marker_id = self.args.marker_id;
      }
      var panes = this.getPanes();
      panes.overlayImage.appendChild(div);
    }
    var point = this.getProjection().fromLatLngToDivPixel(this.latlng);
    if (point) {
      div.style.left = point.x + "px";
      div.style.top = point.y + "px";
    }
  };
  CustomMarker.prototype.remove = function () {
    if (this.div) {
      this.div.parentNode.removeChild(this.div);
      this.div = null;
      $(this).removeClass("clicked");
    }
  };
  CustomMarker.prototype.getPosition = function () {
    return this.latlng;
  };
})(this.jQuery);
