$(function() {
    
    $('.radio-pick input').change(function() {
        $('.radio-pick').each(function() {
            if ($(this).find('input').is(':checked')) {
                $(this).addClass('active');
            } else {
                $(this).removeClass('active');
            }
        })
    })
    
    $('.panel-plan input').change(function() {
        $('.panel-plan').each(function() {
            if ($(this).find('input').is(':checked')) {
                $(this).addClass('active');
            } else {
                $(this).removeClass('active');
            }
        })
    })
    
    $('.mask-cep').mask('00000-000', {placeholder: '_____-___'});
    $('.mask-cel').mask('(00) 0000-00009', {placeholder: '(__) ____-_____'});
    $('.mask-money').mask('000.000.000.000.000,00', {reverse: true});
    $('.mask-exp').mask('AAA/AA', {placeholder: '___/__'})
    
})