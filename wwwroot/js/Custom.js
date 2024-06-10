$(document).ready(function () {

    var previousQ;

    //al change degli Quarti, abilito la squadra selezionata
    jQuery(".Quarti").on('focus', function () {
        previousQ = this.value;
    }).change(function () {
        var selectedTeam = jQuery(this).val();

        jQuery(".Quarti").each(function () {
            $(this).children('option[value="' + previousQ + '"]').removeAttr('disabled');
            if (jQuery(this).val() != selectedTeam) {
                $(this).children('option[value="' + selectedTeam + '"]').attr('disabled', 'disabled');
            }
        });

        //disabilito le option dei Semifinali
        jQuery('.Semifinali option').each(function () {
            jQuery(this).attr('disabled', 'disabled');
        });

        jQuery(".Quarti").each(function () {
            var currentTeam = jQuery(this).val();
            if (currentTeam != '') {
                jQuery('.Semifinali option[value="' + currentTeam + '"]').removeAttr('disabled');
            }
        });

        //pulizia delle vecchie selezioni che sono ancora disabled
        jQuery('.Semifinali').each(function () {
            if (jQuery(this).val() == null)
                jQuery(this).val('');
        });
        $(".Semifinali").trigger('change');
    });

    var previousS;

    //al change degli ottavi, abilito la squadra selezionata
    jQuery(".Semifinali").on('focus', function () {
        previousS = this.value;
    }).change(function () {
        var selectedTeam = jQuery(this).val();

        jQuery(".Semifinali").each(function () {
            $(this).children('option[value="' + previousS + '"]').removeAttr('disabled');
            if (jQuery(this).val() != selectedTeam) {
                $(this).children('option[value="' + selectedTeam + '"]').attr('disabled', 'disabled');
            }
        });

        //disabilito le option dei quarti
        jQuery('.Finale option').each(function () {
            jQuery(this).attr('disabled', 'disabled');
        });

        jQuery(".Semifinali").each(function () {
            var currentTeam = jQuery(this).val();
            if (currentTeam != '') {
                jQuery('.Finale option[value="' + currentTeam + '"]').removeAttr('disabled');
            }
        });

        //pulizia delle vecchie selezioni che sono ancora disabled
        jQuery('.Finale').each(function () {
            if (jQuery(this).val() == null)
                jQuery(this).val('');
        });
        $(".Finale").trigger('change');
    });

    var previousF;

    jQuery(".Finale").on('focus', function () {
        previousF = this.value;
    }).change(function () {
        var selectedTeam = jQuery(this).val();

        jQuery(".Finale").each(function () {
            $(this).children('option[value="' + previousF + '"]').removeAttr('disabled');
            if (jQuery(this).val() != selectedTeam) {
                $(this).children('option[value="' + selectedTeam + '"]').attr('disabled', 'disabled');
            }
        });

        //disabilito le option
        jQuery('.Win option').each(function () {
            jQuery(this).attr('disabled', 'disabled');
        });

        jQuery(".Finale").each(function () {
            var currentTeam = jQuery(this).val();
            if (currentTeam != '') {
                jQuery('.Win option[value="' + currentTeam + '"]').removeAttr('disabled');
            }
        });

        //pulizia delle vecchie selezioni che sono ancora disabled
        jQuery('.Win').each(function () {
            if (jQuery(this).val() == null)
                jQuery(this).val('');
        });

    });
    //$(".Finale").trigger('change');
    $(".Quarti").trigger('change');

})

//$(document).ready(function() {
//    formmodified=0;
//    $('form *').change(function(){
//        formmodified=1;
//    });
//    window.onbeforeunload = confirmExit;
//    function confirmExit() {
//        if (formmodified == 1) {
//            //return "New information not saved. Do you wish to leave the page?";
//            return 'You haven\'t saved your changes.';
//        }
//    }
//    $("input[name='commit']").click(function() {
//        formmodified = 0;
//    });
//});

$('#MyBet').submit(function () {
    $('select').removeAttr('disabled');
});

//$('form *').data('serialize', $('form *').serialize()); // On load save form current state

//$(window).bind('beforeunload', function (e) {
//    if ($('form *').serialize() != $('form *').data('serialize')) return true;
//    else e = null; // i.e; if form state change show box not.
//});
