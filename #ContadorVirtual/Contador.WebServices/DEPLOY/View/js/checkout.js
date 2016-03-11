Iugu.setAccountID("01f7901d-92a9-4e18-b701-03c957928120");
Iugu.setTestMode(true); // Retirar esta linha para produção

jQuery(function ($) {
    $('#payment-form').submit(function (evt) {
        if ($('#credit-card-checkbox').attr('checked')) {
            var form = $(this);
            var tokenResponseHandler = function (data) {

                if (data.errors) {
                    alert("Erro salvando cartão: " + JSON.stringify(data.errors));
                } else {
                    $("#token").val(data.id);
                    form.get(0).submit();
                }
            }

            Iugu.createPaymentToken(this, tokenResponseHandler);
            return false;
        } else {
            form.get(0).submit();
        }
    });

    cc_checkbox = $('#credit-card-checkbox')
    cc_checkbox_label = $('#credit-card-checkbox-label')
    bs_checkbox = $('#bank-slip-checkbox')
    bs_checkbox_label = $('#bank-slip-checkbox-label')
    credit_card_form = $('.usable-creditcard-form')

    cc_checkbox.click(function () {
        bs_checkbox_label.removeClass("active");
        cc_checkbox_label.addClass("active");
        bs_checkbox.removeAttr("checked");
        cc_checkbox.attr("checked", "checked");
        credit_card_form.show();
    });

    bs_checkbox.click(function () {
        cc_checkbox_label.removeClass("active");
        bs_checkbox_label.addClass("active");
        cc_checkbox.removeAttr("checked");
        bs_checkbox.attr("checked", "checked");
        credit_card_form.hide();
    });
});