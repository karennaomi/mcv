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
    <script>
        $("#cnpj-input").mask("00.000.000/0000-00", {
            onComplete: function (texto) {
                $("#buscar-btn").removeAttr("disabled");
            },
            onKeyPress: function (cep, event, currentField, options) {
                $("#buscar-btn").attr("disabled", "disabled");
                PreencheDados(null, true)
            }
        });

$("#buscar-btn").on("click", function () {
    $("#buscar-modal").modal({ show: true });
});

$("#buscar-modal").on("show.bs.modal", function () {
    BuscarCaptcha();
});

$("#buscar-modal").on("shown.bs.modal", function () {
    $("#img-input").focus();
});

$("#buscar-modal").on("hidden.bs.modal", function () {
    $("#img-input").val("");
});

$("#buscar-captcha-btn").on("click", function () {
    $("#captcha-img").fadeOut(1000, function () {
        $(this).attr('src', "");
        BuscarCaptcha();
    });

});

$("#buscarDados-btn").on("click", function () {
    ObterDados();
});

var pathLoader = "@Url.Content("~/Content/ajax-loader-facebook.gif ")";
var $loader = $('<img class="loader-facebook" src="' + pathLoader + '"/> <em>Buscando ...</em>');

var BuscarCaptcha = function () {
    var strUrl = '@Url.Action("GetCaptcha")';
    $.ajax({
        type: 'GET',
        url: strUrl,
        dataType: 'json',
        cache: false,
        async: true,
        beforeSend: function () {
            $loader.insertAfter($("#captcha-img"));
        },
        success: function (data) {
            $("#captcha-img").removeClass("hidden").attr('src', data);
            $("#captcha-img").fadeIn(1000);
        },
        complete: function () {
            $loader.remove();
            $("#img-input").focus();
        },
        error: function () {
            alert("erro na tentativa de obter o captcha");
        }
    });
};

var ObterDados = function () {
    var strUrl = '@Url.Action("ConsultarDados")';
    $.ajax({
        type: 'post',
        url: strUrl,
        cache: false,
        async: true,
        data: { cnpj: $("#cnpj-input").val(), captcha: $("#img-input").val() },
        beforeSend: function () {
            $loader.insertBefore($("#fechar-button"));
        },
        success: function (data) {
            $loader.remove();
            if (data.erro.length > 0) {
                $("#msgErro-span").text(data.erro).closest("p").removeClass("hidden");
                $("#captcha-img").fadeOut(1000, function () {
                    $(this).attr('src', "");
                    BuscarCaptcha();
                    $("#img-input").focus();
                });
                setTimeout(function () {
                    $("#msgErro-span").closest("p").addClass("hidden");
                }, 2000);
            } else {
                if (data.dados != null) {
                    PreencheDados(data.dados, false);
                    $("#buscar-modal").modal("hide");
                } else {
                    $("#msgErro-span").text("erro de comunicação com a receita.").closest("p").removeClass("hidden");
                    $("#captcha-img").fadeOut(1000, function () {
                        $(this).attr('src', "");
                        BuscarCaptcha();
                        $("#img-input").focus();
                    });
                    setTimeout(function () {
                        $("#msgErro-span").closest("p").addClass("hidden");
                    }, 2000);
                }

            }
        },
        error: function (data) {
            $loader.remove();
            alert("erro de comunicação.");
        },
    });
};

var PreencheDados = function (dados, clear) {
    if (clear) {
        $(".caixa-grande").val("");
    } else {
        $("#razaosocial-input").val(dados.Razaosocial);
        $("#fantasia-input").val(dados.NomeFantasia);
        $("#cnae-input").val(dados.Cnae);
        $("#endereco-input").val(dados.Endereco);
        $("#bairro-input").val(dados.Bairro);
        $("#cep-input").val(dados.Cep);
        $("#cidade-input").val(dados.Cidade);
        $("#estado-input").val(dados.Estado);
    }
};
