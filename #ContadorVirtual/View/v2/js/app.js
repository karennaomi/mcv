var App = angular.module('contador', [
    'ngRoute',
    'contadorControllers'
]);
App.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
                when('/', {
                    templateUrl: 'layouts/index.html',
                    controller: 'indexCtrl',
                    resolve: {footer: function () {
                            return 1;
                        }},
                }).
                when('/cadastro', {
                    templateUrl: 'layouts/cadastro.html',
                    controller: 'cadastroCtrl'
                }).
                when('/marca/:id', {
                    templateUrl: 'layouts/marca.html',
                    controller: 'marcaCtrl'
                }).
                when('/frota/:id', {
                    templateUrl: 'layouts/frota.html',
                    controller: 'frotaCtrl'
                }).
                otherwise({
                    redirectTo: '/error'
                });
    }]);