var homeApp = angular.module('homeApp', [
  'homeControllers'
]);


var homeControllers = angular.module('homeControllers', []);

homeControllers.controller('HomeCtrl', ['$scope', '$http', '$sce',

    function ($scope, $http, $sce) {       

        $scope.sendContactForm = function (contactName, contactEmail, contactMessage) {

            console.log("Enviando e-mail");
            $('.loadContainer').css({ 'display': 'inline-block' });

            $http({
                method: 'GET',
                url: '/Home/sendContactForm/',
                params: {
                    contactName: contactName,
                    contactEmail: contactEmail,
                    contactMessage: contactMessage
                }
            })
            .success(function (data, status, headers, config) {
                $('.loadContainer').css({ 'display': 'none' });

                // Limpa o conteúdo do modal
                $("#modalContact .modal-body p").empty();

                // Preenche o modal com a mensagem de retorno do backend
                $("#modalContact .modal-body p").text(data);

                // Chamada do modal
                $('#modalContact').modal('show');

                //alert(data);
            });
        };
    }]);

