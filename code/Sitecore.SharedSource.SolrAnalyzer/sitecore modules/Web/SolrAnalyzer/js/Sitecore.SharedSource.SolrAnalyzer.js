jQuery.noConflict();
jQuery(document).ready(function () {

    var config = {
        '.chosen-select': { width: "100%" }
    }
    var results = [];
    for (var selector in config) {
        var elements = $$(selector);
        for (var i = 0; i < elements.length; i++) {
            results.push(new Chosen(elements[i], config[selector]));
        }
    }

    jQuery(".slider-range").each(function () {
        jQuery(this).slider({
            range: true,
            min: 0,
            max: 100,
            values: [0, 100],
            slide: function (event, ui) {
                var parent = jQuery(this).parent('.emotion-item');
                var rangeVal = ui.values[0] + " - " + ui.values[1] + "%";
                var filterValue = jQuery(parent).find(".filter-value");
                filterValue.html(rangeVal);
                filterValue.data('min', ui.values[0]);
                filterValue.data('max', ui.values[1]);
            }
        });

        var parent = jQuery(this).parent('.emotion-item');
        var filterValue = jQuery(parent).find(".filter-value");
        filterValue.html("0 - 100%");
        filterValue.data('min', 0);
        filterValue.data('max', 100);
    });

    //toggles tabs based on nav clicks
    jQuery(".nav-btn")
        .click(function () {
            var selected = "selected";
            var tab = jQuery(this).attr("rel");
            jQuery(".nav-btn").removeClass(selected);
            jQuery(".tab-content").removeClass(selected);
            jQuery(".tab-content." + tab).addClass(selected);
            jQuery(this).addClass(selected);
        });

    //handles reanalyze all form
    var reanlyzeAllForm = ".reanalyze-all-form";
    jQuery(reanlyzeAllForm + " button")
        .click(function(event) {
            event.preventDefault();
            
            var idValue = jQuery(reanlyzeAllForm + " #id").attr("value");
            var langValue = jQuery(reanlyzeAllForm + " #language").attr("value");
            var dbValue = jQuery(reanlyzeAllForm + " #database").attr("value");
            
            jQuery(".form").hide();
            jQuery(".progress-indicator").show();

            jQuery.post(
                jQuery(reanlyzeAllForm).attr("action"),
                {
                    id: idValue,
                    language: langValue,
                    db: dbValue
                }
            ).done(function (r) {
                jQuery(".result-count").text(r.ItemCount);
                jQuery(".progress-indicator").hide();
                jQuery(".result-display").show();
            });
        });

    //handles the submit on the set all alts form
    var setAltsAllForm = ".set-alt-all-form";
    jQuery(setAltsAllForm + " button")
        .click(function (event) {
            event.preventDefault();

            var idValue = jQuery(setAltsAllForm + " #id").attr("value");
            var langValue = jQuery(setAltsAllForm + " #language").attr("value");
            var dbValue = jQuery(setAltsAllForm + " #database").attr("value");
            var thresholdValue = jQuery(setAltsAllForm + " #threshold").val();
            var overwriteValue = jQuery(setAltsAllForm + " #overwrite").is(':checked');

            jQuery(".form").hide();
            jQuery(".progress-indicator").show();

            jQuery.post(
                jQuery(setAltsAllForm).attr("action"),
                {
                    id: idValue,
                    language: langValue,
                    db: dbValue,
                    threshold: thresholdValue,
                    overwrite: overwriteValue
                }
            ).done(function (r) {
                jQuery(".result-modified").text(r.ItemsModified);
                jQuery(".result-count").text(r.ItemCount);
                jQuery(".progress-indicator").hide();
                jQuery(".result-display").show();
            }).always(function () {
                jQuery(".progress-indicator").hide();
            });
        });

    //closes modal and send selected image back to RTE
    var queryObj;
    var imageSearchForm = ".image-search-form";
    jQuery(imageSearchForm + " .form-submit")
        .click(function (event) {
            event.preventDefault();

            var img = jQuery(".result-items .selected");
            if (img.length)
                CloseRadWindow(jQuery(".result-items .selected").html());
            else
                alert("You need to select an image.");
        });

    //closes modal on cancel press
    jQuery(imageSearchForm + " .form-cancel")
        .click(function (event) {
            event.preventDefault();

            CloseRadWindow();
        });

    //will perform search if you type and wait
    var imageSearchInput = ".rte-search-input";
    jQuery(imageSearchInput)
        .on('input', function (e) {
            clearTimeout(queryObj);
            queryObj = setTimeout(RunQuery, 500);
        });

    //performs search on 'enter-press' on the form
    jQuery(imageSearchForm + " .cognitiveSearchButton")
        .click(function (event) {
            event.preventDefault();

            clearTimeout(queryObj);
            RunQuery();
        });

    //performs image search
    function RunQuery() {
        var langValue = jQuery(imageSearchForm + " #language").attr("value");
        var dbValue = jQuery(imageSearchForm + " #database").attr("value");
        var gender = jQuery(imageSearchForm + " #gender").val();
        var glasses = jQuery(imageSearchForm + " #glasses").val();

        
        jQuery(imageSearchForm + " .progress-indicator").show();
        jQuery(".search-results").hide();

        var tags = GetTagParameters();
        var ranges = GetRangeParameters();

        jQuery.post(
            jQuery(imageSearchForm).attr("action"),
                {
                    formParameters: [],
                    tagParameters: tags,
                    rangeParameters: ranges,
                    gender: gender,
                    glasses: glasses,
                    language: langValue,
                    db: dbValue
                }
            ).done(function (r) {
                jQuery(".result-count").text(r.ResultCount);
                jQuery(".result-items").text("");
                jQuery(".search-results").show();
                for (var i = 0; i < r.Results.length; i++) {
                    var d = r.Results[i];
                    if (d.url != undefined) {
                        jQuery(".result-items").append("<div class='result-img-wrap'><img src=\"" + d.url + "\" alt=\"" + d.alt + "\" /></div>");
                    }
                }

                jQuery(".result-img-wrap")
                    .on("click", function () {
                        jQuery(".result-items .selected").removeClass("selected");
                        jQuery(this).addClass("selected");
                    });
            }).always(function () {
                jQuery(imageSearchForm + " .progress-indicator").hide();
            });
    }

    function GetRangeParameters() {
        var params = [];

        //tags
        var filterElements = jQuery('.filter-value');
        if (filterElements.length > 0) {

            filterElements.each(function () {
                var values = [];
                values.push(jQuery(this).data('min'));
                values.push(jQuery(this).data('max'));

                params.push({
                    key: jQuery(this).data('field'),
                    value: values
                });
            });
        }

        return params;
    }

    function GetTagParameters() {
        var params = [];
        params.push({
            key: "tags",
            value: [""]
        });

        //tags
        var tagElements = jQuery('.search-choice span');
        if (tagElements.length > 0) {

            var values = [];

            tagElements.each(function () {
                var text = jQuery(this).text();
                text = text.substring(0, text.indexOf(' ('));
                text = text.toLowerCase();
                values.push(text);
            });

            params.push({
                key: "tags",
                value: values
            });
        }

        return params;
    }

    //get results for the first load
    RunQuery();

    $(function () {
        function split(val) {
            return val.split(/,\s*/);
        }
        function extractLast(term) {
            return split(term).pop();
        }

        $(imageSearchInput)
          // don't navigate away from the field on tab when selecting an item
          .on("keydown", function (event) {
              if (event.keyCode === $.ui.keyCode.TAB &&
                  $(this).autocomplete("instance").menu.active) {
                  event.preventDefault();
              }
          })
          .autocomplete({
              source: function (request, response) {
                  $.getJSON("search.php", {
                      term: extractLast(request.term)
                  }, response);
              },
              search: function () {
                  // custom minLength
                  var term = extractLast(this.value);
                  if (term.length < 2) {
                      return false;
                  }
              },
              focus: function () {
                  // prevent value inserted on focus
                  return false;
              },
              select: function (event, ui) {
                  var terms = split(this.value);
                  // remove the current input
                  terms.pop();
                  // add the selected item
                  terms.push(ui.item.value);
                  // add placeholder to get the comma-and-space at the end
                  terms.push("");
                  this.value = terms.join(", ");
                  return false;
              }
          });
    });
});

//closes the search modal and passes value back to the RTE
function CloseRadWindow(value) {

    var radWindow;

    if (window.radWindow)
        radWindow = window.radWindow;
    else if (window.frameElement && window.frameElement.radWindow)
        radWindow = window.frameElement.radWindow;
    else
        window.close();

    radWindow.Close(value);
}