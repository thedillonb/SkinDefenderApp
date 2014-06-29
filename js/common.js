/*
 * Petrichore
 * (c) 2013, Web factory Ltd
 */

$(function() {
  // flex slider
  if ($('.flexslider').length) {
    $('.flexslider').flexslider({
      animation: "fade",
      directionNav: true,
      controlNav: false,
      pauseOnAction: true,
      pauseOnHover: true,
      direction: "horizontal",
      slideshowSpeed: 5500
    });
  }
  
  // lightbox gallery
  $("a[data-gal^='prettyPhoto']").prettyPhoto({ social_tools: '' });

  // blockquote rotator
  $('section blockquote').quovolver(500, 6000);

  // Twitter feed
  if ($('#tweet').length) {
    $('#tweet').tweet({
            username: 'envato',
            join_text: 'auto',
            avatar_size: 0,
            count:1,
            auto_join_text_default: ' we said, ',
            auto_join_text_ed: ' we ',
            auto_join_text_ing: ' we were ',
            auto_join_text_reply: ' we replied to ',
            auto_join_text_url: ' we were checking out ',
            loading_text: 'Loading tweets...'
        });
  };

  // main dropdown menu
  $('ul#main-navigation li').hover(function(){
      $(this).children('ul').delay(20).fadeIn(200);
    }, function(){
      $(this).children('ul').delay(20).fadeOut(200);
  });
  
  // generate mobile menu
  if ($('#main-menu-select').length && $('#main-menu-select').attr('data-autogenerate') == 'true') {
    var mobile_menu = $('#main-menu-select');
    $('#main-navigation li a').each(function(index, elem) {
      mobile_menu.append($('<option></option>').val($(elem).attr('href')).html($(elem).html()));
    });
  }
  
  // mobile menu click
  $('#main-menu-select').change(function() {
    link = $(this).val();
    if (!link) {
      return;
    }
    
    document.location.href = link;
  });

  // init newsletter subscription AJAX handling
  if ($('#newsletterform').length) {
    if ($('#newsletterform').attr('data-mailchimp') == 'true') {
      $('#newsletterform').attr('action', '_newsletter-subscribe-mailchimp.php');
      $('#newsletterform').ajaxForm({ dataType: 'json',
                                      timeout: 2000,
                                      success: newsletterResponseMailchimp});
    } else {
      $('#newsletterform').attr('action', '_newsletter-subscribe.php');
      $('#newsletterform').ajaxForm({ dataType: 'json',
                                      timeout: 2000,
                                      success: newsletterResponse});
    }
    $('#button-newsletter').click(function() { $('#newsletterform').submit(); return false; });
  } // if newsletter form
}); // onload

// Facebook like crap
(function(d, s, id) {
  var js, fjs = d.getElementsByTagName(s)[0];
  if (d.getElementById(id)) return;
  js = d.createElement(s); js.id = id;
  js.src = "//connect.facebook.net/en_GB/all.js#xfbml=1&appId=152938731463199";
  fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

// Twitter count crap
!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src="//platform.twitter.com/widgets.js";fjs.parentNode.insertBefore(js,fjs);}}(document,"script","twitter-wjs");

// handle newsletter subscribe AJAX response
function newsletterResponse(response) {
  if (response.responseStatus == 'err') {
    if (response.responseMsg == 'ajax') {
      alert('Error - this script can only be invoked via an AJAX call.');
    } else if (response.responseMsg == 'fileopen') {
      alert('Error opening $emailsFile. Please refer to documentation for help.');
    } else if (response.responseMsg == 'name') {
      alert('Please enter a valid name.');
    } else if (response.responseMsg == 'email') {
      alert('Please enter a valid email address.');
    } else if (response.responseMsg == 'duplicate') {
      alert('You are already subscribed to our newsletter.');
    } else if (response.responseMsg == 'filewrite') {
      alert('Error writing to $emailsFile. Please refer to documentation for help.');
    } else {
      alert('Undocumented error. Please refresh the page and try again.');
    }
  } else if (response.responseStatus == 'ok') {
    alert('Thank you for subscribing to our newsletter! We will not abuse your address.');
  } else {
    alert('Undocumented error. Please refresh the page and try again.');
  }
} // newsletterResponse

// handle newsletter subscribe AJAX response - Mailchimp ver
function newsletterResponseMailchimp(response) {
  if (response.responseStatus == 'err') {
    if (response.responseMsg == 'ajax') {
      alert('Error - this script can only be invoked via an AJAX call.');
    } else if (response.responseMsg == 'name') {
      alert('Please enter a valid name.');
    } else if (response.responseMsg == 'email') {
      alert('Please enter a valid email address.');
    } else if (response.responseMsg == 'listid') {
      alert('Invalid MailChimp list name.');
    } else if (response.responseMsg == 'duplicate') {
      alert('You are already subscribed to our newsletter.');
    } else {
      alert('Undocumented error (' + response.responseMsg + '). Please refresh the page and try again.');
    }
  } else if (response.responseStatus == 'ok') {
    alert('Thank you for subscribing! Please confirm your subscription in the email you\'ll receive shortly.');
  } else {
    alert('Undocumented error. Please refresh the page and try again.');
  }
} // newsletterResponseMailchimp