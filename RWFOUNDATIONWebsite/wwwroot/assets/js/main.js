!(function($) {
  "use strict";

  // Smooth scroll for the navigation menu and links with .scrollto classes
  $(document).on("click", ".nav-menu a, .mobile-nav a, .scrollto", function(e) {
    if (
      location.pathname.replace(/^\//, "") ==
        this.pathname.replace(/^\//, "") &&
      location.hostname == this.hostname
    ) {
      e.preventDefault();
      var target = $(this.hash);
      if (target.length) {
        var scrollto = target.offset().top;
          var scrolled = 20;         

        if ($("#header").length) {
          scrollto -= $("#header").outerHeight();            
          if (!$("#header").hasClass("header-scrolled")) {
              scrollto += scrolled;              
          }
        }

        if ($(this).attr("href") == "#header") {
          scrollto = 0;
        }

        $("html, body").animate(
          {
            scrollTop: scrollto
          },
          1500,
          "easeInOutExpo"
        );

        if ($(this).parents(".nav-menu, .mobile-nav").length) {
          $(".nav-menu .active, .mobile-nav .active").removeClass("active");
          $(this)
            .closest("li")
            .addClass("active");
        }

        if ($("body").hasClass("mobile-nav-active")) {
          $("body").removeClass("mobile-nav-active");
          $(".mobile-nav-toggle i").toggleClass(
            "icofont-navigation-menu icofont-close"
            );           
          $(".mobile-nav-overly").fadeOut();
        }
        return false;
      }
    }
  });

  // Mobile Navigation
  if ($(".nav-menu").length) {
    var $mobile_nav = $(".nav-menu")
      .clone()
      .prop({
        class: "mobile-nav d-lg-none"
      });
    $("body").append($mobile_nav);
    $("body").prepend(
      '<button type="button" class="mobile-nav-toggle d-lg-none"><i class="icofont-navigation-menu"></i></button>'
    );
    $("body").append('<div class="mobile-nav-overly"></div>');

    $(document).on("click", ".mobile-nav-toggle", function(e) {
      $("body").toggleClass("mobile-nav-active");
      $(".mobile-nav-toggle i").toggleClass(
        "icofont-navigation-menu icofont-close"
      );
      $(".mobile-nav-overly").toggle();
    });

    $(document).on("click", ".mobile-nav .drop-down > a", function(e) {
      e.preventDefault();
      $(this)
        .next()
        .slideToggle(300);
      $(this)
        .parent()
        .toggleClass("active");
    });

    $(document).click(function(e) {
      var container = $(".mobile-nav, .mobile-nav-toggle");
      if (!container.is(e.target) && container.has(e.target).length === 0) {
        if ($("body").hasClass("mobile-nav-active")) {
          $("body").removeClass("mobile-nav-active");
          $(".mobile-nav-toggle i").toggleClass(
            "icofont-navigation-menu icofont-close"
          );
          $(".mobile-nav-overly").fadeOut();
        }
      }
    });
  } else if ($(".mobile-nav, .mobile-nav-toggle").length) {
    $(".mobile-nav, .mobile-nav-toggle").hide();
  }
  // Toggle .header-scrolled class to #header when page is scrolled
  $(window).scroll(function() {
    if ($(this).scrollTop() > 100) {
      $("#header").addClass("header-scrolled");
    } else {
      $("#header").removeClass("header-scrolled");
    }
  });

  if ($(window).scrollTop() > 100) {
    $("#header").addClass("header-scrolled");
  }

  // Stick the header at top on scroll
  $("#header").sticky({
    topSpacing: 0,
    zIndex: "50"
  });

  // Real view height for mobile devices
  if (window.matchMedia("(max-width: 767px)").matches) {
    $("#hero").css({
      height: $(window).height()
    });
  }

  // Intro carousel
  var heroCarousel = $("#heroCarousel");
  var heroCarouselIndicators = $("#hero-carousel-indicators");
  heroCarousel
    .find(".carousel-inner")
    .children(".carousel-item")
    .each(function(index) {
      index === 0
        ? heroCarouselIndicators.append(
            "<li data-target='#heroCarousel' data-slide-to='" +
              index +
              "' class='active'></li>"
          )
        : heroCarouselIndicators.append(
            "<li data-target='#heroCarousel' data-slide-to='" +
              index +
              "'></li>"
          );
    });

  heroCarousel.on("slid.bs.carousel", function(e) {
    $(this)
      .find(".carousel-content ")
      .addClass("animated fadeInDown");
  });

  // Back to top button
  $(window).scroll(function() {
    if ($(this).scrollTop() > 100) {
      $(".back-to-top").fadeIn("slow");
    } else {
      $(".back-to-top").fadeOut("slow");
    }
  });

  $(".counter").counterUp({
    delay: 15,
    time: 2000
  });

  $(".back-to-top").click(function() {
    $("html, body").animate(
      {
        scrollTop: 0
      },
      1500,
      "easeInOutExpo"
    );
    return false;
  });

  // Porfolio isotope and filter
  $(window).on("load", function() {
    var portfolioIsotope = $(".portfolio-container").isotope({
      itemSelector: ".portfolio-item",
      layoutMode: "fitRows"
    });

    $("#portfolio-flters li").on("click", function() {
      $("#portfolio-flters li").removeClass("filter-active");
      $(this).addClass("filter-active");

      portfolioIsotope.isotope({
        filter: $(this).data("filter")
      });
    });

    // Initiate venobox (lightbox feature used in portofilo)
    $(document).ready(function() {
      $(".venobox").venobox();
    });
  });

  // Skills section
  $(".skills-content").waypoint(
    function() {
      $(".progress .progress-bar").each(function() {
        $(this).css("width", $(this).attr("aria-valuenow") + "%");
      });
    },
    {
      offset: "80%"
    }
  );

  // Portfolio details carousel
  $(".portfolio-details-carousel").owlCarousel({
    autoplay: true,
    dots: true,
    loop: true,
    items: 1
  });

  // Initi AOS
  AOS.init({
    duration: 800,
    easing: "ease-in-out"
  });

  //forms validations
  var input = $(".validate-input .input100");

  $(".validate-form").on("submit", function() {
    var check = true;

    for (var i = 0; i < input.length; i++) {
      if (validate(input[i]) == false) {
        showValidate(input[i]);
        check = false;
      }
    }

    return check;
  });

  $(".validate-form .input100").each(function() {
    $(this).focus(function() {
      hideValidate(this);
    });
  });

  function validate(input) {
    if ($(input).attr("type") == "email" || $(input).attr("name") == "email") {
      if (
        $(input)
          .val()
          .trim()
          .match(
            /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/
          ) == null
      ) {
        return false;
      }
    } else {
      if (
        $(input)
          .val()
          .trim() == ""
      ) {
        return false;
      }
    }
  }

  function showValidate(input) {
    var thisAlert = $(input).parent();

    $(thisAlert).addClass("alert-validate");
  }

  function hideValidate(input) {
    var thisAlert = $(input).parent();

    $(thisAlert).removeClass("alert-validate");
  }
})(jQuery);

function sendMessage() {
  var name = $("#namevalue").val();
  var email = $("#emailvalue").val();
  var subject = $("#subjectvalue").val();
  var message = $("#messagevalue").val();
  var html = "";
  html +=
    '<h2 style="color:Green; text-align:center">' +
    name +
    "</h2>" +
    '<p style="color:Red; text-align:center">' +
    email +
    "</p>" +
    '<p style="color:Black; text-align:center">' +
    message +
    "</p>";

    if (!name || !email || !subject || !message) {
        alertify.er("Please Enter your Credentials First");
  } else {
    Email.send({     
        SecureToken: "1ec96712-d41d-46f2-8023-a42be3f511bc",     
        To: "info@rwfoundation.org",
        From: "web@rwfoundation.org",       
        Subject: subject,
        Body: html
    }).then(message => alertify.success("mail sent successfully"));
    $("#namevalue").val("");
    $("#emailvalue").val("");
    $("#subjectvalue").val("");
    $("#messagevalue").val("");
    $("#feedback").removeClass("d-none");
  }
}
function SendDonations() {
    var firstname = $("#firstnamedonate").val();
    var lastname = $("#lastnamedonate").val();
    var phonenumber = $('#phonedonate').val();
    var subject = "Nku-RW Foundation Online Donation Information Received";
    var email = $("#emaildonate").val();
    var amount = $("#donationAmount").val();
    var amountrs = "Rs." + amount;
    var html = "";    
    if (!firstname) {
        $("#firstnamedonate").focus();
        alertify.error("Please Enter your First Name");
    }
    else if (!lastname) {
        $("#lastnamedonate").focus();
        alertify.error("Please Enter your Last Name");

    } else if (!phonenumber) {
        $("#phonedonate").focus();
        alertify.error("Please Enter your Phone Number");

    } else if (!amount) {
        $("#donationAmount").focus();
        alertify.error("Please Enter  Amount");

    } else if (!email) {
        $("#emaildonate").focus();
        alertify.error("Please Enter your valid Email");

    }else {
        $("#loading").removeClass('d-none');
        var data = {
            FirstName: firstname,
            LastName: lastname,
            PhoneNumber: phonenumber,
            Email: email,
            PledgedAmount: amount
        };
        $.ajax({
            url: "/Donation/AddDonation",
            type: 'POST',
            datatype: "json",
            data: { model: data },
            success: function (response) {
                if (response.isError !== true) {
                    html +=
                        '<div>' +
                        '<h3 style="color:Black; text-align:left">' +
                        'Dear Accounts Department,' +
                        '</h3>' +
                        '<p style:"color:Black; text-align:left;">' +
                        'Following donation & donor information has been received through our website rwfoundation.org application built by NKU Tech:' +
                        '</p>' +
                        '<h5 style:"color:Black; text-align:left;">' +
                        'Donor #:' + response.message +
                        '</h5>' +
                        '<table style="font-family: Trebuchet MS, Arial, Helvetica, sans-serif;border-collapse: collapse;width: 50%; margin-left: auto; margin-right: auto;">' +
                        '<thead>' +
                        '<tr>' +
                        '<th style="padding-top: 20px;padding-bottom: 20px;text-align: left;background-color: #f8be14;color: #fff; border: 1px solid #ddd;padding: 8px;">' +
                        'Description' +
                        '</th>' +
                        '<th style="padding-top: 20px;padding-bottom: 20px;text-align: left;background-color: #f8be14;color: #fff; border: 1px solid #ddd;padding: 8px;">' +
                        'Particulars' +
                        '</th>' +
                        '</tr>' +
                        '</thead>' +
                        '<tbody>' +
                        '<tr>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        'Donor Name:' +
                        '</td>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        firstname + ' ' + lastname +
                        '</td>' +
                        '</tr>' +
                        '<tr>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        'Donor email ID:' +
                        '</td>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        email +
                        '</td>' +
                        '</tr>' +
                        '<tr>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        'Donor Phone #:' +
                        '</td>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        phonenumber +
                        '</td>' +
                        '</tr>' +
                        '<tr>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        'Amount Pledged:' +
                        '</td>' +
                        '<td style=" border: 1px solid #ddd;padding: 8px;">' +
                        amountrs +
                        '</td>' +
                        '</tr>' +
                        '</tbody>' +
                        '</table>' +
                        '<div style="padding:20px; color:Black; background-color:lightgray;margin-top: 50px;">' +
                        '<h4>' +
                        'Disclaimer:' +
                        '</h4>' +
                        'This information has been passed through our website rwfoundation.org on as it is basis, without any human intervention.So Keep this information confidential.' +
                        '<p>' +
                        'NKU Technologies Pvt Ltd is not liable or responsible for any breach or leakage of information of confidential data after this email you have received.' +
                        '</p>' +
                        '</div>' +
                        '</div>';

                    Email.send({
                        SecureToken: "1ec96712-d41d-46f2-8023-a42be3f511bc",
                        To: "donate@rwfoundation.org",
                        From: "web@rwfoundation.org",                       
                        Subject: subject,
                        Body: html
                    }).then(message => alertify.success("Offline Donation received successfully"));
                    $("#firstnamedonate").val("");
                    $("#lastnamedonate").val("");
                    $("#emaildonate").val("");
                    $("#phonedonate").val(""); 
                    $("#feedback").removeClass("d-none");
                    $("#loading").addClass('d-none');
                }
                else {
                    alertify.error(response.message);
                }
            },
            error: function (response) {
                alertify.error(response.message);
            }
        });

    }    
   
}